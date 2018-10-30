using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace __bin
{
    class Program
    {
        volatile Int32 m_volatileTest;

        static void Main(string[] args)
        {
            Home h = new Home();
            h.DoSomething();

            Int32 int32;
            UInt32 uint32;
            Decimal @decimal;
            System.Threading.Interlocked
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

    struct SomeValType
    {
        static Int32 s_field;

        static SomeValType()
        {

        }
    }
}
