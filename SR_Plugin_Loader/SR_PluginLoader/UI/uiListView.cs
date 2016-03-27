using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SR_PluginLoader
{
    public class uiListView : uiPanel
    {
        public uiListView() : base(uiControlType.Panel)
        {
            this.CanScroll = true;
        }

        public override void doLayout()
        {
            float yPos = 0f;
            for (int i=0; i<children.Count; i++)
            {
                var child = children[i];
                if (child == null) continue;

                child.Set_Pos(0f, yPos);
                yPos += child.size.y;
            }
        }
    }
}
