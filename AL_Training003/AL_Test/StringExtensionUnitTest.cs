using AL_Training003;
using System;
using Xunit;

namespace AL_Test
{
    public class StringExtensionUnitTest
    {
        [Fact]
        public void TestNumber()
        {
            string input = "100";
            Assert.Equal("$100", input.ToCurrency());
        }

        [Fact]
        public void TestDecimalNumber()
        {
            string input = "100.52";
            Assert.Equal("$100.52", input.ToCurrency());
        }

        [Fact]
        public void TestNegativeNumber()
        {
            string input = "-100";
            Assert.Equal("$-100", input.ToCurrency());
        }

        [Fact]
        public void TestNegativeDecimalNumber()
        {
            string input = "-100.12";
            Assert.Equal("$-100.12", input.ToCurrency());
        }
        [Fact]
        public void TestFormatExceptionaCase()
        {
            string input = "100USD";
            Assert.Throws<FormatException>(() => input.ToCurrency());
        }

        [Fact]
        public void TestEmptyCase()
        {
            string input = "";
            Assert.Equal("$0", input.ToCurrency());
        }

        [Fact]
        public void TestNullReferenceExceptionCase()
        {
            string input = null;
            Assert.Throws<NullReferenceException>(() => input.ToCurrency());
        }
    }
}
