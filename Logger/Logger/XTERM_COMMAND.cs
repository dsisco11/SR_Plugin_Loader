using System;
using System.Collections.Generic;
using System.Text;

namespace Logging
{
    /// <summary>
    /// Used internally by the XTERM utility class.
    /// DO NOT REFERENCE!
    /// </summary>
    internal enum XTERM_COMMAND
    {
        SET_FG_BLACK = 30,
        SET_FG_RED,
        SET_FG_GREEN,
        SET_FG_YELLOW,
        SET_FG_BLUE,
        SET_FG_MAGENTA,
        SET_FG_CYAN,
        SET_FG_WHITE,
        SET_FG_RGB = 38,
        SET_FG_DEFAULT = 39,

        SET_BG_BLACK = 40,
        SET_BG_RED,
        SET_BG_GREEN,
        SET_BG_YELLOW,
        SET_BG_BLUE,
        SET_BG_MAGENTA,
        SET_BG_CYAN,
        SET_BG_WHITE,
        SET_BG_RGB = 48,
        SET_BG_DEFAULT = 49,


        SET_FG_BLACK_BRIGHT = 90,
        SET_FG_RED_BRIGHT,
        SET_FG_GREEN_BRIGHT,
        SET_FG_YELLOW_BRIGHT,
        SET_FG_BLUE_BRIGHT,
        SET_FG_MAGENTA_BRIGHT,
        SET_FG_CYAN_BRIGHT,
        SET_FG_WHITE_BRIGHT,

        SET_BG_BLACK_BRIGHT = 100,
        SET_BG_RED_BRIGHT,
        SET_BG_GREEN_BRIGHT,
        SET_BG_YELLOW_BRIGHT,
        SET_BG_BLUE_BRIGHT,
        SET_BG_MAGENTA_BRIGHT,
        SET_BG_CYAN_BRIGHT,
        SET_BG_WHITE_BRIGHT,
    }
}
