using System;
using Xunit;
using AL_Training001;

namespace AL_Test
{
    //Create a unit test project using Xunit
    //write test cases to find length of string for "", "123", "123456789010", NULL
    public class LengthHelperTest
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(0, new LengthHelper().FindLength(""));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(3, new LengthHelper().FindLength("123"));

        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(12, new LengthHelper().FindLength("123456789010"));
        }

        [Fact]
        public void Test4()
        {
            Assert.Throws<NullReferenceException>(() => new LengthHelper().FindLength(null));
        }
    }
}
