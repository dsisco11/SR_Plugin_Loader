using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace SR_PluginLoader
{
    public delegate void onTextChangeDelegate(uiControl c, string str);
    public class uiTextbox : uiControl
    {
        public string text { get { return content.text; } set { content.text = value; } }
        public event onTextChangeDelegate onChange;

        private TextEditor cached_editor = null;
        private TextEditor editor { get { if (cached_editor == null) { cached_editor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), this.id); cached_editor.controlID = this.id; cached_editor.content = new GUIContent(""); } return cached_editor; } }

        private string control_name { get { return String.Format("text_area_{0}", id); } }
        private string pre_focus_text = null;// if this value is NULL then no checking is done, if it has a string value then each frame if this control is not focused and this value does not equal the current text value the onChange event will fire and this will be set to NULL.

        /// <summary>
        /// Does this control have keyboard focus?
        /// </summary>
        private bool hasFocus { get { return (this.id == GUIUtility.keyboardControl); } }


        public uiTextbox() : base(uiControlType.Textbox)
        {
            this.focus_type = FocusType.Keyboard;

            this.border.normal.size = new RectOffset(1, 1, 1, 1);
            this.border.normal.color = new Color(1f, 1f, 1f, 0.2f);

            this.border.focused.color = new Color(1f, 1f, 1f, 0.4f);

            this.padding = new RectOffset(3, 3, 3, 3);
            this.update_area();
        }

        public void Focus()
        {
            if (hasFocus) return;
            pre_focus_text = this.text;

            GUIUtility.keyboardControl = this.id;
            editor.OnFocus();
        }

        public void Unfocus()
        {
            if (!hasFocus) return;
            if (GUIUtility.keyboardControl == this.id)
            {
                GUIUtility.keyboardControl = 0;
                editor.OnLostFocus();
            }

            if (String.Compare(pre_focus_text, this.text) != 0) this.onChange(this, this.text);
            pre_focus_text = null;
        }

        protected override Vector2 Get_Autosize()
        {
            if (content != null)
            {
                Vector2 ret = new Vector2(_size.x, 6f + style.lineHeight);
                return ret;
            }
            return base.Get_Autosize();
        }

        public override void handleEvent()
        {
            Event evt = Event.current;
            if (hasFocus && evt.isKey)
            {
                if(evt.keyCode == KeyCode.KeypadEnter || evt.keyCode == KeyCode.Return)
                {
                    evt.Use();
                    Unfocus();
                    return;
                }
                bool ate = editor.HandleKeyEvent(evt);
                if(ate)
                {
                    evt.Use();
                }
                else if (!char.IsControl(evt.character) && evt.rawType == EventType.KeyDown)
                {
                    int plen = editor.content.text.Length;
                    editor.Insert(evt.character);
                    //editor.cursorIndex += (editor.content.text.Length - plen);
                    editor.cursorIndex = editor.cursorIndex;// This causes the editor to set a flag back to true which enables the cursor to draw again...
                    evt.Use();
                }

                this.content.text = editor.content.text;// We need to copy the current text from the editor object to our content object NOW because the editor instance content doesn't seem to contain anything but a blank string inbetween frames?
            }


            bool wasFocused = hasFocus;
            switch (evt.GetTypeForControl(this.id))
            {
                case EventType.MouseDown:
                    if (absArea.Contains(evt.mousePosition))
                    {
                        evt.Use();
                        Focus();
                        Vector2 clickPos = this.absToRelativePos(evt.mousePosition);
                        editor.MoveCursorToPosition(clickPos);
                    }
                    else//unfocus the control if it's the currently focused one but isnt the one getting clicked.
                    {
                        Unfocus();
                    }
                    break;
                case EventType.MouseDrag:
                    if (absArea.Contains(evt.mousePosition))
                    {
                        evt.Use();
                        if (hasFocus)
                        {
                            Vector2 clickPos = this.absToRelativePos(evt.mousePosition);
                            editor.SelectToPosition(clickPos);
                        }
                    }
                    break;
                default:
                    base.handleEvent();
                    break;
            }
        }

        public override void doLayout()
        {
            editor.position = draw_area;
        }

        protected override void Display()
        {
            base.Display();// Draw BG
            int mx = Math.Max(editor.cursorIndex, editor.selectIndex);
            int mn = Math.Min(editor.cursorIndex, editor.selectIndex);
            styleNoBG.DrawWithTextSelection(inner_area, editor.content, this.id, mn, mx);
            if (hasFocus) styleNoBG.DrawCursor(inner_area, editor.content, this.id, editor.cursorIndex);
        }
    }
}
