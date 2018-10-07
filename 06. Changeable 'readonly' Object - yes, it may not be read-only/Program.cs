using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// "06. Змінний oб'єкт 'readonly' - так, він може бути не тільки незмінним"
namespace _06
{

    /* Поле яке позначене ключевим словом readonly можна ініціалізувати лише явно
       при його оголошенні, або в конструкторі типу. Змінити його не можна.

       Незмінність поля посильного типу означає незмінність посилання, яке цей тип має,
       а не об'єкту, на якого вказує посилання. */

    class Program
    {
        static void Main(string[] args)
        {
            // Виведення значень "readonly"-масиву типу Char[]:
            Console.WriteLine("AType.InvalidChars: ");            
            Array.ForEach(AType.InvalidChars, x => Console.Write($"{x}; "));
            // Метод Array.ForEach - це чудовий одно-рядковий спосіб для виведення символів.
            // Його можна записати ще коротше: Array.ForEach(AType.InvalidChars, Console.WriteLine);

            // Змінення значень "readonly"-масиву типу Char[]:
            Console.WriteLine("\n\nВиклик методу AnotherType.M().");
            AnotherType.M();
            
            // Виведення значень "readonly"-масиву типу Char[]:
            Console.WriteLine("AType.InvalidChars: ");
            Array.ForEach(AType.InvalidChars, x => Console.Write($"{x};"));
        }
    }

    sealed class AType
    {
        // InvalidChars завжди посилається на один об'єкт масиву
        public static readonly Char[] InvalidChars = new Char[] { 'А', 'В', 'C' };
    }
    sealed class AnotherType
    {
        public static void M()
        {
            // Наступні рядки коду коректні, компілюються і успішно змінюють символи масиву InvalidChars
            AType.InvalidChars[0] = 'X';
            AType.InvalidChars[1] = 'Y';
            AType.InvalidChars[2] = 'Z';

            /* Наступний рядок коду некоректний та не скомпілюється,
               так як посилання на InvalidChars змінитись не може */
            // AType.InvalidChars = new Char[] { 'X', 'Y', 'Z' };
        }
    }
}
