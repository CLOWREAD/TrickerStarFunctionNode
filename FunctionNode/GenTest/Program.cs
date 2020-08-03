using System;
using System.Collections.Generic;

namespace GenTest
{
    class Program
    {
        static void Main(string[] args)
        {
          
            Test t = new Test();

            var a = DateTime.Now;

            for (int i= 0;i<128000;i++)
            {
                Console.Write("HELLO");
                //t.API_MAIN(null);
            }
            var b = DateTime.Now;
            var a_utc = a.ToFileTimeUtc();
            var b_utc = b.ToFileTimeUtc();
            Console.WriteLine("{0}",b_utc-a_utc);
        }


    }
    class Test
    {
        public dynamic NODEF89B9A17AB067A623D3F02B19BDE1A8B(dynamic param)
        {
            String V = "HELLO";
    System.Collections.Generic.Dictionary<String, Object> GLOBAL_OUTPUT = param.GLOBAL_OUTPUT;



            dynamic d = new System.Dynamic.ExpandoObject();
            d.FUNCTIONNAMETOINVOKE = "NODEA73FA1AFE7B6DB2724FAB8904D670723";
            d.V = V.ToString();

            return d;
        }
        public void API_MAIN(dynamic param)
        {
            System.Collections.Generic.Dictionary<String, Object> result = new Dictionary<string, object>();
            var this_type = this.GetType();
            String F_Name_invoking = "NODEF89B9A17AB067A623D3F02B19BDE1A8B";
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
                if (method == null) return;
            }
        }

        public dynamic NODEA73FA1AFE7B6DB2724FAB8904D670723(dynamic param)
        {

            System.Collections.Generic.Dictionary<String, Object> GLOBAL_OUTPUT = param.GLOBAL_OUTPUT;

            object V = null; if (GLOBAL_OUTPUT.ContainsKey("NODEF89B9A17AB067A623D3F02B19BDE1A8B")) { try { V = ((dynamic)GLOBAL_OUTPUT["NODEF89B9A17AB067A623D3F02B19BDE1A8B"]).V; } catch (Exception e) { } };

            Console.Write(V);

            dynamic d = new System.Dynamic.ExpandoObject();
            d.FUNCTIONNAMETOINVOKE = "null";

            return d;
        }





    }

}
