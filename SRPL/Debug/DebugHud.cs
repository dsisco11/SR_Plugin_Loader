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
        }
    }
}
