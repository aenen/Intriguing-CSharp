using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            var myprop=new Home().MyProperty;

            Version version;
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
        private int myVar;

        public int MyProperty
        {
            get
            {
                Debug.WriteLine("викликаний метод get властивості MyProperty");
                return ++myVar;
            }
        }

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
