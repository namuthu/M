using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace M.NetV4.Class
{
    public class TestSecret : ISecretCode
    {
        public string GetSomething(string passedValue)
        {
            try
            {
                Console.WriteLine("M.NetV4.Class.TestSecret: " + this.GetType().Assembly.ImageRuntimeVersion);
                
                var asm = Assembly.Load("M.NetV461.Class");

                Console.WriteLine( " Loaded Assembly Verison: " + asm.ImageRuntimeVersion);

                var typeInstance = asm.GetType("M.NetV461.Class.TestClass461", true, false);
                ISecretCode actual = Activator.CreateInstance(typeInstance) as ISecretCode;
                if (actual != null)
                    return actual.GetSomething(passedValue);
            }catch(Exception ex)
            {
                return ex.ToString();
            }

            return "ERROR";
        }
    }
}
