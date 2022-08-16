using System;

namespace AL_Training003
{
    //Extend the string class, add a new function called ToCurrency().
    //if string contains numbers, the function will prepend $ sign to and return a new string
    //or else it will throw an exception.
    public static class StringExtensions
    {
        const string dollar = "$";
        public static string ToCurrency(this string value)
        {

            if (value == null)
            {
                throw new NullReferenceException(value);
            }
            else if (value.Length == 0)
            {
                return string.Concat(dollar, "0");
            }
            else
                return string.Concat(dollar, double.Parse(value));

        }
    }
}
