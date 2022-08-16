using System;

namespace AL_Training003
{   
    class Program
    {
        static void Main(string[] args)
        {
            var a = "1000.00000000";
            a = a.ToCurrency();
            Console.WriteLine(a);
             a = "-1000.00000000";
            a = a.ToCurrency();
            Console.WriteLine(a);

            a = "100";
            a = a.ToCurrency();
            Console.WriteLine(a);
        }
    }
}
