using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SR_PluginLoader
{
    public class uiListView : uiPanel
    {
        private uiControl current_selection = null;
        private ILayoutDirector layout = new Layout_Default();
        public ILayoutDirector Layout { get { return layout; } set { layout = value; update_area(); } }

        public uiListView() : base(uiControlType.Panel)
        {
            this.CanScroll = true;
        }

        public override uiControl Add(uiControl child)
        {
            base.Add(child);
            child.onSelected += (uiControl c) => {
                change_selection(c);
            };
            return child;
        }

        private void change_selection(uiControl select)
        {
            if (current_selection != select && current_selection != null) current_selection.active = false;
            current_selection = select;
            if (select != null) select.active = true;
        }

        public override void doLayout()
        {
            layout.Handle(this, children.ToArray());
        }
    }
}
