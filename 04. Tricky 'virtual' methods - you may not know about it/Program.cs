using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// "04. Хитрі 'virtual' методи - можливо, ти про це не знав"
namespace _04
{
    class Program
    {
        static void Main(string[] args)
        {
            BetterPhone betterPhone = new BetterPhone();    // Створення об'єкту BetterPhone
            betterPhone.Dial();                             // Виклик методу Dial об'єкту BetterPhone
            /* Що буде виведено після виклику цього методу?
               Результат:
               Phone.Dial
               BetterPhone.EstablishConnection
               
               Спочату викликається успадкований в класа Phone метод Dial, 
               який в собі викликає перевизначений метод EstablishConnection класу BetterPhone.
               Хоча, може здатись, що буде викликаний метод EstablishConnection класу Phone. 
               
               В цьому і є головна суть поліморфізму та віртуальних методів. Можна легко змінити частину функціоналу не торкаючись базового класу. */
        }
    }

    // Базовий клас "Phone"
    public class Phone
    {
        public void Dial()
        {
            Console.WriteLine("Phone.Dial");
            // Викликає virtual-метод EstablishConnection()
            EstablishConnection();
        }

        protected virtual void EstablishConnection()
        {
            Console.WriteLine("Phone.EstablishConnection");
        }
    }

    // Клас "BetterPhone". Наслідує клас "Phone"
    public class BetterPhone : Phone
    {
        // Перевизначений віртуальний метод EstablishConnection класу "Phone"
        protected override void EstablishConnection()
        {
            Console.WriteLine("BetterPhone.EstablishConnection");
        }
    }
}
