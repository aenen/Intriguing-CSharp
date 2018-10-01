using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace __bin
{
    class Program
    {
        static void Main(string[] args)
        {
            Home h = new Home();
            h.DoSomething();
        }
    }
    class Base
    {
        public virtual void DoSomething()
        {
            Console.WriteLine("Base.DoSomething");
            DoSomethingWithSomething();
        }

        public virtual void DoSomethingWithSomething()
        {
            Console.WriteLine("Base.DoSomethingWithSomething");
        }
    }
    class Home:Base
    {
        public override void DoSomethingWithSomething()
        {
            Console.WriteLine("Home");
        }
    }
}
