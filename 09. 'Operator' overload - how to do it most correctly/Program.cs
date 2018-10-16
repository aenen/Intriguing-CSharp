using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// "09. Перевантаження 'операторів' - як зробити це найбільш правильно"
namespace _09
{
    public sealed class Complex
    {
        int a, b, c;

        // Перевантажений бінарний оператор +:
        public static Complex operator+(Complex c1, Complex c2)
        {
            return new Complex
            {
                a = c1.a + c2.a,
                b = c1.b + c2.b,
                c = c1.c + c2.c
            };
        }

        /* Для типу з перевантаженими операторами Microsoft рекомендує визначати відкриті екземплярні методи з дружніми
           іменами, які викликають методи перевантажених операторів в своїй внутрішній реалізації. Наприклад, тип з 
           перевантаженими бінарними операторами + та += повинен також визначати відкритий метод з дружньою назвою Add. 
           Список рекомендованих дружніх імен можна знайти тут: https://docs.microsoft.com/en-us/previous-versions/dotnet/netframework-4.0/ms229032(v=vs.100) */
        public static Complex Add(Complex c1, Complex c2)
        {
            return (c1 + c2);
        }

        /* Прикладом типу, в якому перевантажуються оператори та використовуються дружні імена методів відповідно
           з правилами Microsoft може служити клас System.Decimal бібліотеки FCL.
           Щоб побачити їх в Visual Studio натисни правою кнопокою миші на слово Decimal та обреи 'Go To Definition': */
           Decimal click;
    }
}
