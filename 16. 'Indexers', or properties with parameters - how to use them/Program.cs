using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

// "Індексатори, або властивості з параметрами - як їх використовувати"
namespace _16
{
    class Program
    {
        static void Main(string[] args)
        {
            // Використовувати індексатор типу BitArray дуже просто:

            // Виділити масив BitArray, який може зберегти 14 біт
            BitArray ba = new BitArray(14);

            // Встановити всі парні біти викликом методу set
            for (Int32 x = 0; x < 14; x++)
            {
                ba[x] = (x % 2 == 0);
            }

            // Вивести стан всіх бітів викликом методу доступу get
            for (Int32 x = 0; x < 14; x++)
            {
                Console.WriteLine("Bit " + x + " is " + (ba[x] ? "On" : "Off"));
            }



            // Використання методу foreach (необхідна реалізація інтерфейсу IEnumerable)
            foreach (var item in ba)
            {
                Console.Write(item ? "On; " : "Off; ");
            }
        }
    }

    public sealed class BitArray : IEnumerable<Boolean>
    {
        // Закритий байтовий масив, який зберігає біти
        private Byte[] m_byteArray;
        private Int32 m_numBits;
        
        public int Count => m_numBits;

        // Конструктор, який виділяє пам'ять для байтового масиву та встановлює всі біти в 0
        public BitArray(Int32 numBits)
        {
            // Починаємо з перевірки аргументів
            if (numBits <= 0)
                throw new ArgumentOutOfRangeException("numBits must be > 0");
            
            // Зберегти число бітів
            m_numBits = numBits;
            // Виділити байти для масиву бітів
            m_byteArray = new Byte[(numBits + 7) / 8];
        }

        // Індексатор (властивість з параметрами)
        // УВАГА! Розкоментуй наступний атрибут, щоб в IL-коді методи get/set називались get_Bit/set_Bit, а не get_Item/set_Item:
        // [IndexerName("Bit")]
        public Boolean this[Int32 bitPos]
        {

            // Метод доступу get індексатора
            get
            {
                // Спочатку необхідно перевірити аргументи
                if ((bitPos < 0) || (bitPos >= m_numBits))
                    throw new ArgumentOutOfRangeException("bitPos");
                // Повернути стан біта, який індексується
                return (m_byteArray[bitPos / 8] & (1 << (bitPos % 8))) != 0;
            }

            // Метод доступу set індексатора
            set
            {
                if ((bitPos < 0) || (bitPos >= m_numBits))
                    throw new ArgumentOutOfRangeException(
                    "bitPos", bitPos.ToString());
                if (value)
                {
                    // Встановити індексований біт
                    m_byteArray[bitPos / 8] = (Byte)
                    (m_byteArray[bitPos / 8] | (1 << (bitPos % 8)));
                }
                else
                {
                    // Скинути індексований біт
                    m_byteArray[bitPos / 8] = (Byte)
                    (m_byteArray[bitPos / 8] & ~(1 << (bitPos % 8)));
                }
            }
        }
        


        // --- Реалізація інтерфейсу IEnumerable<Boolean> ---

        public IEnumerator<bool> GetEnumerator()
        {
            return new BitArrayEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new BitArrayEnumerator(this);
        }
    }

    // Реалізація інтерфейсу IEnumerator<Boolean>
    class BitArrayEnumerator : IEnumerator<Boolean>
    {
        private BitArray m_bitArray;
        int position = -1;

        public BitArrayEnumerator(BitArray bitArray)
        {
            m_bitArray = bitArray;
        }

        public bool Current => m_bitArray[position];

        object IEnumerator.Current => m_bitArray[position];

        public void Dispose() { /* нічого тут діспоузити */ }

        public bool MoveNext()
        {
            if (position < m_bitArray.Count - 1)
            {
                position++;
                return true;
            }

            return false;
        }

        public void Reset()
        {
            position = -1;
        }
    }
}
