using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SR_PluginLoader
{
    /// <summary>
    /// A <see cref="uiListItem"/> which puts focus on it's icon with text below it.
    /// </summary>
    class uiListIcon : uiListItem
    {
        public uiListIcon() : base()
        {
            title.local_style.fontSize = 12;
            title.local_style.wordWrap = true;
            icon.Autosize = false;
        }
        
        public override void doLayout()
        {
            icon.alignTop();
            icon.Set_Width(icon.size.y);
            icon.CenterHorizontally();
            icon.FloodY(title.size.y);

            title.CenterHorizontally();
            title.moveBelow(icon);
        }
    }
}
