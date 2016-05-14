using System;
using System.IO;
using System.Linq;

namespace Logging
{
    /// <summary>
    /// Abstracts the different log level functions and outputs loglines with a tag and moduleName prepended to them.
    /// Also supports linking to other <see cref="LogModule"/> instances so their logging tag will be prepended before it's own lines in the case that classes need to be more specific about where the lines are coming from.
    /// </summary>
    public class LogModule
    {
        #region Variables
        /// <summary>
        /// If set, all logged messages will be routed through a seperate Logger instance.
        /// </summary>
        private LogModule _Proxy = null;
        /// <summary>
        /// Name of the module which is outputting the messages
        /// </summary>
        protected string _ModuleName { get { return (_Proxy != null ? _Proxy._ModuleName + " " : "") + (_module_name_str != null ? _module_name_str : (null==_module_name_funct ? "" : String.Concat("[", _module_name_funct(), "]"))); } }
        /// <summary>
        /// 
        /// </summary>
        protected Func<string> _module_name_funct = null;
        /// <summary>
        /// If non-null this string will be used for the module name.
        /// </summary>
        private string _module_name_str = null;

        #endregion

        #region Constructors

        public LogModule(string tag, LogModule prxy = null)
        {
            _Proxy = prxy;
            _module_name_str = tag;
        }

        /// <summary>
        /// Creates a new <see cref="LogModule"/> instance whose moduleName is determined by a function which is called whenever the moduleName is needed.
        /// This is useful for cases where the moduleName might be colored and change over time.
        /// </summary>
        /// <param name="nameFunc"></param>
        public LogModule(Func<string> nameFunc) { _module_name_funct = nameFunc; }
        #endregion

        #region Logging functions
        // This outputs a log entry of the level info.
        public void Info(string format, params object[] args) { Logger._OutputLine(LogLevel.Info, _ModuleName, format, args); }

        // This outputs a log entry of the level info.
        public void Info(params object[] args) { Logger._OutputLine(LogLevel.Info, _ModuleName, args); }

        // This outputs a log entry of the level debug.
        public void Debug(string format, params object[] args) { Logger._OutputLine(LogLevel.Debug, _ModuleName, format, args); }

        // This outputs a log entry of the level debug.
        public void Debug(params object[] args) { Logger._OutputLine(LogLevel.Debug, _ModuleName, args); }

        // This outputs a log entry of the level success.
        public void Success(string format, params object[] args) { Logger._OutputLine(LogLevel.Success, _ModuleName, format, args); }

        // This outputs a log entry of the level success.
        public void Success(params object[] args) { Logger._OutputLine(LogLevel.Success, _ModuleName, args); }

        // This outputs a log entry of the level warn.
        public void Warn(string format, params object[] args) { Logger._OutputLine(LogLevel.Warn, _ModuleName, format, args); }

        // This outputs a log entry of the level warn.
        public void Warn(params object[] args) { Logger._OutputLine(LogLevel.Warn, _ModuleName, args); }

        // This outputs a log entry of the level error.
        public void Error(string format, params object[] args) { Logger._OutputLine(LogLevel.Error, _ModuleName, format, args); }

        // This outputs a log entry of the level error.
        public void Error(params object[] args) { Logger._OutputLine(LogLevel.Error, _ModuleName, args); }

        // This outputs a log entry of the level error.
        public void Error(Exception ex) { Logger.Error(_ModuleName, ex); }

        // This outputs a log entry of the level interface;
        // normally, this means that some sort of user interaction
        // is required.
        public void Interface(string format, params object[] args) { Logger._OutputLine(LogLevel.Interface, _ModuleName, format, args); }

        // This outputs a log entry of the level interface;
        // normally, this means that some sort of user interaction
        // is required.
        public void Interface(params object[] args) { Logger._OutputLine(LogLevel.Interface, _ModuleName, args); }

        #endregion
    }
}