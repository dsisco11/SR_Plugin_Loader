using System;
using System.Collections.Generic;
using System.Text;

namespace Logging
{
    /// <summary>
    /// Mimics a global <see cref="LogModule"/> instance
    /// </summary>
    public static class Log
    {
        private static LogModule log = new LogModule(null);
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

        public static void Error(Exception ex) { log.Error(ex); }

        // This outputs a log entry of the level interface;
        // normally, this means that some sort of user interaction
        // is required.
        public static void Interface(string format, params object[] args) { log.Interface(format, args); }

        // This outputs a log entry of the level interface;
        // normally, this means that some sort of user interaction
        // is required.
        public static void Interface(params object[] args) { log.Interface(args); }
        #endregion
    }

}
