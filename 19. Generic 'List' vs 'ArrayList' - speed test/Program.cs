using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

// "19. Узагальнений (generic) 'List' проти 'ArrayList' - тест на швидкість"
namespace _19
{

    /* Узагальнені (generic) типи мають багато переваг, серед яких:
       - Захист початкового коду;
       - Безпека типів;
       - Більш простий і зрозумілий код;
       - Підвищення продуктивності. 
       
       Про останній пункт і піде мова в цьому прикладі: List<T> VS ArrayList. 
       Чи дійсно List<T> більш продуктивний? Дізнайся в кінці прикладу (або запусти його й подивись...)*/

    public static class Program
    {
        public static void Main()
        {
            ValueTypePerfTest();
            ReferenceTypePerfTest();
        }

        private static void ValueTypePerfTest()
        {
            const Int32 count = 100000000;

            using (new OperationTimer("List<Int32>"))
            {
                List<Int32> l = new List<Int32>();
                for (Int32 n = 0; n < count; n++)
                {
                    l.Add(n); // Без упаковки
                    Int32 x = l[n]; // Без розпаковки
                }
                l = null; // Для видалення в процесі збірки сміття
            }

            using (new OperationTimer("ArrayList of Int32"))
            {
                ArrayList a = new ArrayList();
                for (Int32 n = 0; n < count; n++)
                {
                    a.Add(n); // Упаковка
                    Int32 x = (Int32)a[n]; // Розпаковка
                }
                a = null; // Для видалення в процесі збірки сміття
            }
        }

        private static void ReferenceTypePerfTest()
        {
            const Int32 count = 100000000;

            using (new OperationTimer("List<String>"))
            {
                List<String> l = new List<String>();
                for (Int32 n = 0; n < count; n++)
                {
                    l.Add("X"); // Копіювання посилання
                    String x = l[n]; // Копіювання посилання
                }
                l = null; // Для видалення в процесі збірки сміття
            }

            using (new OperationTimer("ArrayList of String"))
            {
                ArrayList a = new ArrayList();
                for (Int32 n = 0; n < count; n++)
                {
                    a.Add("X"); // Копіювання посилання
                    String x = (String)a[n]; // Перевірка перетворення і копіювання посилання
                }
                a = null; // Для видалення в процесі збірки сміття
            }
        }
    }
    
    // Клас для оцінювання часу виконання операцій
    internal sealed class OperationTimer : IDisposable
    {
        private Stopwatch m_stopwatch;
        private String m_text;
        private Int32 m_collectionCount;

        public OperationTimer(String text)
        {
            PrepareForOperation();

            m_text = text;
            m_collectionCount = GC.CollectionCount(0);
            
            // Ця команда повинна бути останньою в цьому методі для максимально точної оцінки швидкодії
            m_stopwatch= Stopwatch.StartNew();
        }

        public void Dispose()
        {
            string result = String.Format("{0} (GCs={1,3}) {2}", (m_stopwatch.Elapsed),
            GC.CollectionCount(0) - m_collectionCount, m_text);

            Console.WriteLine(result);
            //Debug.WriteLine(result);
        }

        private static void PrepareForOperation()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }

    /* Що ж, результати вийшли цікавими...
       
       Суперкомп'ютер дядечка Ріхтера, отримав такий результат:
        00:00:01.6246959 (GCs= 6) List<Int32>
        00:00:10.8555008 (GCs=390) ArrayList of Int32
        00:00:02.5427847 (GCs= 4) List<String>
        00:00:02.7944831 (GCs= 7) ArrayList of String
     
       Тоді як я ось такий:
        00:00:03.2487328 (GCs=  6) List<Int32>
        00:00:42.0170983 (GCs=222) ArrayList of Int32
        00:00:06.8295531 (GCs=  3) List<String>
        00:00:06.0649763 (GCs=  0) ArrayList of String

          Досить неочікувано, що результати в мене і Ріхтера відрізняються досить сильно. Порівняйте лише
       List<String> з ArrayList of String. Можливо, причина в тому, що в мене новіша версія .NET Framework,
       ніж та, яка використовувалась Ріхтером на момент написання книги... Думаю, ця підштовхне вас запустити 
       програму і поглянути на власний результат продуктивності. Далі без коментарів привожу текст з книги:

          Як бачите, з типом Int32 узагальнений алгоритм List працює значно швидше, ніж неузагальнений алгоритм
       ArrayList. Більш того, різниця величезна: 1,6 секунд проти 11 секунд, тобто в 7 раз більше! Крім того,
       використання значимого типу (Int32) з алгоритмом ArrayList потребує багатьох операцій упаковки, і, як
       результат, 390 процедур збірки сміття, а в алгоритмі List всього 6.
          Результати тестування для посилального типу не настільки вражаючі: часові показники і число операцій
       збірки сміття тут приблизно однакові. Тому в даному випадку в узагальненого алгоритму List великих переваг
       немає. Проте, пам'ятайте, що використання узагальненого алгоритму значно спрощує код і контроль типів
       при компіляції. Таким чином, хоча виграшу в продуктивності практично нема, узагальнений алгоритм
       зазвичай має інші переваги. */
}
