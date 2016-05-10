using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SR_PluginLoader
{
    public class uiUpdatesAvailable : uiDialogResult
    {
        private uiListView list = null;

        public uiUpdatesAvailable()
        {
            Title = "Updates Available";
            Set_Size(300, 600);
            Center();

            contentPanel.onLayout += ContentPanel_onLayout;

            message.Text = "Updates are available!\nThe files which will be updated are listed below.";
            message.TextAlign = TextAnchor.UpperCenter;

            list = Create<uiListView>("list", contentPanel);
            //list.Set_Background(new UnityEngine.Color(0f, 0f, 0f, 0.2f));
            //list.Autosize_Method = AutosizeMethod.FILL;
            list.Autosize = false;
            //list.Set_Background(Color.clear);
            list.disableBG = true;
        }

        private void ContentPanel_onLayout(uiPanel c)
        {
            list.FloodXY();
        }

        public void Add_File(string filename)
        {
            var itm = Create<uiListItem_Progress>(filename, list);
            itm.Selectable = false;
            itm.Clickable = false;
            itm.Text = Path.GetFileName(filename);
            itm.TextStyle = FontStyle.Bold;
            itm.TextSize = 14;
        }
    }
}
