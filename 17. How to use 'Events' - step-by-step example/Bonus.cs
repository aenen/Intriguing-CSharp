using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

// "17. Як використовувати "Події" - поетапний приклад [БОНУС]"
namespace _17
{
    // Цей приклад показує, як виглядає подія після компіляції ☺
    class Bonus
    {
        // В класі MailManager (файл Program.cs) є рядок коду, який визначає сам член-подію:
        public event EventHandler<NewMailEventArgs> NewMail;
        
        
        // Під час компіляції цого рядка компілятор перетворює його в наступні три конструкції:

        // 1. ЗАКРИТЕ поле делегату, яке ініціалізоване значенням null:
        private EventHandler<NewMailEventArgs> NewMail = null;

        // 2. ВІДКРИТИЙ метод add_Xxx (де Xxx - це ім'я події)
        // Дозволяє об'єктам реєструватись для отримання сповіщень про подію
        public void add_NewMail(EventHandler<NewMailEventArgs> value)
        {
            // Цикл і виклик CompareExchange - хитромудрий спосіб додавання делегату способом, безпечним по відношенні до потоків
            EventHandler<NewMailEventArgs> prevHandler;
            EventHandler<NewMailEventArgs> newMail = this.NewMail;
            do
            {
                prevHandler = newMail;
                EventHandler<NewMailEventArgs> newHandler =
                (EventHandler<NewMailEventArgs>)Delegate.Combine(prevHandler, value);
                newMail = Interlocked.CompareExchange<EventHandler<NewMailEventArgs>>(
                ref this.NewMail, newHandler, prevHandler);
            } while (newMail != prevHandler);
        }

        // 3. ВІДКРИТИЙ метод remove_Xxx (де Xxx - це ім'я події)
        // Дозволяє об'єктам відміняти реєстрацію в якості одержувачів сповіщень про подію
        public void remove_NewMail(EventHandler<NewMailEventArgs> value)
        {
            // Цикл і виклик CompareExchange - хитромудрий спосіб видалення делегату способом, безпечним по відношенні до потоків
            EventHandler<NewMailEventArgs> prevHandler;
            EventHandler<NewMailEventArgs> newMail = this.NewMail;
            do
            {
                prevHandler = newMail;
                EventHandler<NewMailEventArgs> newHandler =
                (EventHandler<NewMailEventArgs>)Delegate.Remove(prevHandler, value);
                newMail = Interlocked.CompareExchange<EventHandler<NewMailEventArgs>>(
                ref this.NewMail, newHandler, prevHandler);
            } while (newMail != prevHandler);
        }



        /* Як бачите, після компіляції створюється закрите поле події - NewMail, і це може вплинути на продуктивність, якщо в типі 
           знаходиться велика кількість подій. Про те, як цого можна запобігти розповідається в наступному прикладі (№18). */
    }
}
