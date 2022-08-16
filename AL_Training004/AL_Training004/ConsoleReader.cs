using System;
using System.Linq;

namespace AL_Training004
{
    /** 
     delegates: inputParser: Write a class ConsoleReader that has a run function that reads input from console, it also takes onWord, onNumber and onJunk delegates as the parameter and calls onWord or onNumber or onJunk delegates based on the following algorithm
    The console reader continously reads the input from the console using ReadLine
    and if it encounters a word that is group of english characters, it calls onWord.
    If it encounters a group of numbers, it calls onNumber
    and if the input has junk(non alphabet or non numbers) characters it calls onJunk.You can assume user will enter one word at a time and if the word has junk characters it will always call
    onJunk, if the input has both numbers and english characters, it will always call onWord, and if the input has only numbers, it will call onNumber
    You have to write Unit test cases for ConsoleReader and it is implementation.
    **/
    public delegate void InputParser(string input);

    public class ConsoleReader : IConsoleReader
    {
        public void Run()
        {
            while (true)
            {
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    break;
                }
                else if (CheckStringType.IsNumber(input))
                {
                    ExecuteInputParser(input, OnNumber);
                }
                else if (CheckStringType.IsWord(input))
                {
                    ExecuteInputParser(input, OnWord);
                }
                else
                    ExecuteInputParser(input, OnJunk);
            }
        }

        void ExecuteInputParser(string input, InputParser inputParser)
        {
            inputParser(input);
        }

        public void OnNumber(string input)
        {
            Console.WriteLine($"OnNumber Executed : {input}");
        }

        public void OnWord(string input)
        {
            Console.WriteLine($"OnWord Executed : {input}");
        }

        public void OnJunk(string input)
        {
            Console.WriteLine($"OnJunk Executed : {input}");
        }
    }

    public static class CheckStringType
    {
        public static bool IsNumber(string input)
        {
            return input.Trim().All(char.IsDigit);
        }

        public static bool IsWord(string input)
        {
            return !IsNumber(input) && input.Trim().All(char.IsLetterOrDigit);
        }
    }
}
