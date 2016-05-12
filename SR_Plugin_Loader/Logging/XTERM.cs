using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

// https://en.wikipedia.org/wiki/ANSI_escape_code#Windows_and_DOS
namespace Logging
{

    /// <summary>
    /// Provides a helper class for adding XTERM/ANSI color codes to log messages.
    /// </summary>
    public static class XTERM
    {
        /// <summary>
        /// The char that begins an ANSI command.
        /// </summary>
        private const char ESC = '\x1b';
        /// <summary>
        /// The ANSI command to reset the Foreground & Background colors back to default
        /// </summary>
        private const string RESET = "\x1b[39;49m";
        private enum ANSI_COLOR
        {
            BLACK = 0,
            RED,
            GREEN,
            YELLOW,
            BLUE,
            MAGENTA,
            CYAN,
            WHITE
        }

        #region COMMAND BUILDING
        private static string Set_BG_Color(ANSI_COLOR color, string msg)
        {
            return String.Concat(ESC, '[', Convert.ToString(40 + (int)color), 'm', msg, RESET);
        }

        private static string Set_BG_Color_Bright(ANSI_COLOR color, string msg)
        {
            return String.Concat(ESC, '[', Convert.ToString(100 + (int)color), 'm', msg, RESET);
        }

        private static string Set_FG_Color(ANSI_COLOR color, string msg)
        {
            return String.Concat(ESC, '[', Convert.ToString(30 + (int)color), 'm', msg, RESET);
        }

        private static string Set_FG_Color_Bright(ANSI_COLOR color, string msg)
        {
            return String.Concat(ESC, '[', Convert.ToString(90 + (int)color), 'm', msg, RESET);
        }
        #endregion

        #region FOREGROUND COLORS
        public static string black(object obj=null) { return Set_FG_Color(ANSI_COLOR.BLACK, (null!=obj ? obj.ToString() : "")); }

        public static string red(object obj=null) { return Set_FG_Color(ANSI_COLOR.RED, (null != obj ? obj.ToString() : "")); }

        public static string green(object obj=null) { return Set_FG_Color(ANSI_COLOR.GREEN, (null != obj ? obj.ToString() : "")); }

        public static string yellow(object obj=null) { return Set_FG_Color(ANSI_COLOR.YELLOW, (null != obj ? obj.ToString() : "")); }

        public static string blue(object obj=null) { return Set_FG_Color(ANSI_COLOR.BLUE, (null != obj ? obj.ToString() : "")); }

        public static string magenta(object obj=null) { return Set_FG_Color(ANSI_COLOR.MAGENTA, (null != obj ? obj.ToString() : "")); }

        public static string cyan(object obj=null) { return Set_FG_Color(ANSI_COLOR.CYAN, (null != obj ? obj.ToString() : "")); }

        public static string white(object obj=null) { return Set_FG_Color(ANSI_COLOR.WHITE, (null != obj ? obj.ToString() : "")); }
        #endregion

        #region BRIGHT FOREGROUND COLORS
        public static string blackBright(object obj=null) { return Set_FG_Color_Bright(ANSI_COLOR.BLACK, (null != obj ? obj.ToString() : "")); }

        public static string redBright(object obj=null) { return Set_FG_Color_Bright(ANSI_COLOR.RED, (null != obj ? obj.ToString() : "")); }

        public static string greenBright(object obj=null) { return Set_FG_Color_Bright(ANSI_COLOR.GREEN, (null != obj ? obj.ToString() : "")); }

        public static string yellowBright(object obj=null) { return Set_FG_Color_Bright(ANSI_COLOR.YELLOW, (null != obj ? obj.ToString() : "")); }

        public static string blueBright(object obj=null) { return Set_FG_Color_Bright(ANSI_COLOR.BLUE, (null != obj ? obj.ToString() : "")); }

        public static string magentaBright(object obj=null) { return Set_FG_Color_Bright(ANSI_COLOR.MAGENTA, (null != obj ? obj.ToString() : "")); }

        public static string cyanBright(object obj=null) { return Set_FG_Color_Bright(ANSI_COLOR.CYAN, (null != obj ? obj.ToString() : "")); }

        public static string whiteBright(object obj=null) { return Set_FG_Color_Bright(ANSI_COLOR.WHITE, (null != obj ? obj.ToString() : "")); }
        #endregion

        #region UTILITY
        public static bool Is_CSI_Termination_Char(char c) { return (c >= 64 && c <= 126); }

        public static string Console_Color(ConsoleColor clr, string msg)
        {
            switch (clr)
            {
                case ConsoleColor.DarkYellow:
                    return yellow(msg);
                case ConsoleColor.Yellow:
                    return yellowBright(msg);
                case ConsoleColor.DarkRed:
                    return red(msg);
                case ConsoleColor.Red:
                    return redBright(msg);
                case ConsoleColor.DarkMagenta:
                    return magenta(msg);
                case ConsoleColor.Magenta:
                    return magentaBright(msg);
                case ConsoleColor.DarkGreen:
                    return green(msg);
                case ConsoleColor.Green:
                    return greenBright(msg);
                case ConsoleColor.DarkCyan:
                    return cyan(msg);
                case ConsoleColor.Cyan:
                    return cyanBright(msg);
                case ConsoleColor.DarkBlue:
                    return blue(msg);
                case ConsoleColor.Blue:
                    return blueBright(msg);
                case ConsoleColor.White:
                    return whiteBright(msg);
                case ConsoleColor.Gray:
                    return white(msg);
                case ConsoleColor.DarkGray:
                    return blackBright(msg);
                case ConsoleColor.Black:
                    return black(msg);
            }

            return msg;
        }

        private static XTERM_BLOCK Compile_Xterm_Command_Block(string str)
        {
            // Each escape sequesnts begins with ESC (0x1B) followed by a single char in the range <64-95>(most often '[') followed by a series of command chars each sperated by a semicolon ';', the entire sequence is ended with a character in the range <64-126>(most often 'm')
            Regex CSI = new Regex(@"^(\x1b\[(?<CMD>(\d+;)*\d*)[@-~]{1})?(?<TEXT>.*)?$");
            //Match match = CSI.Match(str);
            List<XTERM_COMMAND> codes = new List<XTERM_COMMAND>();
            // Ok first let's grab all the CSI command codes(if any)
            const char ESC = '\x1b';
            StringTokenizer tok = new StringTokenizer(str);
            if (tok.TryConsume(ESC))//it's only a CSI if it starts with the Control Sequence escape character.
            {
                if (tok.TryConsume('['))// If '[' is right after the control char then it indicates that this is a multi-command-char sequence, AKA THE ONLY ONE ANYBODY EVER USES! (Even though there ARE other kinds) So it's the only one we care to handle
                {
                    // what follows the first two chars SHOULD be a text list of numbers seperated by ';' chars. we need to gather said number list
                    while (tok.HasNext())
                    {
                        if (Char.IsDigit(tok.peek()))// Okay this char is part of a number
                        {
                            // Consume all consecutive digits
                            while (Char.IsDigit(tok.peekNext())) tok.Next();// move to the next char
                            //since the next char is not a digit we want to consume the list of digits we just verified and push them to the control code list now.
                            string num = tok.Consume();
                            int code = Convert.ToInt32(num);
                            codes.Add((XTERM_COMMAND)code);
                        }

                        // Okay we have encountered the first non-digit char.
                        // IF it is ';' then we can consume and continue, otherwise we abort the loop.
                        if (!tok.TryConsume(';')) break;
                    }
                    //now verify that the next char is the Control Sequence termination char
                    char c = tok.peek();
                    if (Is_CSI_Termination_Char(c)) tok.Consume();
                }
            }
            // Now that we will have parsed out the CSI block, consume what remains of the string as our text!
            string TEXT = tok.ConsumeAll();
            return new XTERM_BLOCK(TEXT, codes);
        }

        /// <summary>
        /// Strips all of the XTERM command sequences from a string and returns the cleaned string.
        /// </summary>
        public static string Strip(string format, params object[] args)
        {
            string str = format;
            if (args.Length > 0) str = String.Format(format, args);
            // Get the list of CSI's 
            List<string> CSIS = Tokenize_Control_Sequence_Initiators(str);

            // Now build an XTERM_COMMAND_BLOCK for each CSI and add it to our list
            string cleanStr = "";
            foreach (string block in CSIS)
            {
                XTERM_BLOCK xb = Compile_Xterm_Command_Block(block);
                cleanStr += xb.TEXT;
            }

            return cleanStr;
        }
        #endregion

        #region TERMINAL OUTPUT EMULATION
        public static void WriteLine(string format, params object[] args)
        {
            string str = format;
            if (args.Length > 0) str = String.Format(format, args);
            // Get the list of CSI's 
            List<string> CSIS = Tokenize_Control_Sequence_Initiators(str);
            // Now build an XTERM_COMMAND_BLOCK for each CSI and add it to our list
            foreach (string block in CSIS)
            {
                Execute_Xterm_Command_Block(block);
            }
            Console.WriteLine();
        }

        private static List<string> Tokenize_Control_Sequence_Initiators(string str)
        {
            List<string> list = new List<string>();
            int len = str.Length;
            int p = 0;// current processing idx
            int c = 0;// last consumed idx
            const char ESC = '\x1b';

            while (++p < len)
            {
                if (str[p] == ESC)
                {
                    int l = (p - c);
                    if (l <= 0) continue;
                    string tok = str.Substring(c, l);// extract our token
                    list.Add(tok);
                    c = p;
                }
            }

            string tk = str.Substring(c, (p - c));// extract our token
            list.Add(tk);

            return list;
        }

        private static void Execute_Xterm_Command_Block(string str)
        {
            XTERM_BLOCK xb = Compile_Xterm_Command_Block(str);
            foreach (XTERM_COMMAND cmd in xb.Codes)
            {
                Execute_Xterm_Command(cmd);
            }

            Console.Write(xb.TEXT);
        }

        private static void Execute_Xterm_Command(XTERM_COMMAND cmd)
        {
            switch (cmd)
            {
                case XTERM_COMMAND.SET_BG_DEFAULT:
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
                case XTERM_COMMAND.SET_FG_DEFAULT:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;


                case XTERM_COMMAND.SET_BG_BLACK:
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
                case XTERM_COMMAND.SET_BG_BLACK_BRIGHT:
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    break;

                case XTERM_COMMAND.SET_BG_RED:
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    break;
                case XTERM_COMMAND.SET_BG_RED_BRIGHT:
                    Console.BackgroundColor = ConsoleColor.Red;
                    break;

                case XTERM_COMMAND.SET_BG_GREEN:
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    break;
                case XTERM_COMMAND.SET_BG_GREEN_BRIGHT:
                    Console.BackgroundColor = ConsoleColor.Green;
                    break;

                case XTERM_COMMAND.SET_BG_YELLOW:
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    break;
                case XTERM_COMMAND.SET_BG_YELLOW_BRIGHT:
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    break;

                case XTERM_COMMAND.SET_BG_BLUE:
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    break;
                case XTERM_COMMAND.SET_BG_BLUE_BRIGHT:
                    Console.BackgroundColor = ConsoleColor.Blue;
                    break;

                case XTERM_COMMAND.SET_BG_MAGENTA:
                    Console.BackgroundColor = ConsoleColor.DarkMagenta;
                    break;
                case XTERM_COMMAND.SET_BG_MAGENTA_BRIGHT:
                    Console.BackgroundColor = ConsoleColor.Magenta;
                    break;

                case XTERM_COMMAND.SET_BG_CYAN:
                    Console.BackgroundColor = ConsoleColor.DarkCyan;
                    break;
                case XTERM_COMMAND.SET_BG_CYAN_BRIGHT:
                    Console.BackgroundColor = ConsoleColor.Cyan;
                    break;

                case XTERM_COMMAND.SET_BG_WHITE:
                    Console.BackgroundColor = ConsoleColor.Gray;
                    break;
                case XTERM_COMMAND.SET_BG_WHITE_BRIGHT:
                    Console.BackgroundColor = ConsoleColor.White;
                    break;


                // FOREGROUND COLORS
                case XTERM_COMMAND.SET_FG_BLACK:
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
                case XTERM_COMMAND.SET_FG_BLACK_BRIGHT:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;

                case XTERM_COMMAND.SET_FG_RED:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case XTERM_COMMAND.SET_FG_RED_BRIGHT:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;

                case XTERM_COMMAND.SET_FG_GREEN:
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    break;
                case XTERM_COMMAND.SET_FG_GREEN_BRIGHT:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;

                case XTERM_COMMAND.SET_FG_YELLOW:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case XTERM_COMMAND.SET_FG_YELLOW_BRIGHT:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;

                case XTERM_COMMAND.SET_FG_BLUE:
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    break;
                case XTERM_COMMAND.SET_FG_BLUE_BRIGHT:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;

                case XTERM_COMMAND.SET_FG_MAGENTA:
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    break;
                case XTERM_COMMAND.SET_FG_MAGENTA_BRIGHT:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;

                case XTERM_COMMAND.SET_FG_CYAN:
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    break;
                case XTERM_COMMAND.SET_FG_CYAN_BRIGHT:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;

                case XTERM_COMMAND.SET_FG_WHITE:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case XTERM_COMMAND.SET_FG_WHITE_BRIGHT:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
        }
        #endregion
    }

}
