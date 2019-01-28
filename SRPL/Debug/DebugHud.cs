using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SRPL.Debug
{
    public class DebugHud
    {
        private static GameObject _hudRoot;
        private static DebugHudRenderer _hudRenderer;
        private static List<string> lines = new List<string>();

        public static void Init()
        {
            if (_hudRoot == null)
            {
                _hudRoot = new GameObject();
                UnityEngine.Object.DontDestroyOnLoad(_hudRoot);
            }
            if (_hudRenderer == null)
            {
                _hudRenderer = _hudRoot.AddComponent<DebugHudRenderer>();
                UnityEngine.Object.DontDestroyOnLoad(_hudRenderer);
            }

            Logging.Logger.onLog += Logger_onLog;
        }

        private static void Logger_onLog(Logging.LogLevel level, string module, string msg)
        {
            if (_hudRenderer == null)
            {
                lines.Add(msg);
            }
            else
            {
                if (lines.Count > 0)
                {
                    foreach (string s in lines)
                    {
                        _hudRenderer.AddLine(s);
                    }
                    lines.Clear();
                }
                _hudRenderer.AddLine(msg);
            }
        }
    }
}
