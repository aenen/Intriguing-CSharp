using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

// "17. Як використовувати "Події" - поетапний приклад"
namespace _17
{
    /* Припустимо, ми проектуємо поштову програму. Отримавши повідомлення по електронній пошті користувач
       може захотіти переслати його на факс, або пейджер. Припустимо, ви почали проектування програми з
       розробки типу MailManager, який отримує вхідні повідомлення. Тип MailManager буде підтримувати подію
       NewMail. Інші типи (наприклад, Fax чи Pager) можуть зареєструватись для отримання сповіщень про цю
       подію. Коли тип MailManager отримає нове повідомлення, він ініціює подію, в результаті чого повідомлення
       буде отримано всіми зареєстрованими об'єктами. Потім кожен об'єкт обробляє повідомлення як хоче.
       
       Що ж, почнемо! */



    // --- Етап 1 ---
    // Визначення типу для збереження інформації, що передається одержувачам сповіщення про подію
    // УВАГА! Даний клас повинен наслідувати тип System.EventArgs, а його ім'я закінчуватись словом EventArgs.
    internal class NewMailEventArgs : EventArgs
    {
        private readonly String m_from, m_to, m_subject;

        public NewMailEventArgs(String from, String to, String subject)
        {
            m_from = from; m_to = to; m_subject = subject;
        }

        public String From { get { return m_from; } }
        public String To { get { return m_to; } }
        public String Subject { get { return m_subject; } }
    }



    // --- Етапи 2, 3 і 4 ---
    internal class MailManager
    {

        // Етап 2. Визначення члена події
        public event EventHandler<NewMailEventArgs> NewMail;
        /* Тут NewMail - ім'я події, а його типом є EventHandler<NewMailEventArgs>. Це означає, що одержувачі 
           сповіщення про подію повинні надавати метод зворотнього виклику, прототип якого відповідний до типу-
           делегату EventHandler<NewMailEventArgs>. Так як узагальнений делегат System.EventHandler визначений наступним чином:           
        public delegate void EventHandler<TEventArgs>(Object sender, TEventArgs e) where TEventArgs: EventArgs;           
           Тому прототип методу повинен виглядати ось так:
        void MethodName(Object sender, NewMailEventArgs e); */



        // Етап 3. Визначення методу відповідального за сповіщення зареєстрованих об'єктів про подію
        // УВАГА! Якщо цей клас ізольований (sealed), потрібно створити метод закритим і невіртуальним
        protected virtual void OnNewMail(NewMailEventArgs e)
        {

            // Зберегти посилання на делегат в тимчасовій змінній для забезпечення безпеки потоків
            EventHandler<NewMailEventArgs> temp = Volatile.Read(ref NewMail);
            
            // Якщо є об'єкти, які зареєстровані для отримання сповіщень про подію, сповіщуємо їх
            if (temp != null) temp(this, e);

            /* ПРИМІТКА. Visual Studio пропонує спростити 2 попередні рядки коду (використовуючи "синтаксичний цукор") таким чином:             
               Volatile.Read(ref NewMail)?.Invoke(this, e); */
        }



        // Етап 4. Визначення методу, що перетворює вхідну інформацію в бажану подію
        public void SimulateNewMail(String from, String to, String subject)
        {
            Console.WriteLine("Отримано нове повідомлення! Сповіщую підписників...");

            // Створити об'єкт для збереження інформації, яку потрібно передати отримувачам сповіщень
            NewMailEventArgs e = new NewMailEventArgs(from, to, subject);

            // Викликати віртуальний метод, який сповіщує об'єкт про подію
            // Якщо жоден з похідних типів не перевизначає цей метод, об'єкт сповістить всіх зареєстрованих одержувачів сповіщення
            OnNewMail(e);
        }
    }



    // --- Етап 5 ---
    // Створення типу, що відслідковує подію
    internal sealed class Fax
    {
        // Передаємо конструктору об'єкт MailManager
        public Fax(MailManager mm)
        {

            // Створюємо екземпляр делегату EventHandler<NewMailEventArgs>, який посилається на метод зворотнього виклику FaxMsg.
            // Реєструємо зворотній виклик для події NewMail об'єкту MailManager.
            mm.NewMail += FaxMsg;
            Console.WriteLine("Fax підписався на подію NewMail.");
        }
        
        // MailManager викликає цей метод для сповіщення об'єкту Fax про отримання нового поштового повідомлення
        private void FaxMsg(Object sender, NewMailEventArgs e)
        {

            // 'sender' використовується для взаємодії з об'єктом MailManager, якщо буде потрібно передати йому якусь інформацію.
            // 'e' визначає додаткову інформацію про подію, яку побажає надати MailManager.
            
            // Зазвичай розміщений тут код відправляє повідомлення по факсу. Тестова реалізація виводить інформацію на консоль.
            Console.WriteLine("Faxing mail message:");
            Console.WriteLine(" From={0}, To={1}, Subject={2}", e.From, e.To, e.Subject);
        }
        
        // Цей метод може виконуватись для відміни реєстрації об'єкту Fax в якості одержувача сповіщень події NewMail.
        public void Unregister(MailManager mm)
        {

            // Відмінити реєстрацію на сповіщення про подію NewMail об'єкту MailManager.
            mm.NewMail -= FaxMsg;
            Console.WriteLine("Fax відписався від події NewMail.");
        }
    }
    #region Аналогічний клас з назвою Pager
    internal sealed class Pager
    {
        // Передаємо конструктору об'єкт MailManager
        public Pager(MailManager mm)
        {

            // Створюємо екземпляр делегату EventHandler<NewMailEventArgs>, який посилається на метод зворотнього виклику PagerMsg.
            // Реєструємо зворотній виклик для події NewMail об'єкту MailManager.
            mm.NewMail += PagerMsg;
            Console.WriteLine("Pager підписався на подію NewMail.");
        }

        // MailManager викликає цей метод для сповіщення об'єкту Fax про отримання нового поштового повідомлення
        private void PagerMsg(Object sender, NewMailEventArgs e)
        {

            // 'sender' використовується для взаємодії з об'єктом MailManager, якщо буде потрібно передати йому якусь інформацію.
            // 'e' визначає додаткову інформацію про подію, яку побажає надати MailManager.

            // Зазвичай розміщений тут код відправляє повідомлення по факсу. Тестова реалізація виводить інформацію на консоль.
            Console.WriteLine("Pagering mail message:");
            Console.WriteLine(" From={0}, To={1}, Subject={2}", e.From, e.To, e.Subject);
        }

        // Цей метод може виконуватись для відміни реєстрації об'єкту Fax в якості одержувача сповіщень події NewMail.
        public void Unregister(MailManager mm)
        {

            // Відмінити реєстрацію на сповіщення про подію NewMail об'єкту MailManager.
            mm.NewMail -= PagerMsg;
            Console.WriteLine("Pager відписався від події NewMail.");
        }
    }
    #endregion



    // --- Етап ФІНАЛ ---
    // Тестуємо написане
    class Program
    {
        static void Main(string[] args)
        {
            MailManager mailManager = new MailManager();

            // Підписуємось на події (реалізовано в конструкторі):
            Fax fax = new Fax(mailManager);
            Pager pager = new Pager(mailManager);

            // Отримуємо нове повідомлення на пошту:
            mailManager.SimulateNewMail("yaroslavchik@aol.com", "president@gov.ua", "Ходімо гуляти!");

            // Pager відписується від події:
            pager.Unregister(mailManager);

            // Отримуємо нове повідомлення на пошту:
            mailManager.SimulateNewMail("yaroslavchik@aol.com", "president@gov.ua", "Чому не відповідаєш? Ну все, я піду один :(");
        }
    }
}
