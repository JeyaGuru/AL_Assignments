using AL_Training004;
using System;
using System.IO;
using Xunit;

namespace AL_Test
{
    public class ConsoleReaderUnitTest
    {
        [Fact]
        public void ConsoleReaderTestCases()
        {
            //Case 1:
            var input = "TestWordFormat";
            Assert.Equal($"OnWord Executed : {input}", TestOnWord(input));

            //Case 2:
            input = "abc123!@#$%$@!";
            Assert.Equal($"OnJunk Executed : {input}", TestOnJunk(input));

            //Case 3:
            input = "12343234";
            Assert.Equal($"OnNumber Executed : {input}", TestOnNumber(input));

            //Case 4:
            input = "Test001";
            Assert.Equal($"OnWord Executed : {input}", TestCharAndNumber(input));

            //Case 5:
            input = "\n";
            Assert.Empty(TestEnterOrNewLineCase(input));

            //Case 6:
            input = " ";
            Assert.Empty(TestWhiteSpaceCase(input));
        }


        public string TestOnWord(string input)
        {
            ConsoleReader consoleReader = new ConsoleReader();

            var stringReader = new StringReader(input);
            Console.SetIn(stringReader);

            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            consoleReader.Run();

            return stringWriter.ToString().Replace("\r\n", "");
        }

        public string TestOnJunk(string input)
        {
            ConsoleReader consoleReader = new ConsoleReader();

            var stringReader = new StringReader(input);
            Console.SetIn(stringReader);

            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            consoleReader.Run();

            return stringWriter.ToString().Replace("\r\n", "");
        }

        public string TestOnNumber(string input)
        {
            ConsoleReader consoleReader = new ConsoleReader();          

            var stringReader = new StringReader(input);
            Console.SetIn(stringReader);

            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            consoleReader.Run();

            return stringWriter.ToString().Replace("\r\n", "");
        }

        public string TestCharAndNumber(string input)
        {
            ConsoleReader consoleReader = new ConsoleReader();

            var stringReader = new StringReader(input);
            Console.SetIn(stringReader);

            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            consoleReader.Run();

            return stringWriter.ToString().Replace("\r\n", "");
        }



        public string TestEnterOrNewLineCase(string input)
        {
            ConsoleReader consoleReader = new ConsoleReader();

            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            var stringReader = new StringReader(input);
            Console.SetIn(stringReader);

            consoleReader.Run();

            return stringWriter.ToString().Replace("\r\n", "");
        }

        public string TestWhiteSpaceCase(string input)
        {
            ConsoleReader consoleReader = new ConsoleReader();

            var stringReader = new StringReader(input);
            Console.SetIn(stringReader);

            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            consoleReader.Run();

            return stringWriter.ToString().Replace("\r\n", "");
        }
    }
}
