using M.NetV4.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M.Test.From461
{
    class Program
    {
        static void Main(string[] args)
        {
            ISecretCode t = new TestSecret();
            Console.WriteLine(t.GetSomething("Hello from 452"));
            Console.ReadLine();
        }
    }
}
