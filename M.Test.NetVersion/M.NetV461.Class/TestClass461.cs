using M.NetV4.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M.NetV461.Class
{
    public class TestClass461:ISecretCode
    {

        public string GetSomething(string passedValue)
        {

            Console.WriteLine("M.NetV461.Class.TestClass461: " +this.GetType().Assembly.ImageRuntimeVersion);

            return $"Hello from v46 \n The message recieved was   {passedValue}";

        }

    }
}
