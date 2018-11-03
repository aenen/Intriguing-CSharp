using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// "15. Анонімні типи - створення об'єктів без типу"
namespace _15
{
    class Program
    {
        static void Main(string[] args)
        {
            // --- 1 --- Анонімні типи

            /* Механізм анонімних типів в C# дозволяє автоматично оголосити кортежний ти за допомогою простого
               синтаксису. Кортежний тип (tuple type) - це тип, який містить колекцію властивостей, якимось чином
               пов'язаних один з одним. В першому рядку наступного програмного коду я визначаю клас з двома властивостями
               (Name типу String і Year типу Int32), створюю екземпляр цього типу та призначаю властивості Name
               значення Jeff, а властивості Year - значення 1964.*/

            // Визначення типу, створення сутності та ініціалізація властивостей
            var o1 = new { Name = "Jeff", Year = 1964 };
            // Виведення властивостей на консоль
            Console.WriteLine("Name={0}, Year={1}", o1.Name, o1.Year); // Виводить: Name=Jeff, Year=1964

            /* Тут створюється анонімний тип, тому що не був визначений тип імені після слова new, таким чином,
               компілятор автоматично створює ім'я типу, але не повідомляє яке воно (саме тому тип і названий анонімним).*/





            // --- 2 --- Альтернативний синтаксис оголошення

            /* Компілятор підтримує два додаткових варіанти синтаксису оголошення властивостей в анонімному типі,
               де на підставі змінних визначаються імена та типи властивостей: */

            String Name = "Grant";
            DateTime dt = DateTime.Now;
            // Анонімний тип з двома властивостями:
            // 1. Властивості Name типу String призначене значення Grant
            // 2. Властивості Year типу Int32 призначений рік з dt
            var o2 = new { Name, dt.Year };

            /* В данному прикладі компілятор визначає, що перша властивість повинна називатись Name. Так як Name - ім'я
               локальної змінної, то компілятор встановлює значення типу властивості аналогічно типу локальної змінної,
               тобто String. Для другої властивості компілятор використовує ім'я поля/властивості: Year. Year - це
               властивість класу DateTime з типом Int32, а отже, властивість Year в анонімному типі буде відноситись до типу Int32. */





            // --- 3 --- Використання однакових анонімних типів

            /* Під час оголошення анонімного типу "під капотом" компілятор створює аналогічний клас зі всіма властивостями
               та перевизначивши методи Equals, GetHashCode і ToString. В коді з прикладів тип змінної о1 і тип змінної о2
               однаковий, тому що в двох рядках коду визначений анонімний тип з властивістю Name/String і Year/Int32, 
               і Name стоїть перед Year. Якщо дві змінні належать до одного типу - відкривається маса корисних можливостей: */

            // Збіг типів дозволяє здійснювати операції порівняння і присвоєння
            Console.WriteLine("Objects are equal: " + o1.Equals(o2));
            o1 = o2; // Присвоєння

            // Якщо ці типи ідентичні, то можна створити масив явних типів з анонімних типів:
            // Це працює, тому що всі об'єкти мають один анонімний тип
            var people = new[] {
                 o1,
                 new { Name = "Kristin", Year = 1970 },
                 new { Name = "Aidan", Year = 2003 },
                 new { Name = "Grant", Year = 2008 }
                };
            // Організація перебору масиву анонімних типів
            // (ключове слово var обов'язкове).
            foreach (var person in people)
                Console.WriteLine("Person={0}, Year={1}", person.Name, person.Year);





            // --- 4 --- Анонімні типи та LINQ

            /* Анонімні типи зазвичай використовуються з технологією мови інтегрованих запитів (Language Integrated Query, LINQ),
               коли в результаті виконання запиту створюється колекція об'єктів, які відносяться до одного анонімного типу,
               після чого здійснюється обробка обєктів в отриманій колекції. В наступному прикладі всі файли з папки з моїми
               документами, які були змінені за останні сім днів: */

            String myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var query =
             from pathname in Directory.GetFiles(myDocuments)
             let LastWriteTime = File.GetLastWriteTime(pathname)
             where LastWriteTime > (DateTime.Now - TimeSpan.FromDays(7))
             orderby LastWriteTime
             select new { Path = pathname, LastWriteTime };

            foreach (var file in query)
                Console.WriteLine("LastWriteTime={0}, Path={1}",
                file.LastWriteTime, file.Path);
        }





        // --- 5 --- Тип System.Tuple

        /* В просторі імен System визначено кілька узагальнених (generic) кортежних типів (всі вони наслідуються від класу Object),
           які відрізняються кількістю узагальнених параметрів. Об'єкт класу Tuple дозволяє використовувати методи CompareTo, Equals, 
           GetHashCode і ToString, як і властивість Size. До того ж всі типи Tuple реалізують інтерфейси IStructuralEquatable, 
           IStructuralComparable і IComparable, тому ви можете порівняти два об'єкти типу Tuple один з одним. Класи Tuple виглядають так:
           
           // Проста форма:
           [Serializable]
           public class Tuple<T1> {
            private T1 m_Item1;
            public Tuple(T1 item1) { m_Item1 = item1; }
            public T1 Item1 { get { return m_Item1; } }
           }

           // Складна форма:
           [Serializable]
           public class Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> {
            private T1 m_Item1; private T2 m_Item2;
            private T3 m_Item3; private T4 m_Item4;
            private T5 m_Item5; private T6 m_Item6;
            private T7 m_Item7; private TRestm_Rest;

            public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, TRest t) {
                m_Item1 = item1; m_Item2 = item2; m_Item3 = item3; m_Item4 = item4;
                m_Item5 = item5; m_Item6 = item6; m_Item7 = item7; m_Rest = rest;
            }

            public T1 Item1 { get { return m_Item1; } }
            public T2 Item2 { get { return m_Item2; } }
            public T3 Item3 { get { return m_Item3; } }
            public T4 Item4 { get { return m_Item4; } }
            public T5 Item5 { get { return m_Item5; } }
            public T6 Item6 { get { return m_Item6; } }
            public T7 Item7 { get { return m_Item7; } }
            public TRest Rest { get { return m_Rest; } }
           }  */



        // Приведу приклад методу, який використовує тип Tuple для повернення двох частин інформації в викликаючий метод:

        // Повертає мінінмум в Item1 і максимум в Item2
        private static Tuple<Int32, Int32> MinMax(Int32 a, Int32 b)
        {
            return new Tuple<Int32, Int32>(Math.Min(a, b), Math.Max(a, b));
            // ...або:
            // return Tuple.Create(Math.Min(a, b), Math.Max(a, b)); // Спрощений синтаксис
        }

        // Приклад виклику методу та використання Tuple
        private static void TupleTypes()
        {
            var minmax = MinMax(6, 2);
            Console.WriteLine("Min={0}, Max={1}", minmax.Item1, minmax.Item2); // Min=2, Max=6

            //Щоб створити тип Tuple з більш ніж вісьмома елементами, передайте інший об'єкт Tuple в параметр Rest:
            var t = Tuple.Create(0, 1, 2, 3, 4, 5, 6, Tuple.Create(7, 8));
            Console.WriteLine("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}",
             t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6, t.Item7, t.Rest.Item1.Item1, t.Rest.Item1.Item2);
        }
    }
}
