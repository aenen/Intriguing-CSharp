using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// "03. Страшне неявне упакування - завжди пам'ятай про нього"
namespace _03
{
    /* Типи даних C# поділяють на значимі та посилальні (зсилочні на суржику ☺).
       Якщо коротко, то структури (struct) мають значимий тип даних, а класи (class) - посилальний.
       Значимі типи зберігаються в стеці, а посилальні - в купі.
       Якщо спробувати зберегти об'єкт значимого типу даних в Object, то він скопіюється та збережеться в купі,
       а Object матиме посилання на цей новий об'єкт. Це називають "упакуванням". 
       Упакування призводить до частішого виклику Garbage Collector,
       що може вплинути на продуктивність, тому його краще уникати завжди коли це можливо */
    class Program
    {
        static void Main(string[] args)
        {
            Int32 v = 5; // Створення неупакованої змінної значимого типу o
            Object o = v; // Вказує на упаковане Int32 зі значенням 5
            v = 123; // Змінюємо неупаковане значення на 123 (значення змінної о не змінюється)
            Console.WriteLine(v + ", " + (Int32)o); // Відображається "123, 5"            /* А тепер запитання: скільки операцій упакування в даному коді? Правильно, цілих 3!
             
               Перше упакування, зрозуміло, що знаходиться тут: Object o = v.
               А друге та третє приховано в Console.WriteLine(v + ", " + (Int32)o),
               а точніше, в виразі: (v + ", " + (Int32)o)
               Справа в тому, що при об'єднані строк (рядків) CLR генерує в IL-коді виклик методу String.Concat,
               який в якості параметрів приймає Object (наприклад, версія String.Concat(Object arg0, Object arg1, Object arg2)).
               Саме тому при передачі параметрів виконується неявне упакування змінних Int32. */

            // Отриманий IL-код стане ефективнішим, якщо переписати зверенення до WriteLine:
            Console.WriteLine(v + ", " + o); // Відображається "123, 5"

            // Тепер сумарна кількість операцій упакування зменшилась до 2, але і це можна покращити:
            Console.WriteLine(v.ToString() + ", " + o); // Відображається "123, 5"
            /* В цьому коді v викликає свій перевизначений метод ToString(), який повертає посилальний об'єкт
                типу String, тому тепер упакування об'єкту v не відбувається.
                Ось таким чином можна зменшити розмір IL-коду та збільшити продуктивність, 
                просто звертаючи увагу на неявне упакування та уникаючи його. */



            // І натомість ще одне запитання: Скільки операцій упакування в цьому коді?:
            Console.WriteLine(v);
            // ...
            // ...
            // ...
            // ...
            // ...
            // Відповідь: 0!

            /* Справа в тому, що в класі System.Console описаний метод WriteLine, який приймає в якості параметра тип Int32.               
               Клас System.Console пропонує такі перевантажені варіанти методу WriteLine:

               public static void WriteLine(Boolean);
               public static void WriteLine(Char);
               public static void WriteLine(Char[]);
               public static void WriteLine(Int32);
               public static void WriteLine(UInt32);
               public static void WriteLine(Int64);
               public static void WriteLine(UInt64);
               public static void WriteLine(Single);
               public static void WriteLine(Double);
               public static void WriteLine(Decimal);
               public static void WriteLine(Object);
               public static void WriteLine(String); */
        }
    }
}
