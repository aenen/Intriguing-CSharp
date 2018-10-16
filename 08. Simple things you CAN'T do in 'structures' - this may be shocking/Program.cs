using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// "08. Прості речі, які ти НЕ можеш зробити в 'структурах' - це може бути шокуючим"
namespace _08
{
    internal struct SomeValType
    {
        // №1. В структрурах НЕ можна викоистовувати явну ініціалізацію полів.
        //     error CS0573: 'SomeValType.m_x': cannot have instance field initializers in structs
        private Int32 m_x = 5, m_y;

        // №2. В структурах НЕ можна оголошувати конструктор без параметрів.
        //     error CS0568: Structs cannot contain explicit parameterless constructors
        public SomeValType() { }

        // №3. В структурах МОЖНА оголошувати конструктори з параметрами, АЛЕ вони повинні ініціалізувати ВСІ поля.
        //     error CS0171: Field 'SomeValType.m_y' must be fully assigned before control leaves the constructor
        public SomeValType(Int32 x)
        {
            m_x = x;
        }



        // Помилку №3 можна виправити таким чином:
        public SomeValType(Int32 x)
        {
            // Виглядає незвично, але компілюється прекрасно,
            // і всі поля ініціалізуюсться значеннями 0 або null
            this = new SomeValType();

            m_x = x; // Привласнює m_x значення x
                     // Зверніть увагу, що поле m_y було ініціалізоване нулем
        }
    }
}
