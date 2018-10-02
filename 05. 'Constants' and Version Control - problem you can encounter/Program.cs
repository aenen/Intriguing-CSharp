using SomeLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// "'Константи' та Контроль Версій - проблема, з якою ти можеш зіткнутись"
namespace _05
{
    /* Код даної програми має посилання на константу MaxEntriesInList з SomeLibrary.SomeLibraryType.
       При компонуванні цього коду компілятор, побачивши, що MaxEntriesInList - це літерал константи
       зі значенням 50, впровадить значення типу Int32 прямо в IL-код програми. Фактично після
       побудови коду DLL-збірка навіть не буде завантажуватись з диску в період виконання, тому
       її можна просто видалити.
       
       Через це може виникнути проблема з контролем версій. Якщо змінити значення SomeLibrary.SomeLibraryType.MaxEntriesInList
       та перезібрати цю бібліотеку - значення в програмі (яка використовує дану збірку) не зміниться.
       
       Ви можете змінити значення MaxEntriesInList в файлі /Library/SomeLibrary.cs та перебудувати збірку
       ("csc /t:library /out:SomeLibrary.dll SomeLibrary.dll" в Developer Command Prompt), після чого перенести
       .dll-файл в /bin/Debug. Після цього запустіть .exe-файл та буде видно, що значення MaxEntriesInList в програмі не оновилось.*/
    class Program
    {
        static void Main(string[] args)
        {
            // Проблема, яка описана вище, виникає тут при викоританні SomeLibraryType.MaxEntriesInList:
            Console.WriteLine("Max entries supported in list: " + SomeLibraryType.MaxEntriesInList);
            Console.ReadKey();
        }
    }
}
