using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// "13. Ключове слово 'params' - передай НЕСКІНЧЕННУ кількість параметрів в метод"
namespace _13
{
    class Program
    {
        // Перегляньте цей метод та зверніть увагу на ключове слово params біля параметру.
        /* УВАГА! Ключовим словом params може бути позначений лише останній параметр методу.
                  Він повинен вказувати на одновимірний масив довільного типу. */
        static Int32 Add(params Int32[] values)
        {
            // УВАГА! При необхідності масив values можна передати іншим методам
            Int32 sum = 0;
            if (values != null)
            {
                for (Int32 x = 0; x < values.Length; x++)
                    sum += values[x];
            }
            return sum;
        }

        static void Main(string[] args)
        {
            // Очевидно, метод Add можна викликати ось так:
            Console.WriteLine(Add(new Int32[] { 1, 2, 3, 4, 5 })); // Виводить "15"

            // Розробники, звісно ж, віддали б перевагу викликати цей метод так:
            Console.WriteLine(Add(1, 2, 3, 4, 5)); // Виводить "15"
            // Така форма виклику можлива завдяки ключовому слову params.

            // Наступний приклад може допомогти вам трішки підвищити продуктивність:
            // Обидва рядки виводять "0"
            Console.WriteLine(Add()); // передає новий елемент Int32[0] методу Add
            Console.WriteLine(Add(null)); // передає методу Add значення null, що більш 
                                          // ефективно (не виділяється пам'ять під масив)
            /* Якщо ви викликаєте метод, який кінцевим параметром приймає params-масив, але вами не 
               використовується - варто передати йому значення null, аби не виділялась пам'ять під масив.*/



            /* УВАГА! Виклик методу, який приймає змінне число аргументів знижує продуктивність, якщо, звісно,
               не передавати йому в явному вигляді значення null. В будь якому випадку всім об'єктам масиву
               потрібно виділити місце в купі та ініціалізувати всі елементи масиву, а по завершені роботи
               зайнята масивом пам'ять повинна бути очищена збирачем сміття (garbage collector). Щоб зменшити
               негативний вплив цих операцій на продуктивність можна визначити декілька перевантажених методів,
               які не використовують ключове слово params. За прикладом зверніться до методу Concat класу System.String:
               
               public sealed class String : Object, ... {
                 public static string Concat(object arg0);
                 public static string Concat(object arg0, object arg1);
                 public static string Concat(object arg0, object arg1, object arg2);
                 public static string Concat(params object[] args);
                 public static string Concat(string str0, string str1);
                 public static string Concat(string str0, string str1, string str2);
                 public static string Concat(string str0, string str1, string str2, string tr3);
                 public static string Concat(params string[] values);
               }
            */
        }
    }
}
