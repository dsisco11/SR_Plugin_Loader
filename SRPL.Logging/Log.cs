using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace SRPL.Logging
{    
    /// <summary>
    /// Provides static access to the SR_PluginLoader's logging system.
    /// </summary>
    public static class SLog
    {
        private static string originName {
            get
            {
                // This handly little doodad will figure our which plugin DLL the log message is coming from, it might be a little slow (not really) but I mean these are debug logs guys...
                int idx = 0;
                string class_name = null;
                StackFrame frame = null;

                // Search upwards through the callstack to find the first classname that isnt DebugHud. USUALLY this will only loop once and cost us no more time then the old method I used to use. 
                // But this one will catch those odd cases where this function gets called from a big hierarchy of DebugHud functions and STILL give us the plugin name we so want!
                while (class_name == null || String.Compare("DebugHud", class_name) == 0)
                {
                    frame = new StackFrame(++idx, false);// pre incrementing makes idx = 1 on the first loop
                    class_name = frame.GetMethod().DeclaringType.Name;
                }

                string plugin_name = frame.GetMethod().Module.ScopeName;
                return String.Concat("<b>", plugin_name, "</b>");
            }
        }

        private static LogModule log = new LogModule(originName);
        #region Logging Functions

        // This outputs a log entry of the level info.
        public static void Info(string format, params object[] args) { log.Info(format, args); }

        // This outputs a log entry of the level info.
        public static void Info(params object[] args) { log.Info(args); }

        // This outputs a log entry of the level debug.
        public static void Debug(string format, params object[] args) { log.Debug(format, args); }

        // This outputs a log entry of the level debug.
        public static void Debug(params object[] args) { log.Debug(args); }

        // This outputs a log entry of the level success.
        public static void Success(string format, params object[] args) { log.Success(format, args); }

        // This outputs a log entry of the level success.
        public static void Success(params object[] args) { log.Success(args); }

        // This outputs a log entry of the level warn.
        public static void Warn(string format, params object[] args) { log.Warn(format, args); }

        // This outputs a log entry of the level warn.
        public static void Warn(params object[] args) { log.Warn(args); }

        // This outputs a log entry of the level error.
        public static void Error(string format, params object[] args) { log.Error(format, args); }

        // This outputs a log entry of the level error.
        public static void Error(params object[] args) { log.Error(args); }

        public static void Error(Exception ex) { log.Error(Logger.Format_Exception(ex)); }

        // This outputs a log entry of the level interface;
        // normally, this means that some sort of user interaction
        // is required.
        public static void Interface(string format, params object[] args) { log.Interface(format, args); }

        // This outputs a log entry of the level interface;
        // normally, this means that some sort of user interaction
        // is required.
        public static void Interface(params object[] args) { log.Interface(args); }
        #endregion


        /// <summary>
        /// Checks for a condition; if the condition is <c>false</c>, outputs a specified message and displays a message box that shows the call stack.
        /// This method is equivalent to System.Diagnostics.Debug.Assert, however, it was modified to also write to the Logger output.
        /// </summary>
        /// <param name="condition">The conditional expression to evaluate. If the condition is <c>true</c>, the specified message is not sent and the message box is not displayed.</param>
        /// <param name="module">The category of the message.</param>
        /// <param name="message">The message to display if the assertion fails.</param>
        public static void Assert(bool condition, string message)
        {
            Logger.Assert(condition, originName, message);
        }
    }

}
