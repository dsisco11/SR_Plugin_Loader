using System;
using System.IO;
using System.Linq;

namespace Logging
{
    /// <summary>
    /// Manages the actual output part of the logging process
    /// </summary>
    public static class Logger
    {
        #region Variables
        /// <summary>
        /// Should module names be included in the output?
        /// </summary>
        public static bool showModuleNames = true;
        /// <summary>
        /// Should log lines be timestamped?
        /// </summary>
        public static bool showTimestamps = true;
        /// <summary>
        /// Should colors be stripped from log lines before they are output to file?
        /// </summary>
        public static bool stripXTERM = true;
        /// <summary>
        /// If <c>TRUE</c> then every log line output will show it's LogLevel at the start of the line.
        /// Meaning that debug lines start with "DEBUG: " and error lines start with "ERROR: " etc.
        /// </summary>
        public static bool Show_Log_Levels = true;

        private static StreamWriter _FileStream;
        public static LogLevel OutputLevel;
        public static LogLevel FileLogLevel;
        #endregion

        #region Begin/End logic

        /// <summary>
        /// Begins logging to a file, overwriting the contents of the file if it already exists.
        /// </summary>
        public static void Begin(string logFile, LogLevel consoleLogLevel = LogLevel.Info, LogLevel fileLogLevel = LogLevel.Debug)
        {
            string logPath = Path.GetFullPath(logFile);
            Directory.CreateDirectory(Path.GetDirectoryName(logPath));
            FileStream fs;
            try
            {
                fs = File.OpenWrite(logFile);
                fs.Seek(0, SeekOrigin.Begin);// Move to start of file.
                fs.SetLength(0);// Erase all contents.
                _FileStream = new StreamWriter(fs);
                _FileStream.AutoFlush = true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(Format_Exception(ex));
            }

            OutputLevel = consoleLogLevel;
            FileLogLevel = fileLogLevel;
        }

        /// <summary>
        /// Continues logging to a file if it already exists or creates it.
        /// </summary>
        public static void BeginAppend(string logFile, LogLevel consoleLogLevel = LogLevel.Info, LogLevel fileLogLevel = LogLevel.Debug)
        {
            string logPath = Path.GetFullPath(logFile);
            Directory.CreateDirectory(Path.GetDirectoryName(logPath));
            try
            {
                _FileStream = File.AppendText(logFile);
                _FileStream.AutoFlush = true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(Format_Exception(ex));
            }

            OutputLevel = consoleLogLevel;
            FileLogLevel = fileLogLevel;
        }
        
        /// <summary>
        /// Releases the logfile
        /// </summary>
        public static void End()
        {
            _FileStream.Dispose();//Just helps to ensure we release the file.
        }
        #endregion

        #region Internal logic
        // Outputs a line to both the log and the console, if
        // applicable.
        public static void _OutputLine(LogLevel level, string moduleName, params object[] args)
        {
            string lineStr = "";
            for (int i = 0; i < args.Length; i++)
            {
                lineStr += (args[i] + " ");
            }

            _OutputLine(level, moduleName, lineStr);
        }
        // Outputs a line to both the log and the console, if
        // applicable.
        public static void _OutputLine(LogLevel level, string moduleName, string format, params object[] args)
        {
            string logLevelStr = "";
            if(Show_Log_Levels) logLevelStr = String.Format("{0}: ", _ColorLogLevel(level, _LogLevel(level).ToUpper()));
            string lineStr = _ColorLog(level, (args != null && args.Any() ? String.Format(format, args) : format));

            string timeStr = "";
            if (showTimestamps) timeStr = String.Concat("[", DateTime.Now.ToString("HH:mm:ss tt"), "] ");

            string moduleStr = "";
            if (showModuleNames)
            {
                moduleStr = "(System) ";
                if (moduleName != null) moduleStr = (moduleName + " ");
            }

            string formattedString = String.Concat(timeStr, moduleStr, logLevelStr, lineStr);
            string fileFormattedString = (Logger.stripXTERM ? XTERM.Strip(formattedString) : formattedString);

            if (level >= FileLogLevel) _FileStream.WriteLine(fileFormattedString);
            if (level >= OutputLevel) XTERM.WriteLine(formattedString);
        }

        public static string _ColorLog(LogLevel level, string msg)
        {
            switch (level)
            {
                case LogLevel.Info:
                case LogLevel.Debug:
                    return XTERM.white(msg);
                case LogLevel.Success:
                    return XTERM.green(msg);
                case LogLevel.Warn:
                    return XTERM.yellow(msg);
                case LogLevel.Error:
                    return XTERM.red(msg);
                case LogLevel.Assert:
                    return XTERM.magenta(msg);
                case LogLevel.Interface:
                    return XTERM.cyan(msg);
                default:
                    return msg;
            }
        }

        public static string _ColorLogLevel(LogLevel level, string msg)
        {
            switch (level)
            {
                case LogLevel.Info:
                case LogLevel.Debug:
                    return XTERM.whiteBright(msg);
                case LogLevel.Success:
                    return XTERM.greenBright(msg);
                case LogLevel.Warn:
                    return XTERM.yellowBright(msg);
                case LogLevel.Error:
                    return XTERM.redBright(msg);
                case LogLevel.Assert:
                    return XTERM.magentaBright(msg);
                case LogLevel.Interface:
                    return XTERM.cyanBright(msg);
                default:
                    return msg;
            }
        }

        // Determine the string equivalent of the LogLevel.
        public static string _LogLevel(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Info:
                    return "info";
                case LogLevel.Debug:
                    return "debug";
                case LogLevel.Success:
                    return "success";
                case LogLevel.Warn:
                    return "warn";
                case LogLevel.Error:
                    return "error";
                case LogLevel.Assert:
                    return "assert";
                case LogLevel.Interface:
                    return "interface";
                default:
                    return "undef";
            }
        }

        /// <summary>
        /// Checks for a condition; if the condition is <c>false</c>, outputs a specified message and displays a message box that shows the call stack.
        /// This method is equivalent to System.Diagnostics.Debug.Assert, however, it was modified to also write to the Logger output.
        /// Borrowed from <c>SteamKit2</c>
        /// </summary>
        /// <param name="condition">The conditional expression to evaluate. If the condition is <c>true</c>, the specified message is not sent and the message box is not displayed.</param>
        /// <param name="category">The category of the message.</param>
        /// <param name="message">The message to display if the assertion fails.</param>
        public static void Assert(bool condition, string category, string message)
        {
            // make use of .NET's assert facility first
            System.Diagnostics.Debug.Assert(condition, string.Format("{0}: {1}", category, message));

            // then spew to our debuglog, so we can get info in release builds
            if (!condition)
                _OutputLine(LogLevel.Assert, category, "Assertion Failed! " + message);
        }

        public static string Format_Exception(Exception ex)
        {
            string MSG = ex.Message;
            string TRACE = ex.StackTrace;
            if (ex.InnerException != null)
            {
                MSG = String.Format("{0} {1}", MSG, ex.InnerException.Message);
                TRACE = String.Format("{0}\n\nOutter Stack Trace: {1}", ex.InnerException.StackTrace, ex.StackTrace);
            }

            return string.Format("{0}\nStack Trace: {1}", MSG, TRACE);
        }
        #endregion


        public static string Get_Todays_LogFile() { return Path.Combine("./logs/", String.Concat(DateTime.Now.ToString("yyyy_MM_dd"), ".log")); }

        #region Logging functions
        // This outputs a log entry of the level info.
        public static void Info(string _ModuleName, string format, params object[] args)
        {
            _OutputLine(LogLevel.Info, _ModuleName, format, args);
        }

        // This outputs a log entry of the level info.
        public static void Info(string _ModuleName, params object[] args)
        {
            _OutputLine(LogLevel.Info, _ModuleName, args);
        }

        // This outputs a log entry of the level debug.
        public static void Debug(string _ModuleName, string format, params object[] args)
        {
            _OutputLine(LogLevel.Debug, _ModuleName, format, args);
        }

        // This outputs a log entry of the level debug.
        public static void Debug(string _ModuleName, params object[] args)
        {
            _OutputLine(LogLevel.Debug, _ModuleName, args);
        }

        // This outputs a log entry of the level success.
        public static void Success(string _ModuleName, string format, params object[] args)
        {
            _OutputLine(LogLevel.Success, _ModuleName, format, args);
        }

        // This outputs a log entry of the level success.
        public static void Success(string _ModuleName, params object[] args)
        {
            _OutputLine(LogLevel.Success, _ModuleName, args);
        }

        // This outputs a log entry of the level warn.
        public static void Warn(string _ModuleName, string format, params object[] args)
        {
            _OutputLine(LogLevel.Warn, _ModuleName, format, args);
        }

        // This outputs a log entry of the level warn.
        public static void Warn(string _ModuleName, params object[] args)
        {
            _OutputLine(LogLevel.Warn, _ModuleName, args);
        }

        // This outputs a log entry of the level error.
        public static void Error(string _ModuleName, string format, params object[] args)
        {
            _OutputLine(LogLevel.Error, _ModuleName, format, args);
        }

        // This outputs a log entry of the level error.
        public static void Error(string _ModuleName, params object[] args)
        {
            _OutputLine(LogLevel.Error, _ModuleName, args);
        }

        public static void Error(string _ModuleName, Exception ex)
        {
            _OutputLine(LogLevel.Error, _ModuleName, Format_Exception(ex));
        }

        // This outputs a log entry of the level interface;
        // normally, this means that some sort of user interaction
        // is required.
        public static void Interface(string _ModuleName, string format, params object[] args)
        {
            _OutputLine(LogLevel.Interface, _ModuleName, format, args);
        }

        // This outputs a log entry of the level interface;
        // normally, this means that some sort of user interaction
        // is required.
        public static void Interface(string _ModuleName, params object[] args)
        {
            _OutputLine(LogLevel.Interface, _ModuleName, args);
        }

        #endregion
    }

}
