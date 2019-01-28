using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace SRPL.Debug
{
    public class DebugHudRenderer : MonoBehaviour
    {
        private const float PANEL_WIDTH = 500f;
        private List<string> lines = new List<string>();
        private Dictionary<string, int> stacks = new Dictionary<string, int>();

        private GUIContent consoleLines = new GUIContent();
        private GUIContent alertContent = new GUIContent();
        private GUIContent alertSubContent = new GUIContent();

        private bool needsLayout = true;
        private bool dirtyStyles = true;
        private bool open = false;
        private int newCount = 0;
        private int id = 0;

        private const KeyCode OPEN_KEY = KeyCode.Tab;

        public void Awake()
        {
            Clear();
        }

        public void Clear()
        {
            lines.Clear();
            stacks.Clear();
        }

        public void AddLine(string str)
        {
            newCount++;
            lines.Add(str);

            consoleLines.text = string.Join("\n", lines.ToArray());
            string msg = string.Format("{0} new log{1}", newCount, newCount > 1 ? "s" : "");
            alertContent.text = msg;
            alertSubContent.text = string.Format("<i>Press <b>{0}</b> to open the plugins console.</i>", OPEN_KEY);
            needsLayout = true;
        }

        private void initStyles()
        {
            dirtyStyles = false;
        }
        
        private void Update()
        {
            if (Input.GetKeyUp(OPEN_KEY) || (Input.GetKeyUp(KeyCode.Escape) && open)))
            {
                open = (!open);
                if (open)
                {
                    newCount = 0;
                }
                onVisibility_Change(open);
            }
        }

        private void onVisibility_Change(bool is_vis)
        {
            if (is_vis == true) GameTime.Pause();
            else GameTime.Unpause();
        }

        private void OnGUI()
        {
            if (dirtyStyles) initStyles();
        }
    }
}