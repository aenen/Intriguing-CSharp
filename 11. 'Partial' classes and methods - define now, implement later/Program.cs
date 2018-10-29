using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// "11. 'Partial' класи та методи - визнач зараз, реалізуй потім"
namespace _11
{
    // Оголошення частвого класу (також частковими можуть бути структури та статичні класи)
    internal sealed partial class Base
    {
        private String m_name;

        // Це оголошення з визначенням часткового методу викликається перед зміною поля m_name
        // УВАГА! Часткові методи зажди повинні повертати тип void та не можуть мати 'out'-параметрів.
        // УВАГА! Часткові методи не можуть мати модифікатор доступу, вони завжди приватні.
        partial void OnNameChanging(String value);

        public String Name
        {
            get { return m_name; }
            set
            {
                // Інформування класу про потенційну зміну
                // УВАГА! Якщо метод OnNameChanging не буде реалізований, то цей виклик методу ігнорується.
                OnNameChanging(value.ToUpper());
                m_name = value; // Зміна поля
            }
        }
    }

    // Реалізований програмістом клас Base, який (припустимо) знаходиться в іншому файлі
    internal sealed partial class Base
    {
        // Це оголошення з реалізацією часткового методу викликається перед тим, як буде змінено поле m_name.
        // УВАГА! Якщо цей метод не буде реалізований, то його не буде в кінцевому IL-коді класу Base після компіляції.
        partial void OnNameChanging(String value)
        {
            if (String.IsNullOrEmpty(value))
                throw new ArgumentNullException("value");
        }
    }

    /* ПРИМІТКА. Якщо в редакторі Visual Studio ввести partial та натиснути пробіл, в вікні IntelliSense з'явиться
       оголошення всіх часткових методів типу, які поки не реалізовані. Можна обрати частковий метод у вікні IntelliSense,
       і Visual Studio згенерує прототип методу автоматично. */
}
