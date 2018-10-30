using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// "12. Безпечні параметри 'ref' і 'out' - дещо про безпеку типів"
namespace _12
{
    class Program
    {
        static void Main(string[] args)
        {
            String s1;
            String s2;


            /* ПРИКЛАД 1. Здавалось би, все написано правильно, але це не так. Код компілюватсь не буде.
               Справа в тому, що змінні які передаються методу по посиланню повинні бути одного типу.
               Інакше кажучи, метод Swap очікує отримати посилання на тип Object, а не String. */
            s1 = "Jeffrey";
            s2 = "Richter";
            Swap(ref s1, ref s2);
            Console.WriteLine(s1); // Виводить "Richter"
            Console.WriteLine(s2); // Виводить "Jeffrey"
            

            /* ПРИКЛАД 2. Щоб виправитии помилку з першого прикладу можна зробити наступним чином.
               Такий код буде компілюватись та працювати як треба. Причиною обмеження, яке
               нам довелось обходити, є забезпечення безпеки типів. */
            s1 = "Jeffrey";
            s2 = "Richter";
            // Типи змінних переданих по посиланню повинні відповідати очікуваним
            Object o1 = s1, o2 = s2;
            Swap(b: ref o2, a: ref o1); // Зверніть увагу, що можна передавати змінні ref/out за ім'ям параметрів
            // Приведення об'єктів до строкового типу
            s1 = (String)o1;
            s2 = (String)o2;
            Console.WriteLine(s1); // Виводить "Richter"
            Console.WriteLine(s2); // Виводить "Jeffrey"
            

            // ПРИКЛАД 3. Виявляється, приклад 1 можна простіше змусити працювати за допомогою узагальнень (generic).
            s1 = "Jeffrey";
            s2 = "Richter";
            SwapGeneric(ref s1, ref s2); // Аналогічно запису SwapGeneric<String>(ref s1, ref s2)
            Console.WriteLine(s1); // Виводить "Richter"
            Console.WriteLine(s2); // Виводить "Jeffrey"



            /* ПРИМІТКА. За іншими прикладами рішень, які викоритовують узагальнення, звертайтесь до класу 
               System.Threading.Interlocked з його методами  CompareExchange і Exchange. */
        }
                


        // Метод міняє об'єкти один з одним. (використовується в прикладах 1 і 2)
        public static void Swap(ref Object a, ref Object b)
        {
            Object t = b;
            b = a;
            a = t;
        }

        // Узагальнений метод міняє об'єкти один з одним. (використовується в прикладі 3)
        public static void SwapGeneric<T>(ref T a, ref T b)
        {
            T t = b;
            b = a;
            a = t;
        }
    }
}
