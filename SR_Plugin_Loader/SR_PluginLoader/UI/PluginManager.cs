using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace SR_PluginLoader
{
    public class PluginManager : uiWindow
    {
        private string selected = null;//the hash of the currently selected plugin
        private uiListView list = null;
        private uiPanel info_panel = null;
        private uiText pl_title = null, pl_auth = null, pl_vers = null;
        private uiTextArea pl_desc = null;
        private uiIcon pl_thumb = null;
        private uiButton btn_download = null, btn_copy_json = null;
        private PluginStore plugin_store = null;
        private uiToggle pl_toggle = null;


        public PluginManager()
        {
            this.title = "Plugin Manager";
            this.Set_Size(650, 400);
            this.Center();
            this.onClosed += PluginManager_onClosed;
            this.onShown += PluginManager_onShown;

            plugin_store = Create<PluginStore>();
            plugin_store.onClosed += Plugin_store_onClosed;

            list = Create<uiListView>(this);
            list.Set_Width(200f);

            btn_download = Create<uiButton>(this);
            btn_download.text = "Download Plugins";
            btn_download.Set_Width(200f);
            btn_download.local_style.fontSize = 16;
            btn_download.local_style.fontStyle = FontStyle.Bold;
            btn_download.margin = new RectOffset(3, 3, 0, 0);
            btn_download.onClicked += Btn_download_onClicked;

            info_panel = Create<uiScrollView>();
            info_panel.margin = new RectOffset(2, 2, 2, 2);
            info_panel.padding = new RectOffset(5, 5, 5, 5);
            info_panel.onLayout += Info_panel_onLayout;

            pl_title = Create<uiText>(info_panel);
            pl_title.local_style.fontSize = 22;
            pl_title.local_style.fontStyle = FontStyle.Bold;

            pl_toggle = Create<uiToggle>(info_panel);
            pl_toggle.local_style.fontSize = 14;
            pl_toggle.onChange += Pl_toggle_onChange;

            pl_auth = Create<uiText>(info_panel);
            pl_desc = Create<uiTextArea>(info_panel);
            pl_vers = Create<uiText>(info_panel);

            pl_thumb = Create<uiIcon>(info_panel);

            btn_copy_json = Create<uiButton>(info_panel);
            btn_copy_json.text = "Copy Json";
            btn_copy_json.onClicked += Btn_copy_json_onClicked;

            this.Add(info_panel);

        }

        public override void doLayout()
        {
            base.doLayout();

            btn_download.alignTop(2f);
            list.moveBelow(btn_download, 2f);
            list.floodY(2f);

            Vector2 isz = new Vector2((inner_area.width - list.area.width), inner_area.height);
            info_panel.Set_Size(isz);
            info_panel.moveRightOf(list);
        }

        private void Info_panel_onLayout(uiPanel c)
        {
            pl_thumb.CenterX();
            pl_toggle.CenterX();
            pl_toggle.moveBelow(pl_thumb, 5f);

            pl_title.moveBelow(pl_toggle, 10f);
            pl_title.CenterX();

            pl_vers.moveBelow(pl_title, 2f);
            pl_vers.CenterX();

            pl_auth.moveBelow(pl_vers, 2f);
            pl_auth.CenterX();

            pl_desc.moveBelow(pl_auth, 10f);

            btn_copy_json.moveBelow(pl_desc, 5f);
        }

        private void Pl_toggle_onChange(uiToggle c, bool was_clicked)
        {
            if (!was_clicked) return;//if the control didnt change due to being clicked then it changed because WE manually set the state. which we don't want to react to or we will enter a loop.

            if (c.isChecked) this.GetPlugin().Enable();
            else this.GetPlugin().Disable();
        }

        private void Btn_copy_json_onClicked(uiControl c)
        {
            Plugin pl = Loader.Get_Plugin(this.selected);
            if (pl == null)
            {
                DebugHud.Log("Unable to find plugin via hash: {0}", this.selected);
                return;
            }

            string json_str = String.Format("\"{0}.{1}\": {{\n\t\t \"name\": \"{0}\",\n\t\t\t \"author\": \"{1}\",\n\t\t\t \"description\": \"{2}\",\n\t\t\t \"update_method\": \"{3}\",\n\t\t\t \"url\": \"{4}\"\n\t\t }}", pl.data.NAME, pl.data.AUTHOR, Utility.JSON_Escape_String(pl.data.DESCRIPTION), Enum.GetName(typeof(UPDATER_TYPE), pl.data.UPDATE_METHOD.METHOD), pl.data.UPDATE_METHOD.URL);
            GUIUtility.systemCopyBuffer = json_str;//copy this text to the clipboard
        }

        private void Plugin_store_onClosed(uiControl c)
        {
            this.Show();
        }

        private void Btn_download_onClicked(uiControl c)
        {
            this.Hide();
            plugin_store.Show();
        }

        private void PluginManager_onShown(uiControl c)
        {
            this.Update_Plugins_List();
        }

        private void PluginManager_onClosed(uiControl c)
        {
            MainMenu.mainmenu.SetActive(true);
        }

        public void Update_Plugins_List()
        {
            this.needs_layout = true;
            this.list.Clear_Children();

            float dY = 3f;
            foreach (KeyValuePair<string, Plugin> kv in Loader.plugins)
            {
                try
                {
                    var sel = Create<PluginSelector>();
                    sel.Set_Plugin(kv.Value);
                    sel.onClicked += Sel_onClicked;
                    this.list.Add(kv.Value.Hash, sel);
                }
                catch (Exception ex)
                {
                    DebugHud.Log(ex);
                }
            }

            if (Loader.plugins.Count > 0)
            {
                this.Select_Plugin(Loader.plugins.First().Value);
            }
        }

        private void Sel_onClicked(uiControl c)
        {
            PluginSelector sel = c as PluginSelector;
            this.Select_Plugin(sel.Get_Plugin());
        }

        private void Select_Plugin(Plugin p)
        {
            this.needs_layout = true;
            //set ALL selectors to inactive first.
            foreach (var sel in this.list.Get_Children())
            {
                ((PluginSelector)sel).active = false;
            }

            this.selected = p.Hash;
            if (p != null)
            {
                PluginSelector sel = list[p.Hash] as PluginSelector;
                sel.active = true;
            }

            float thumb_aspect = 1f;
            float thumb_sz = 256f;

            if (p == null || p.data == null)
            {
                this.pl_title.text = "";
                this.pl_auth.text = "";
                this.pl_desc.text = "";
                this.pl_vers.text = "";
                this.pl_thumb.image = null;
                this.pl_toggle.visible = false;
                this.pl_toggle.isChecked = false;
            }
            else
            {
                this.pl_title.text = p.data.NAME;
                this.pl_auth.text = String.Format("<color=#808080ff>Author:</color> {0}", p.data.AUTHOR);
                this.pl_desc.text = p.data.DESCRIPTION;
                this.pl_vers.text = p.data.VERSION.ToString();
                this.pl_thumb.image = p.thumbnail;
                this.pl_toggle.visible = true;
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
