using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

using SRPL.Util;
using SRPL.Graphics;

namespace SRPL.Debug
{
    public class DebugHudRenderer : MonoBehaviour
    {
        private const float ALERT_SIZE = 32f;
        private const float ALERT_ICON_OFFSET = 10f;
        private const float PANEL_WIDTH = 500f;

        private List<string> lines = new List<string>();
        private Dictionary<string, int> stacks = new Dictionary<string, int>();

        private Rect screenArea, consoleArea, consoleInnerArea, consoleInnerTextArea, fadeArea, watermarkTextArea, watermarkArea, playerPosArea;
        private Rect alertPos = new Rect(ALERT_ICON_OFFSET, ALERT_ICON_OFFSET, ALERT_SIZE, ALERT_SIZE);
        private Rect alertTxtPos = new Rect(0f, 0f, 0f, 0f);
        private Rect alertSubTxtPos = new Rect(0f, 0f, 0f, 0f);

        private Vector2 alertTxtSz = new Vector2();
        private Vector2 alertSubTxtSz = new Vector2();
        private Vector2 consoleScroll = Vector2.zero;

        private GUIContent consoleLines = new GUIContent();
        private GUIContent alertContent = new GUIContent();
        private GUIContent alertSubContent = new GUIContent();
        private GUIContent watermarkContent = new GUIContent();
        private GUIContent watermarkTextContent = new GUIContent();
        private GUIContent playerPosText = new GUIContent();

        private static Texture2D bgFade = null;
        private static GUIStyle blackout = new GUIStyle();
        private static GUIStyle textStyle = new GUIStyle();
        private static GUIStyle subtextStyle = new GUIStyle();
        private static GUIStyle consoleTextStyle = new GUIStyle();
        private static GUIStyle watermarkStyle = new GUIStyle();

        private static GUISkin skin = null;

        private bool needsLayout = true;
        private bool dirtyStyles = true;
        private bool open = false;
        private int newCount = 0;
        private int id = 0;

        private float scrollbarWidth = 6f;

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
            bgFade = TextureHelper.CreateGradientTexture(400, GRADIENT_DIR.LEFT_RIGHT, new Color(0, 0, 0, 1), new Color(0, 0, 0, 0), true, 1.5f);
            TextureHelper.SetBGColor(blackout.normal, 0.1f, 0.1f, 0.1f, 0.7f);

            textStyle.normal.textColor = Color.white;
            textStyle.active.textColor = Color.white;
            textStyle.hover.textColor = Color.white;
            textStyle.fontSize = 16;
            textStyle.fontStyle = FontStyle.Bold;

            subtextStyle = new GUIStyle(textStyle);
            subtextStyle.fontSize = 12;
            subtextStyle.fontStyle = FontStyle.Normal;
            subtextStyle.richText = true;

            skin = ScriptableObject.CreateInstance<GUISkin>();
            skin.name = "DebugHudRenderer";

            skin.verticalScrollbar = new GUIStyle();
            skin.verticalScrollbar.fixedWidth = scrollbarWidth;
            TextureHelper.SetBGColor(skin.verticalScrollbar.normal, new Color32(16, 16, 16, 200));

            skin.verticalScrollbarThumb = new GUIStyle();
            skin.verticalScrollbarThumb.fixedWidth = scrollbarWidth;
            TextureHelper.SetBGColor(skin.verticalScrollbarThumb.normal, new Color32(80, 80, 80, 255));

            skin.horizontalScrollbar = null;
            skin.horizontalScrollbarLeftButton = null;
            skin.horizontalScrollbarRightButton = null;
            skin.horizontalScrollbarThumb = null;

            consoleTextStyle.normal.textColor = Color.white;
            consoleTextStyle.active.textColor = Color.white;
            consoleTextStyle.hover.textColor = Color.white;
            consoleTextStyle.fontSize = 14;
            consoleTextStyle.fontStyle = FontStyle.Normal;
            consoleTextStyle.richText = true;

            watermarkStyle = new GUIStyle(GUI.skin.GetStyle("label"));
            watermarkStyle.normal.textColor = new Color(1, 1, 1, 0.6f);
            watermarkStyle.fontSize = 16;
            watermarkStyle.fontStyle = FontStyle.Bold;
            watermarkStyle.padding = new RectOffset(3, 3, 3, 3);
            watermarkStyle.normal.background = null;

            watermarkTextContent.text = Loader.TITLE;
            watermarkContent = new GUIContent(TextureHelper.ICON_LOGO);
        }
        
        private void Update()
        {
            if (Input.GetKeyUp(OPEN_KEY) || (Input.GetKeyUp(KeyCode.Escape) && open))
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

        /// <summary>
        /// Uses all of the system mouse movement, hover, and input events so we can prevent all controls under this one from getting them.
        /// Because this is a fullscreen overlay.
        /// </summary>
        private bool handleEvents()
        {
            id = GUIUtility.GetControlID(id, FocusType.Keyboard, screenArea);
            var evt = Event.current.GetTypeForControl(id);
            switch (evt)
            {
                case EventType.Layout:
                    doLayout();
                    return false;
                case EventType.Ignore:
                case EventType.Used:
                    return false;
                case EventType.Repaint:
                    break;
                case EventType.MouseDrag:
                case EventType.MouseMove:
                case EventType.MouseDown:
                case EventType.MouseUp:
                    //Event.current.Use();
                    return false;
                default:
                    //Event.current.Use();
                    break;
            }

            return true;
        }

        private void doLayout()
        {
            needsLayout = false;
            float offsetH = 50f;
            float offsetBottom = 20f;
            float cPad = 5f;

            screenArea = new Rect(0f, 0f, Screen.width, Screen.height);
            fadeArea = screenArea;
            consoleArea = new Rect(cPad, offsetH, Screen.width - (cPad * 2), Screen.height - offsetH - offsetBottom);

            float consoleWidth = consoleArea.width + scrollbarWidth;
            float textHeight = consoleTextStyle.CalcHeight(consoleLines, consoleWidth);
            consoleInnerArea = new Rect(0f, 0f, consoleWidth, textHeight);
            consoleInnerTextArea = new Rect(consoleInnerArea.x + scrollbarWidth * 3, consoleInnerArea.y, consoleInnerArea.width - scrollbarWidth - 2, textHeight);

            alertPos = new Rect(ALERT_ICON_OFFSET, ALERT_ICON_OFFSET, ALERT_SIZE, ALERT_SIZE);

            alertTxtSz = textStyle.CalcSize(alertContent);
            alertTxtPos = new Rect(alertPos.x + alertPos.width + 3, alertPos.y + (alertPos.height * 0.5f) - (alertTxtSz.y * 0.5f), alertTxtSz.x, alertTxtSz.y);
            alertSubTxtSz = subtextStyle.CalcSize(alertSubContent);
            alertSubTxtPos = new Rect(alertTxtPos.xMin, alertTxtPos.yMax + 3, alertSubTxtSz.x, alertSubTxtSz.y);

            float hw = Screen.width / 2f;
            Vector2 txtSz = watermarkStyle.CalcSize(watermarkTextContent);

            float logoSize = 36f;

            float padding = 2f;
            float X = padding;
            float Y = Screen.height - logoSize + padding;

            watermarkArea = new Rect(X, Y, logoSize, logoSize);
            watermarkTextArea = new Rect(watermarkArea.xMax + 1, watermarkArea.yMax - txtSz.y - 2, 300, 25);
        }

        private void OnGUI()
        {
            if (dirtyStyles) initStyles();

            if (Event.current.GetTypeForControl(id) == EventType.Layout || needsLayout)
            {
                doLayout();
                return;
            }

            if (!open)
            {
                if (newCount > 0)
                {
                    Color clr = new Color(1, 1, 1, 0.85f);

                    textStyle.Draw(alertTxtPos, alertContent, id);
                    subtextStyle.Draw(alertSubTxtPos, alertSubContent, id);

                    Color prevClr = GUI.color;
                    GUI.color = clr;

                    //GUI.DrawTexture(alertPos, TextureHelper.ICON_ALERT, ScaleMode.ScaleToFit);
                    GUI.color = prevClr;
                }
            }
            else
            {
                int prevDepth = GUI.depth;
                GUI.depth = 100;
                if (!handleEvents()) return;

                blackout.Draw(screenArea, GUIContent.none, id);

                GUISkin prevSkin = GUI.skin;
                GUI.skin = skin;

                consoleScroll = GUI.BeginScrollView(consoleArea, consoleScroll, consoleInnerArea, false, false);
                consoleTextStyle.Draw(consoleInnerTextArea, consoleLines, id);

                GUI.EndScrollView(true);

                GUI.depth = prevDepth;
                GUI.skin = prevSkin;

                consoleTextStyle.Draw(playerPosArea, playerPosText, false, false, false, false);
            }
        }
    }
}