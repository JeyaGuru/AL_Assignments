using System;
using System.Collections.Generic;
using System.Text;

namespace AL_Training002
{
    static class StaticConstructor
    {
        public static int value;

        static StaticConstructor()
        {
            value = 200;
        }

        public static int GetValue()
        {
            return value;
        }
    }
}
