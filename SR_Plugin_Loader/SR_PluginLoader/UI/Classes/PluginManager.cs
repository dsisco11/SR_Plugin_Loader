using System;
using System.Collections.Generic;
using UnityEngine;


namespace SR_PluginLoader
{
    public class PluginManager : uiWindow
    {
        public static PluginManager Instance = null;

        private string selected = null;//the hash of the currently selected plugin
        private uiListView list = null;
        private uiTabPanel tabPanel = null;

        private uiText pl_title = null, pl_auth = null, pl_vers = null;
        private uiTextArea pl_desc = null;
        private uiIcon pl_thumb = null;
        private uiButton btn_copy_json = null;
        private uiToggle pl_toggle = null;
        private uiTextArea ins_title = null, ins_text = null, ins_no_plugins_text = null;
        private uiIconButton btn_store = null;
        private uiWrapperPanel nop_wrapper = null;
        private uiTab pl_tab = null, tab_need_plugins = null, tab_ins = null;
        private uiCollapser control_panel = null;

        const string PLUGIN_TAB_NAME = "plugins_tab";
        const string INSTRUCTION_TAB_NAME = "instructions_tab";
        const string NEED_PLUGINS_TAB_NAME = "need_plugins_tab";

        public PluginManager()
        {
            onLayout += PluginManager_onLayout;
            Title = "Plugin Manager";
            Set_Size(650, 400);
            Center();
            onShown += PluginManager_onShown;
            onHidden += PluginManager_onHidden;

            list = Create<uiListView>(this);
            list.Set_Width(200f);
            list.Set_Margin(2, 0, 2, 2);


            btn_store = Create<uiIconButton>(this);
            btn_store.Text = "Plugin Store";
            btn_store.Icon = TextureHelper.icon_arrow_left;
            btn_store.Border.type = uiBorderType.NONE;
            //btn_store.Skin = uiSkinPreset.FLAT;
            btn_store.Border.normal.color = Color.white;
            btn_store.Border.normal.size = new RectOffset(1, 1, 1, 1);
            btn_store.onClicked += btn_store_onClicked;


            //CONTROL PANEL
            control_panel = Create<uiCollapser>("control_panel", this);
            control_panel.Autosize_Method = AutosizeMethod.BLOCK;
            control_panel.Set_Margin(2, 2, 2, 0);
            control_panel.Set_Padding(2);
            Util.Set_BG_Color(control_panel.local_style.normal, new Color(0f, 0f, 0f, 0.2f));
            control_panel.onLayout += Control_panel_onLayout;
            control_panel.Collapse();

            pl_toggle = Create<uiToggle>("pl_toggle", control_panel);
            pl_toggle.onChange += Pl_toggle_onChange;

            btn_copy_json = Create<uiButton>("copy_json", control_panel);
            btn_copy_json.Text = "Copy Json";
            btn_copy_json.Set_Margin(3, 0);
            btn_copy_json.onClicked += Btn_copy_json_onClicked;


            // TABBED PANEL
            tabPanel = Create<uiTabPanel>(this);
            tabPanel.Autosize_Method = AutosizeMethod.FILL;
            tabPanel.disableBG = true;
            tabPanel.Set_Margin(2);// This margin gives us that light colored area surrounding the list and tabpanel
            pl_tab = tabPanel.Add_Tab(PLUGIN_TAB_NAME);
            tabPanel.CurrentTab.onLayout += tabPanel_onLayout;
            tabPanel.onChanged += TabPanel_onChanged;

            pl_tab.Set_Padding(3);// so we can distinguish where the plugin thumbnail's borders are

            pl_title = Create<uiText>("pl_title", tabPanel);
            pl_title.local_style.fontSize = 22;
            pl_title.local_style.fontStyle = FontStyle.Bold;


            pl_auth = Create<uiText>("pl_author", tabPanel);
            pl_vers = Create<uiText>("pl_vers", tabPanel);
            pl_desc = Create<uiTextArea>("pl_desc", tabPanel);

            pl_thumb = Create<uiIcon>("pl_thumb", tabPanel);
            pl_thumb.Border.normal.color = new Color(0f, 0f, 0f, 0.3f);

            pl_desc.Set_Margin(2);
            pl_desc.Set_Background(new Color(0.1f, 0.1f, 0.1f, 0.4f));
            pl_desc.Set_Padding(2);
            pl_desc.Border.normal.color = new Color(0f, 0f, 0f, 0.3f);
            pl_desc.Autosize_Method = AutosizeMethod.BLOCK;

            // INSTRUCTIONS TAB
            tab_ins = tabPanel.Add_Tab(INSTRUCTION_TAB_NAME);
            tabPanel.CurrentTab.onLayout += InstructionsTab_onLayout;

            ins_title = Create<uiTextArea>(tabPanel);
            ins_title.Text = "Settings";
            ins_title.TextSize = 20;
            ins_title.TextStyle = FontStyle.Bold;

            ins_text = Create<uiTextArea>(tabPanel);
            ins_text.Text = "Select a plugin from the list on the left to manage it's settings.\nOr, to browse more plugins for download, click the button below!";
            ins_text.TextStyle = FontStyle.Italic;
            ins_text.Autosize = true;


            tab_need_plugins = tabPanel.Add_Tab(NEED_PLUGINS_TAB_NAME);
            tabPanel.CurrentTab.onLayout += NeedPluginsTab_onLayout;

            nop_wrapper = Create<uiWrapperPanel>(tabPanel);
            nop_wrapper.onLayout += Nop_Wrapper_onLayout;

            ins_no_plugins_text = Create<uiTextArea>(nop_wrapper);
            ins_no_plugins_text.Text = "You do not have any plugins installed!\nVisit the plugin store to find and install new plugins.";
            ins_no_plugins_text.TextSize = 16;
            ins_no_plugins_text.TextColor = new Color(1f, 0.1f, 0.1f, 0.9f);
            ins_no_plugins_text.TextStyle = FontStyle.Bold;
            ins_no_plugins_text.TextAlign = TextAnchor.UpperCenter;
            ins_no_plugins_text.Autosize = true;
        }

        private void PluginManager_onLayout(uiPanel c)
        {
        }

        public override void doLayout()
        {
            base.doLayout();

            btn_store.alignTop(2);
            btn_store.alignLeftSide(2);

            list.alignLeftSide();

            control_panel.alignTop();
            control_panel.moveRightOf(list);

            list.moveBelow((control_panel.isCollapsed ? btn_store as uiControl : control_panel as uiControl));
            list.FloodY();

            tabPanel.moveBelow(control_panel);
            tabPanel.moveRightOf(list);
            tabPanel.FloodXY();
        }

        private void TabPanel_onChanged(uiTabPanel arg1, uiTab tab)
        {
            control_panel.Set_Collapsed(tab != pl_tab);
        }

        private void Control_panel_onLayout(uiControl c)
        {
            pl_toggle.alignLeftSide();
            btn_copy_json.moveRightOf(pl_toggle, 3);
        }

        private void btn_store_onClicked(uiControl c)
        {
            //PluginManager.Instance.Hide();
            //PluginStore.Instance.Show();
            uiWindow.Switch(PluginStore.Instance);
        }

        private void Nop_Wrapper_onLayout(uiControl c)
        {
            ins_no_plugins_text.Set_Pos(0, 0);
        }

        private void NeedPluginsTab_onLayout(uiControl c)
        {
            nop_wrapper.CenterVertically();
            nop_wrapper.CenterHorizontally();
        }

        private void InstructionsTab_onLayout(uiControl c)
        {
            ins_title.alignTop(5);
            ins_title.CenterHorizontally();

            ins_text.moveBelow(ins_title, 6);
            ins_text.CenterHorizontally();
        }

        private void tabPanel_onLayout(uiControl c)
        {
            pl_thumb.alignTop();
            pl_thumb.CenterHorizontally();
            
            //pl_toggle.CenterHorizontally();
            //pl_toggle.moveBelow(pl_thumb, 5);
            
            pl_title.moveBelow(pl_thumb, 10);
            pl_title.CenterHorizontally();

            pl_vers.moveBelow(pl_title, 1);
            pl_vers.CenterHorizontally();

            pl_auth.moveBelow(pl_vers, 1);
            pl_auth.CenterHorizontally();

            pl_desc.alignLeftSide(3);
            pl_desc.moveBelow(pl_auth, 10);

            //btn_copy_json.moveBelow(pl_desc, 5);
        }

        private void Pl_toggle_onChange(uiToggle c, bool was_clicked)
        {
            if (!was_clicked) return;//if the control didnt change due to being clicked then it changed because WE manually set the state. which we don't want to react to or we will enter a loop.

            if (c.isChecked) GetPlugin().Enable();
            else GetPlugin().Disable();
        }

        private void Btn_copy_json_onClicked(uiControl c)
        {
            Plugin pl = Loader.Get_Plugin(this.selected);
            if (pl == null)
            {
                DebugHud.Log("Unable to find plugin via hash: {0}", this.selected);
                return;
            }
            /*
            string json_str = String.Format("\"{0}.{1}\": {{\n\t\t \"name\": \"{0}\",\n\t\t\t \"author\": \"{1}\",\n\t\t\t \"description\": \"{2}\",\n\t\t\t \"update_method\": \"{3}\",\n\t\t\t \"url\": \"{4}\"\n\t\t }}", pl.data.NAME, pl.data.AUTHOR, Util.JSON_Escape_String(pl.data.DESCRIPTION), Enum.GetName(typeof(UPDATER_TYPE), pl.data.UPDATE_METHOD.METHOD), pl.data.UPDATE_METHOD.URL);
            GUIUtility.systemCopyBuffer = json_str;//copy this text to the clipboard
            */
            GUIUtility.systemCopyBuffer = pl.data.ToJSON();
        }
        
        private void PluginManager_onShown(uiControl c)
        {
            Update_Plugins_List();
        }
        
        private void PluginManager_onHidden(uiWindow c)
        {
            //tabPanel.Set_Tab(INSTRUCTION_TAB_NAME);
            tab_ins.Select();
        }

        public void Update_Plugins_List()
        {
            set_layout_dirty();
            list.Clear_Children();

            float dY = 3f;
            foreach (KeyValuePair<string, Plugin> kv in Loader.plugins)
            {
                try
                {
                    var sel = Create<Plugin_Manager_List_Item>();
                    sel.Set_Plugin(kv.Value);
                    sel.onClicked += Sel_onClicked;
                    list.Add(kv.Value.Hash, sel);
                }
                catch (Exception ex)
                {
                    DebugHud.Log(ex);
                }
            }

            if (list.isEmpty) tab_need_plugins.Select();// tabPanel.Set_Tab(NEED_PLUGINS_TAB_NAME);
            else tab_ins.Select();// tabPanel.Set_Tab(INSTRUCTION_TAB_NAME);
            // Set the very first plugin in our list as the active one
            //if (Loader.plugins.Count > 0) { this.Select_Plugin(Loader.plugins.First().Value); }
        }

        private void Sel_onClicked(uiControl c)
        {
            Plugin_Manager_List_Item sel = c as Plugin_Manager_List_Item;
            this.Select_Plugin(sel.Get_Plugin());
        }

        private void Select_Plugin(Plugin p)
        {
            set_layout_dirty();
            if (!list.isEmpty)
            {
                pl_tab.Select();
                //set ALL selectors to inactive first.
                foreach (var sel in list.Get_Children())
                {
                    ((Plugin_Manager_List_Item)sel).Active = false;
                }
            }
            else
            {
                tab_ins.Select();
            }


            selected = null;
            if (p != null)
            {
                selected = p.Hash;
                Plugin_Manager_List_Item sel = list[p.Hash] as Plugin_Manager_List_Item;
                if(sel != null) sel.Active = true;
            }

            control_panel.Set_Collapsed(selected==null);

            float thumb_aspect = 1f;
            float thumb_sz = 256f;

            if (p == null || p.data == null)
            {
                this.pl_title.Text = "";
                this.pl_auth.Text = "";
                this.pl_desc.Text = "";
                //this.pl_desc.isDisabled = true;

                this.pl_vers.Text = "";
                this.pl_thumb.image = null;
                this.pl_toggle.isVisible = false;
                this.pl_toggle.isChecked = false;
            }
            else
            {
                this.pl_title.Text = p.data.NAME;
                this.pl_auth.Text = String.Format("<color=#808080ff>Author:</color> {0}", p.data.AUTHOR);
                this.pl_desc.Text = (string.IsNullOrEmpty(p.data.DESCRIPTION) ? "<b><color=#808080ff>No Description</color></b>" : p.data.DESCRIPTION);
                //this.pl_desc.isDisabled = false;

                this.pl_vers.Text = p.data.VERSION.ToString();
                this.pl_thumb.image = p.thumbnail;
                this.pl_toggle.isVisible = true;
                this.pl_toggle.isChecked = p.enabled;
            }

            if (this.pl_thumb.image == null) thumb_sz = 0f;
            else thumb_aspect = ((float)this.pl_thumb.image.height / (float)this.pl_thumb.image.width);

            float thumb_height = (thumb_sz * thumb_aspect);
            pl_thumb.Set_Size(thumb_sz, thumb_height);
        }

        private Plugin GetPlugin()
        {
            return Loader.Get_Plugin(this.selected);
        }

    }
}
