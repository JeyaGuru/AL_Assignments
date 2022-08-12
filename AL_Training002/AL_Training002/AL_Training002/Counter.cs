using System;
using System.Collections.Generic;
using System.Text;

namespace AL_Training002
{
    //Write a Singleton class called Counter.
    //It will have two functions Increment and Decrement that will increase and decrease the count respectively.
    //It has one property called count that returns the current counter.
    //The class will have one static getinstance method to return the instance of Counter.
    //Singleton typically means that only one instance of the class can be created.


    sealed class Counter
    {
        public int Count { get; set; }

        private static Counter _instance = null;

        private Counter()
        {

        }

        public void Increment()
        {
            Count++;
        }

        public void Decrement()
        {
            Count--;
        }

        public static Counter GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Counter();
            }
            return _instance;
        }
    }
}
