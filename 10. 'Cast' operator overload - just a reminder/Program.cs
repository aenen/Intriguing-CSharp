using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _10
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Створення об'єкту Rational з Int32:");
            Rational r1 = 5; // Неявне перетворення Int32 в Rational
            Console.WriteLine("\nСтворення об'єкту Rational з Single:");
            Rational r2 = 2.5F; // Неявне перетворення Single в Rational

            Console.WriteLine("\nСтворення об'єкту Int32 з Rational:");
            Int32 x = (Int32)r1; // Явне перетворення Rational в Int32
            Console.WriteLine("\nСтворення об'єкту Single з Rational:");
            Single s = (Single)r2; // Явне перетворення Rational в Single

            /* УВАГА! C# генерує код виклику оператору неявного перетворення у випадку, коли використовується
               вираз приведення типів. Проте оператори неявного перетворення ніколи не викликаються,
               якщо використовується оператор as або is. */
            Rational r3 = x as Rational; // Помилка компіляції CS0039



            /* Щоб по-справжньому розібратись в методах перевантажених операторів та операторів перетворення я рекомендую
               використовувати тип System.Decimal як приклад. В цьому типі визначено кілька конструкторів, які дозволяють
               перетворити в Decimal об'єкти різних типів. Він також підтримує кілька методів 'ToXxx'.
               Щоб побачити їх в Visual Studio натисни правою кнопокою миші на слово Decimal та обреи 'Go To Definition': */
            Decimal click;
        }
    }



    public sealed class Rational
    {
        // Контсруктори класу Rational:

        // Створює Rational з Int32
        public Rational(Int32 num)
        {
            Console.WriteLine("Виклик конструктору 'Rational(Int32 num)'");
        }
        // Створює Rational з Single
        public Rational(Single num)
        {
            Console.WriteLine("Виклик конструктору 'Rational(Single num)'");
        }



        // НЕЯВНІ оператори перетворення:

        // Неявно створює Rational з Int32 і повертає отриманий об'єкт
        public static implicit operator Rational(Int32 num)
        {
            Console.WriteLine("Неявне перетворення Int32 в Rational");
            return new Rational(num);
        }
        // Неявно створює Rational з Single і повертає отриманий об'єкт
        public static implicit operator Rational(Single num)
        {
            Console.WriteLine("Неявне перетворення Single в Rational");
            return new Rational(num);
        }



        // ЯВНІ оператори перетворення:

        // Явно повертає об'єкт типу Int32, отриманий з Rational
        public static explicit operator Int32(Rational r)
        {
            Console.WriteLine("Явне перетворення Rational в Int32");
            return r.ToInt32();
        }
        // Явно повертає об'єкт типу Single, отриманий з Rational
        public static explicit operator Single(Rational r)
        {
            Console.WriteLine("Явне перетворення Rational в Single");
            return r.ToSingle();
        }



        // Додаткові методи перетворення 'ToXxx':

        // Перетворює Rational в Int32
        public Int32 ToInt32()
        {
            Console.WriteLine("Виклик методу 'ToInt32()'");
            return 999;
        }
        // Перетворює Rational в Single
        public Single ToSingle()
        {
            Console.WriteLine("Виклик методу 'ToSingle()'");
            return 999.99F;
        }
    }
}
