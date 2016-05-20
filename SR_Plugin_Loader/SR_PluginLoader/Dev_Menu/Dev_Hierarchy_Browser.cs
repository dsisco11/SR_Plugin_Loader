using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SR_PluginLoader
{
    public class Dev_Hierarchy_Browser: uiPanel
    {
        GameObject selection = null;

        uiListView list = null;
        uiWrapperPanel info_panel = null;
        uiText lbl_components = null;
        uiListView var_components = null;
        uiVarText var_child_count = null;


        public Dev_Hierarchy_Browser()
        {
            Autosize = true;
            Autosize_Method = AutosizeMethod.FILL;

            list = uiControl.Create<uiListView>(this);
            list.Set_Width(300);
            list.Autosize = false;
            //list.CONFIRM_LAYOUT = true;


            info_panel = uiControl.Create<uiWrapperPanel>(this);
            info_panel.onLayout += Info_panel_onLayout;
            info_panel.Set_Padding(3);

            lbl_components = Create<uiText>(info_panel);
            lbl_components.Text = "Scripts";

            var_components = Create<uiListView>(info_panel);

            var_child_count = Create<uiVarText>(info_panel);
            var_child_count.Text = "Children: ";

            Init();
        }

        private void Update_info()
        {
            var_child_count.Value = (selection == null ? 0 : selection.transform.childCount).ToString();
            var_components.Clear_Children();
            if(selection!=null)
            {
                Component[] comps = selection.GetComponents<Component>();
                foreach(var c in comps)
                {
                    uiListItem n = Create<uiListItem>(var_components);
                    n.Title = c.GetType().Name;
                    n.TextSize = 12;
                    n.Selectable = false;
                }
            }
        }

        private void Info_panel_onLayout(uiPanel c)
        {
            var_child_count.alignTop();

            lbl_components.moveBelow(var_child_count);

            var_components.moveBelow(lbl_components);
            var_components.FloodXY();
        }

        public override void doLayout()
        {
            base.doLayout();
            list.FloodY();
            info_panel.moveRightOf(list);
            info_panel.FloodXY();
        }

        private uiList_TreeNode Spawn_Node(GameObject gm, uiPanel parent)
        {
            uiList_TreeNode node = Create<uiList_TreeNode>(parent);
            node.tag = gm;
            node.Title = gm.name;
            node.onExpanded += Populate_Node;
            node.onClicked += (uiControl c) => { var sel = ((c as uiList_TreeNode).tag as GameObject);  if (sel != selection) { selection = sel; }  Update_info(); };
            return node;
        }

        private void Init()
        {
            HashSet<GameObject> rootObjects = new HashSet<GameObject>();
            foreach (Transform xform in UnityEngine.Object.FindObjectsOfType<Transform>())
            {
                if (xform.parent == null && xform.gameObject!=null)
                {
                    rootObjects.Add(xform.gameObject);
                }
            }

            foreach(GameObject gm in rootObjects)
            {
                var node = Spawn_Node(gm, list);
                Populate_Single_Node(node);
            }
        }

        private void Populate_Node(uiList_TreeNode node)
        {
            node.Clear_Children();
            if (node.tag == null) return;
            Populate_Single_Node(node);
            foreach(uiList_TreeNode n in node.Get_Children())
            {
                Populate_Single_Node(n);
            }
        }

        private void Populate_Single_Node(uiList_TreeNode node)
        {
            node.Clear_Children();
            if (node.tag == null) return;
            GameObject gm = (node.tag as GameObject);
            for (int i = 0; i < gm.transform.childCount; i++)
            {
                Transform trans = gm.transform.GetChild(i);
                if (trans.gameObject == trans.parent || trans.parent == null) continue;
                Spawn_Node(trans.gameObject, node);
            }
        }
    }
}
