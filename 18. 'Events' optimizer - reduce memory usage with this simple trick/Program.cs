using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

// "18. Оптимізатор подій - зменш використання пам'яті з цим простим трюком"
namespace _18
{

    /* В типі System.Windows.Forms.Control визначено близько 70 подій. Якщо тип Control реалізує події, дозволяючи
       компілятору явно генерувати методи доступу add і remove і поля-делегати, то кожен об'єкт Control буде мати
       70 полей-делегатів для кожної події! Так як багатьох програмістів цікавить відносно невелка кількість подій,
       для кожного об'єкту, створеного з похідного від Control типу, величезний об'єм пам'яті буде витрачатись даремно.
       
       В цьому прикладі розказано про те, яким чином компілятор C# дозволяє розробникам реалізовувати події, керуючи тим,
       як методи add і remove маніпулюють делегатами зворотніх викликів. Я покажу, як явна реалізація події допомагає
       ефективно реалізувати клас з підтримкою багатьох подій.*/




    // Цей клас потрібен для підтримки безпеки типу і коду при використанні EventSet
    public sealed class EventKey : Object { }


    public sealed class EventSet
    {
        // Закритий словник служить для відображення EventKey -> Delegate
        private readonly Dictionary<EventKey, Delegate> m_events = new Dictionary<EventKey, Delegate>();

        // Додавання відображення EventKey -> Delegate, якщо його не існує і компоновка делегату з існуючим ключем EventKey
        public void Add(EventKey eventKey, Delegate handler)
        {
            Monitor.Enter(m_events);
            Delegate d;
            m_events.TryGetValue(eventKey, out d);
            m_events[eventKey] = Delegate.Combine(d, handler);
            Monitor.Exit(m_events);
        }

        // Видалення делегату з EventKey (якщо він існує) і розрив зв'язку EventKey -> Delegate при видаленні останнього делегату
        public void Remove(EventKey eventKey, Delegate handler)
        {
            Monitor.Enter(m_events);
            // Виклик TryGetValue запобігає видачі виключення при спробі видалення делегату з відсутнім ключем EventKey
            Delegate d;
            if (m_events.TryGetValue(eventKey, out d))
            {
                d = Delegate.Remove(d, handler);
                // Якщо делегат залишається, то встановити новий ключ EventKey, інакше - видалити EventKey
                if (d != null) m_events[eventKey] = d;
                else m_events.Remove(eventKey);
            }
            Monitor.Exit(m_events);
        }

        // Інформування про подію для вказаного ключа EventKey
        public void Raise(EventKey eventKey, Object sender, EventArgs e)
        {
            // Не видавати виключення при відсутності ключа EventKey
            Delegate d;
            Monitor.Enter(m_events);
            m_events.TryGetValue(eventKey, out d);
            Monitor.Exit(m_events);

            if (d != null)
            {
                /* Через те що словник може містити декілька різних типів делегатів, неможливо створити виклик делегату, безпечний
                   по відношенню до типу, під час компіляції. Я викликаю метод DynamicInvoke типу System.Delegate, передаючи йому
                   параметри методу зворотнього виклику в вигляді масива об'єктів. DynamicInvoke буде контролювати безпеку типів
                   параметрів для викликаного методу зворотнього виклику. Якщо буде знайдена невідповідність типів, видається виключення. */
                d.DynamicInvoke(new Object[] { sender, e });
            }
        }
    }




    // Визначення типу, успадкованого від EventArgs для події Foo типу TypeWithLotsOfEvents
    public class FooEventArgs : EventArgs { }


    public class TypeWithLotsOfEvents
    {
        // Визначення закритого екземплярного поля, яке посилається на колекцію. Колекція керує множиною пар "Event/Delegate"
        // ПРИМІТКА: Тип EventSet не входить до FCL, це мій особистий тип
        private readonly EventSet m_eventSet = new EventSet();

        // Захищена властивість дозволяє похідним типам працювати з колекцією
        protected EventSet EventSet { get { return m_eventSet; } }

        #region Код для підтримки події Foo (повторіть цей шаблон для інших подій)
        // Визначення членів, необхідних для події Foo.
        // 1. Створіть статичний, доступний тільки для зчитування об'єкт для ідентифікації події.
        // Кожен об'єкт має свій хеш-код для знаходження зв'язаного списку делегатів події в колекції.
        protected static readonly EventKey s_fooEventKey = new EventKey();

        // 2. Визначення для події методів доступу для додавання або видалення делегату з колекції.
        public event EventHandler<FooEventArgs> Foo
        {
            add { m_eventSet.Add(s_fooEventKey, value); }
            remove { m_eventSet.Remove(s_fooEventKey, value); }
        }

        // 3. Визначення захищеного віртуального методу On для цієї події.
        protected virtual void OnFoo(FooEventArgs e)
        {
            m_eventSet.Raise(s_fooEventKey, this, e);
        }

        // 4. визначення методу, який перетворює вхідні дані цієї події
        public void SimulateFoo() { OnFoo(new FooEventArgs()); }
        #endregion
    }



    /* Програмний код, який використовує тип TypeWithLotsOfEvents, не може сказати, чи була подія реалізована неявно
       компілятором, чи явно розробником. Він просто реєструє події з використанням звичайного синтаксису. Приклад: */
    public sealed class Program
    {
        public static void Main()
        {
            TypeWithLotsOfEvents twle = new TypeWithLotsOfEvents();

            // Додавання зворотнього виклику
            twle.Foo += HandleFooEvent;

            // Перевіряємо працездатність
            twle.SimulateFoo();
        }

        private static void HandleFooEvent(object sender, FooEventArgs e)
        {
            Console.WriteLine("Handling Foo Event here...");
        }
    }



    /* Якщо приклад здався вам трішки незрозумілим, спробую коротко пояснити суть. При оголошенні події без явного визначення
       методів add та remove компілятор сам створює методи add та remove, А ЩЕ створює приватний делегат. В даному прикладі
       цей додатковий делегат для подій класу TypeWithLotsOfEvents створюватись НЕ буде. Перевірити це можна переглянувши клас
       TypeWithLotsOfEvents в Ildasm.exe і порівнявши його з класом MailManager (приклад №17, файл Program.cs). Також в прикладі №17
       в файлі Bonus.cs описано як саме компілятор реалізує методи add та remove в подіях. Так що check this out too, if you haven't already ☺ */

}