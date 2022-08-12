using System;

namespace AL_Training002
{
    class Program
    {
        static void Main(string[] args)
        {
            //Study Case 1:
            //Static class allows only static methods.
            //Static class initilize at first loaded

            Console.WriteLine(StaticConstructor.value);
            Console.WriteLine(StaticConstructor.GetValue());

            //study Case 2:
            //Singleton class can be initialized lazily & allows us to access the single created instance.
            //singleton can implement interfaces

            //Ex2:          
            var counterObj = Counter.GetInstance();
            counterObj.Count = 1;
            counterObj.Increment(); //1
            counterObj.Increment(); //2
            counterObj.Decrement(); //1

            Console.WriteLine(counterObj.Count);
        }
    }
}
