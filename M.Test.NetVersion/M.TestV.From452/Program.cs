using M.NetV4.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M.TestV.From452
{
    class Program
    {
        static void Main(string[] args)
        {
            TestSecret t = new TestSecret();
            Console.WriteLine(t.GetSomething("Hello from 452"));
            Console.ReadLine();
        }
    }
}
