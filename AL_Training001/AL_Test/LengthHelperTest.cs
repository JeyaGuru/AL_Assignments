using System;
using Xunit;
using AL_Training001;

namespace AL_Test
{
    public class LengthHelperTest
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(0, new LengthHelper().findLength(""));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(3, new LengthHelper().findLength("123"));

        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(12, new LengthHelper().findLength("123456789010"));
        }

        [Fact]
        public void Test4()
        {
            Assert.Throws<NullReferenceException>(() => new LengthHelper().findLength(null));
        }
    }
}
