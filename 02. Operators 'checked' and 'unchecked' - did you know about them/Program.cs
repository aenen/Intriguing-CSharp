using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02
{
    class Program
    {
        static void Main(string[] args)
        {
            // При переповненні об'єкту загорнутого в оператор unchecked виключення не виникає
            UInt32 invalidUint1 = unchecked((UInt32)(-27)); // OK
            Char invalidChar1 = unchecked((Char)(Char.MaxValue + 4)); // OK

            // При переповненні об'єкту загорнутого в оператор checked виникає виключення OverflowException
            Byte b1 = 100;
            b1 = checked((Byte)(b1 + 200)); // Виключення OverflowException



            // Також можна створити блоки checked/unchecked, наприклад,
            // для перевірки декількох змінних на об'єкт переповнення
            unchecked
            {
                UInt32 invalidUint2 = (UInt32)(-27); // OK
                Char invalidChar2 = (Char)(Char.MaxValue + 4); // OK
            }
            checked
            {
                Byte b2 = 100;
                // В блоці checked можна використати оператор +=, який трішки спростить код
                b2 += 200; // Виключення OverflowException
            }



            // УВАГА! Встановлення режиму контролю переповнення не впливає на роботу методу в тілі блоку checked
            checked
            {
                // AddToByte намагається помістити 400 в Byte
                Byte newByte = AddToByte(400);
                // Виникнення OverflowException в AddToByte залежить від наявності в ньому операторів перевірки
            }
        }

        private static Byte AddToByte(Int32 number)
        {
            // За замовчуванням OverflowException не з'явиться
            // Детальніше шукай інформацію про параметри компілятора /checked+ та /checked-
            Byte result = (Byte)number;
            return result;
        }
    }
}
