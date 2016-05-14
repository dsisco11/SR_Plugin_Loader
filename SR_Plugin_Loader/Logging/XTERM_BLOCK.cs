using System;
using System.Collections.Generic;
using System.Text;

namespace Logging
{
    internal struct XTERM_BLOCK
    {
        public List<XTERM_COMMAND> Codes;
        public string TEXT;

        public XTERM_BLOCK(string str, List<XTERM_COMMAND> commands)
        {
            TEXT = str;
            Codes = commands;
        }
    }

}
