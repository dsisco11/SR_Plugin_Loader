using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

using SRPL.Debug;

namespace SRPL
{
    public static class Loader
    {
        public const string VERSION = "v1.0.0";
        public const string TITLE = "SRPL " + VERSION;

        /// <summary>
        /// Initializes the plugin loader. Called from SECTR_AudioSystem.OnEnable
        /// </summary>
        public static void Init()
        {
            // TODO: Initialize loader
            DebugHud.Init();
        }
    }
}
