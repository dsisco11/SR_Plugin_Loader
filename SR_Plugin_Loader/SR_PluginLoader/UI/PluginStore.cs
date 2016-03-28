using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SimpleJSON;
using System.Net;
using System.IO;

namespace SR_PluginLoader
{
    /// <summary>
    /// A "form" that displays the plugins in the master list from the github page
    /// Users can view all the plugins and pick ones to download & install!
    /// </summary>
    public class PluginStore : uiWindow
    {
        private readonly string PLUGINS_LIST_URL = "https://github.com/dsisco11/SR_Plugin_Loader/raw/master/MASTER_PLUGINS_LIST.json";
        private Dictionary<string, Plugin_Download_Data> plugins = new Dictionary<string, Plugin_Download_Data>();
        private uiListView list = null;
        private uiTextbox search = null;
        private uiText lbl_search = null, pl_title = null, pl_auth = null;
        private uiVarText lbl_pl_count = null;
        private uiTextArea pl_desc = null;
        private bool loaded = false, pending_rebuild = false;
        private Plugin_StoreItem selected_plugin = null;
        private uiButton install_btn = null;
        private uiPanel info_panel = null;


        public PluginStore()
        {
            this.title = "Install Plugins";
            this.Set_Size(800, 600);
            this.Center();

            lbl_pl_count = Create<uiVarText>(this);
            lbl_pl_count.label = "Total Plugins:";
            lbl_pl_count.text = "0";
            lbl_pl_count.label_style.fontStyle = FontStyle.Bold;

            list = uiControl.Create<uiListView>(this);


            search = uiControl.Create<uiTextbox>(this);
            search.onChange += Search_onChange;

            lbl_search = uiControl.Create<uiText>(this);
            lbl_search.text = "Search ";

            info_panel = Create<uiPanel>();
            info_panel.padding = new RectOffset(4, 4, 4, 4);
            info_panel.onLayout += Info_panel_onLayout;


            pl_title = uiControl.Create<uiText>(info_panel);
            pl_title.text = "";
            var sty = new GUIStyle(pl_title.style);
            sty.fontStyle = FontStyle.Bold;
            sty.fontSize = 22;
            pl_title.Set_Style(sty);


            pl_auth = uiControl.Create<uiText>(info_panel);
            pl_auth.text = "";

            pl_desc = uiControl.Create<uiTextArea>(info_panel);
            pl_desc.text = "";


            install_btn = uiControl.Create<uiButton>(info_panel);
            install_btn.local_style.fontSize = 18;
            install_btn.local_style.fontStyle = FontStyle.Bold;
            install_btn.text = "Install";
            install_btn.onClicked += Install_btn_onClicked;
            install_btn.visible = false;

            this.Add(info_panel);
        }

        public override void doLayout()
        {
            lbl_pl_count.alignTop(5f);
            lbl_pl_count.alignLeftSide(5f);

            list.Set_Width(Plugin_StoreItem.DEFAULT_WIDTH);


            float xPad = 5f;
            lbl_search.alignTop(10f);
            lbl_search.moveRightOf(list, xPad);

            search.alignTop(10f);
            search.moveRightOf(lbl_search);
            search.floodX(12f);
            if (search.size.y != lbl_search.size.y) lbl_search.Set_Height(search.size.y);

            list.moveBelow(search, 10f);
            list.floodY();

            info_panel.moveBelow(search, 10f);
            info_panel.moveRightOf(list, 2f);
            info_panel.floodY();
            info_panel.floodX();
        }

        private void Info_panel_onLayout(uiPanel c)
        {
            install_btn.CenterX();

            pl_title.moveBelow(install_btn, 10f);
            pl_title.CenterX();

            pl_auth.moveBelow(pl_title, 2f);
            pl_auth.CenterX();

            pl_desc.moveBelow(pl_auth, 5f);
        }


        private void Install_btn_onClicked(uiControl c)
        {
            if (selected_plugin == null) return;
            var hash = selected_plugin.plugin_hash;
            string url = this.plugins[hash].URL;
            if(url == null || url.Length <= 0)
            {
                DebugHud.Log("Cannot download plugin, Invalid URL: {0}", url);
                return;
            }

            Download_Plugin(hash, url);
        }

        private void Search_onChange(uiControl c, string str)
        {
            pending_rebuild = true;
        }
       
        private void Select_Plugin(uiControl c)
        {
            Plugin_StoreItem pl = (Plugin_StoreItem)c;
            if (selected_plugin != null)
                selected_plugin.active = false;
            selected_plugin = pl;

            if (c == null)
            {
                pl_title.text = "";
                pl_auth.text = "N/A";
                pl_desc.text = "N/A";
                install_btn.visible = false;
                return;
            }
            c.active = true;
            Plugin_Download_Data data = this.plugins[pl.plugin_hash];

            pl_title.text = data.Name;
            pl_auth.text = String.Format("<color=#808080ff>Author:</color> {0}", data.Author);
            pl_desc.text = data.Description;
            if (pl_desc.text == null || pl_desc.text.Length <= 0) pl_desc.text = "N/A";

            install_btn.visible = !data.isInstalled;
        }

        public void Update()
        {
            if (pending_rebuild) Rebuild_Plugins_UI();
        }

        protected override void Display()
        {
            if(!loaded) this.Update_Plugins_List();
            base.Display();// Draw BG
        }

        private void Update_Plugins_List()
        {
            loaded = true;
            StartCoroutine(Client_Plugins_List_Update());
        }
        
        private IEnumerator Client_Plugins_List_Update()
        {
            var iter = Git_Updater.instance.Cache_And_Open_File(PLUGINS_LIST_URL);
            while (iter.MoveNext()) yield return null;
            
            try
            {
                FileStream strm = iter.Current as FileStream;

                byte[] data = Utility.Read_Stream(strm);
                var list = JSON.Parse(Encoding.ASCII.GetString(data))["plugins"];
                this.plugins.Clear();
                
                foreach (JSONNode json in list.Childs)
                {
                    var dat = new Plugin_Download_Data(json);
                    this.plugins[dat.Hash] = dat;
                }

                lbl_pl_count.text = this.plugins.Count.ToString();
                pending_rebuild = true;
            }
            catch(Exception ex)
            {
                DebugHud.Log("Error while updating plugins list.");
                DebugHud.Log(ex);
            }
            yield break;
        }

        private void Rebuild_Plugins_UI()
        {
            pending_rebuild = false;
            list.Clear_Children();
            string search_str = search.text.ToLower();

            // add all the plugins to the sidebar
            foreach (KeyValuePair<string, Plugin_Download_Data> kv in this.plugins)
            {
                Plugin_Download_Data plugin = kv.Value;

                Plugin_Data dat = new Plugin_Data()
                {
                    AUTHOR = plugin.Author,
                    NAME = plugin.Name,
                    DESCRIPTION = plugin.Description
                };
                
                if (search_str.Length > 0)
                {
                    bool match = false;
                    if (plugin.Name.ToLower().IndexOf(search_str) > -1) match = true;
                    if (plugin.Author.ToLower().IndexOf(search_str) > -1) match = true;

                    if (!match) continue;
                }
                              

                Plugin_StoreItem itm = uiControl.Create<Plugin_StoreItem>();
                itm.onClicked += this.Select_Plugin;
                itm.Set_Plugin_Data(dat);
                list.Add(itm.plugin_hash, itm);
            }

            // select one of the plugins 
            if (list.Get_Children().Count > 0)
            {
                if (this.selected_plugin == null)
                {
                    Select_Plugin(list.Get_Children()[0]);
                }
                else
                {
                    uiControl found = null;
                    foreach (uiControl c in this.list.Get_Children())
                    {
                        Plugin_StoreItem itm = (Plugin_StoreItem)c;
                        if (itm.plugin_hash == selected_plugin.plugin_hash)
                        {
                            found = c;
                            break;
                        }
                    }

                    if (found != null) Select_Plugin(found);
                    else Select_Plugin(list.Get_Children()[0]);
                }
            }
            else Select_Plugin(null);

        }

        private void Download_Plugin(string hash, string url)
        {
            Plugin_Download_Data plData = this.plugins[hash];
            string pl_title = "unknown";
            if (plData != null) pl_title = plData.Title;

            if(plData.Updater == null)
            {
                DebugHud.Log("The plugin \"{0}\" has an invalid updater instance, it's update method might not have been specified by the author!", pl_title);
                return;
            }

            string local_file = String.Format("{0}\\..\\plugins\\{1}", UnityEngine.Application.dataPath, plData.Filename);
            StartCoroutine(plData.Updater.Download(url, local_file, (string ContentType) =>
           {
               if (ContentType.StartsWith("application/")) return true;//yea it's binary file data

               DebugHud.Log("The download url for the plugin \"{0}\" returns content of type \"{1}\" rather than the plugin file itself.\nThis may indicate that the url for this plugin leads to a download PAGE as opposed to giving the actual file, the plugin creator should supply a valid url leading DIRECTLY to the file.", pl_title, ContentType);
               return false;//This file is not ok to download.
           },
           (int read, int total) =>
           {
               Plugin_StoreItem pl = this.list[hash] as Plugin_StoreItem;
               if (pl != null)
               {
                   pl.progress_bar.progress = ((float)read / (float)total);
               }
            },
           (string filename) =>
           {
               Loader.Add_Plugin_To_List(filename);
               Select_Plugin(selected_plugin);
           }));
        }
    }
}
