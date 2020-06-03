using System;
using System.Collections.Generic;

namespace GenTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Test t = new Test();
            t.API_MAIN(null);
        }


    }
    class Test
    {
        public void API_MAIN(dynamic param)
        {
            System.Collections.Generic.Dictionary<String, Object> result = new Dictionary<string, object>();
            var this_type = this.GetType();
            String F_Name_invoking = "NODE94A187D68CF7645C4064DFDCDDEE4683"; ;
            var method = this_type.GetMethod(F_Name_invoking);
            dynamic d = new System.Dynamic.ExpandoObject();

            while (F_Name_invoking != null && F_Name_invoking.Length != 0)
            {
                d = new System.Dynamic.ExpandoObject();
                d.GLOBAL_OUTPUT = result;
                dynamic invoke_res = method.Invoke(this, new object[] { d });
                result[F_Name_invoking] = invoke_res;
                if (invoke_res.FUNCTIONNAMETOINVOKE == null)
                {
                    break;
                }
                F_Name_invoking = invoke_res.FUNCTIONNAMETOINVOKE;
                method = this_type.GetMethod(F_Name_invoking);
            }
        }
        public dynamic NODE94A187D68CF7645C4064DFDCDDEE4683(dynamic param)
        {
            String V = "HELLO WORLD"
        System.Collections.Generic.Dictionary<String, Object> GLOBAL_OUTPUT = param.GLOBAL_OUTPUT;

            Console.Write(V);

            dynamic d = new System.Dynamic.ExpandoObject();
            d.FUNCTIONNAMETOINVOKE = "NODE0446E88790D1E5C92E7E798DE582970B"; ;
            d.V = V.ToString();

            return d;
        }
        public dynamic NODE0446E88790D1E5C92E7E798DE582970B(dynamic param)
        {

            System.Collections.Generic.Dictionary<String, Object> GLOBAL_OUTPUT = param.GLOBAL_OUTPUT;

            object V = null;

            dynamic d = new System.Dynamic.ExpandoObject();
            d.FUNCTIONNAMETOINVOKE = null;



            return d;
        }






    }

}
