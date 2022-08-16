using System;
using System.Collections.Generic;
using System.Text;

namespace AL_Training004
{
    interface IConsoleReader
    {
        void Run();

        void OnWord(string input);

        void OnNumber(string input);

        void OnJunk(string input);
    }
}
