using UnityEngine;

namespace SR_PluginLoader
{
    public class uiText : uiControl
    {
        public uiText() : base(uiControlType.Text) { init(); }
        public uiText(uiControlType type) : base(type) { init(); }

        private void init()
        {
            Set_Padding(1);
        }

        /// <summary>
        /// The only difference between this overriden Autosizing logic and the default logic is that this overriden logic will give the control 0 width when it has no text.
        /// </summary>
        protected override Vector2 Get_Autosize(Vector2? starting_size = null)
        {
            if (content == null || content.text == null || content.text.Length <= 0) return base.Get_Autosize(content_size_to_inner(new Vector2(0f, Style.lineHeight)));
            
            Vector2 csz = styleText.CalcSize(content);
            Vector2 acsz = Padding.Add(new Rect(Vector2.zero, csz)).size;
            return base.Get_Autosize(acsz);
            
            //return base.Get_Autosize(starting_size);
        }

        protected override void Display()
        {
            if (CONFIRM_DRAW) { DebugHud.Log("[{0}]({1})  Confirm Display  |  draw_area: {2}", this, Typename, draw_area); }
            Display_BG();// Draw BG
            Display_Text();// Draw our text
            /*
            Display_BG();// Draw Background
            styleText.Draw(_inner_area, content, isMouseOver || isActive, isActive, false, isFocused);// Draw text
            */
        }
    }
}
