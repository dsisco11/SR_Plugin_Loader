using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SR_PluginLoader
{
    public static class DebugHud
    {
        private static GameObject hud_root = null;
        private static DebugHUD_Renderer hud = null;

        private static List<string> lines = new List<string>();
        private static Dictionary<string, int> stacks = new Dictionary<string, int>();
        private static FileStream log_file = null;


        public static void Init()
        {
            if (DebugHud.hud_root == null)
            {
                DebugHud.hud_root = new GameObject();
                UnityEngine.Object.DontDestroyOnLoad(DebugHud.hud_root);
            }

            if (DebugHud.hud == null)
            {
                DebugHud.hud = DebugHud.hud_root.AddComponent<DebugHUD_Renderer>();
                UnityEngine.Object.DontDestroyOnLoad(DebugHud.hud);
            }
        }

        public static void open_log_stream()
        {
            if (DebugHud.log_file != null) DebugHud.log_file.Close();

            string logPath = String.Format("{0}/Plugins.log", UnityEngine.Application.dataPath);
            DebugHud.log_file = new FileStream(logPath, FileMode.Create);
        }

        private static void write_log(string str)
        {
            if (DebugHud.log_file == null) DebugHud.open_log_stream();
            
            if (!str.EndsWith("\n")) str += "\n";
            UnityEngine.Debug.Log(str);

            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);

            DebugHud.log_file.Write(bytes, 0, bytes.Length);
        }

        private static void write_log(string format, params object[] args)
        {
            string str = String.Format(format, args);
            write_log(str);
        }

        public static string Format_Log(Exception ex, int stack_offset=0)
        {
            string str = String.Format("{0}\n{1}", ex.Message, ex.StackTrace);

            if(ex.InnerException != null)
            {
                str = String.Format("{0}\n{1}", str, ex.InnerException.Message);
            }

            return DebugHud.Format_Log(str, 1);//reformat our exception string with an additional stack offset of 1 to make it skip past the function that called THIS format function.
        }

        public static string Format_Log(string format, int stack_offset=0, params object[] args)
        {
            StackFrame frame = new StackFrame(2+stack_offset, true);
            string meth = frame.GetMethod().ReflectedType.Name;
            string funct = frame.GetMethod().Name;
            int line = frame.GetFileLineNumber();

            string str = String.Format(format, args);
            str = String.Format("{0}  [Function: {1}::{2} @ Line: {3}]", str, meth, funct, line);

            return str;
        }

        public static void Add_Line(string str)
        {
            DebugHud.write_log(str);

            if (DebugHud.hud == null)
            {
                DebugHud.lines.Add(str);
            }
            else
            {
                if (DebugHud.lines.Count > 0)
                {
                    foreach (string s in DebugHud.lines)
                    {
                        DebugHud.hud.Add_Line(s);
                    }
                    DebugHud.lines.Clear();
                }
                DebugHud.hud.Add_Line(str);
            }
        }

        public static void Log(string format, params object[] args)
        {
            string str = String.Format(format, args);
            DebugHud.Add_Line(str);
        }

        public static void Log(string str)
        {
            DebugHud.Add_Line(str);
        }

        public static void Log(Exception ex)
        {
            string str = DebugHud.Format_Log(ex);
            DebugHud.Add_Line(str);
        }



        public static void LogSilent(string format, params object[] args)
        {
            string str = DebugHud.Format_Log(format, 0, args);
            DebugHud.write_log(str);
        }

        public static void LogSilent(string str)
        {
            DebugHud.write_log(str);
        }

        public static void LogSilent(Exception ex)
        {
            string str = DebugHud.Format_Log(ex, 0);
            DebugHud.write_log(str);
        }

        public static void tally(string str)
        {
            if (DebugHud.hud != null) DebugHud.hud.Add_Tally(str);

            if (DebugHud.hud != null)
            {
                if (DebugHud.stacks.Count > 0)
                {
                    foreach (KeyValuePair<string, int> kv in DebugHud.stacks)
                    {
                        DebugHud.hud.Add_Tally(kv.Key, kv.Value);
                    }
                    DebugHud.stacks.Clear();
                }
                DebugHud.hud.Add_Tally(str);
            }
            else
            {
                int tmp = 0;
                if(!DebugHud.stacks.TryGetValue(str, out tmp)) DebugHud.stacks[str] = 0;

                DebugHud.stacks[str]++;
            }
        }

        public static void Reset()
        {
            if (DebugHud.hud != null) DebugHud.hud.Clear();
        }

    }
}
