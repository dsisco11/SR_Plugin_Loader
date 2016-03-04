using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace SR_PluginLoader
{
    class MainMenu : MonoBehaviour
    {
        public static GameObject mainmenu = null;
        private static GameObject plugins_panel_root = null;
        public static PluginsPanel plugins_panel = null;
        public static PluginsDownloadPanel plugins_download_panel = null;
        private static bool _active = false;
        public static bool Active { get { return MainMenu._active; } }
        private static GUIContent title_content = null;

        private static GUIStyle title_style = null;
        private static Color blue_clr = new Color(255f / 55f, 255f / 149f, 255f / 237f);//Blue color from our old Killstreaks.tf TTT server


        private void Awake()
        {
            title_content = new GUIContent(Loader.NAME);
            this.TrySpawnPluginPanel();
        }

        private void Start()
        {
            this.TrySpawnPluginMenu();
        }

        private void Update()
        {
            if (Levels.isSpecial(Application.loadedLevelName))
            {
                if (MainMenu.mainmenu == null)
                {
                    this.TrySpawnPluginMenu();
                }
            }
        }

        private void OnLevelWasLoaded(int lvl)
        {
            MainMenu.mainmenu = null;
            return;

            if(Levels.isSpecial(Application.loadedLevelName))
            {
                StartCoroutine(CheckForPluginUpdates());
            }
        }

        private IEnumerator CheckForPluginUpdates()
        {
            int updates = 0;
            foreach(Plugin plugin in Loader.plugins.Values)
            {
                if(plugin.data.UPDATEURL != null)
                {
                    WWW update = new WWW(plugin.data.UPDATEURL);

                    while (!update.isDone)
                        yield return new WaitForSeconds(0.1f);

                    JSONNode data = JSON.Parse(update.text);
                    if(data != null && data["plugin_hash"] != null && data["plugin_version"] != null && data["plugin_download"] != null)
                    {
                        if(String.Compare(plugin.DLL_Hash, data["plugin_hash"])!=0 || String.Compare(plugin.data.VERSION.ToString(), data["plugin_version"])!=0)
                        {
                            WWW download = new WWW(data["plugin_download"]);

                            while (!download.isDone)
                                yield return new WaitForSeconds(0.1f);

                            if (download.bytes.Length > 0)
                            {
                                string file = plugin.file;
                                string new_file = String.Format("{0}.tmp", file);
                                string old_file = String.Format("{0}.old", file);

                                File.WriteAllBytes(new_file, download.bytes);
                                if (File.Exists(old_file)) File.Delete(old_file);
                                File.Replace(new_file, file, old_file);
                                updates++;
                            }
                        }
                    }
                }
            }

            if(updates > 0)
            {
                new UI_Notification()
                {
                    msg = updates + " Plugins have been Updated.\nClick this box to restart!",
                    title = updates + " Plugins Updated",
                    onClick = delegate () { Loader.Restart_App(); }
                };
            }
        }

        private void TrySpawnPluginPanel()
        {
            MainMenu.plugins_panel_root = new GameObject("PluginsPanel");
            MainMenu.plugins_panel = MainMenu.plugins_panel_root.AddComponent<PluginsPanel>();
            //MainMenu.plugins_download_panel = MainMenu.plugins_panel_root.AddComponent<PluginsDownloadPanel>();
            UnityEngine.Object.DontDestroyOnLoad(MainMenu.plugins_panel_root);
        }

        private void TrySpawnPluginMenu()
        {
            var menu = UnityEngine.Object.FindObjectOfType<MainMenuUI>();
            //check if we are at the main menu and if we already have a gameobject instance with a MainMenuUI attached.
            if (Levels.isSpecial(Application.loadedLevelName) && menu != null)
            {
                Transform MenuUI = menu.transform.GetChild(0);
                //does the main menu already have a plugin button?
                if (MenuUI.FindChild("PluginsMenu") == null)
                {
                    //push all the buttons (excluding the play button) down by their own height, to make room for OUR button
                    for (int i = 1; i < MenuUI.childCount; i++)
                    {
                        var child = MenuUI.GetChild(i);

                        var rTrans = child.GetComponent<RectTransform>();
                        var height = rTrans.rect.height;

                        Vector2 newPos = (rTrans.anchoredPosition + new Vector2(0f, height));
                        child.GetComponent<RectTransform>().anchoredPosition.Set(newPos.x, newPos.y);
                    }

                    // Locate the play button
                    Transform btnPos = MenuUI.FindChild("PlayButton");
                    RectTransform btnSize = btnPos.GetComponent<RectTransform>();

                    // Create a copy of the play button that we can alter to do our bidding. (We want to make a copy so we don't need to re-apply all the same styling and whatnot, thus ensuring it won't break in the future)
                    GameObject PluginsButton = UnityEngine.Object.Instantiate<GameObject>(btnPos.gameObject);
                    PluginsButton.transform.SetParent(MenuUI);
                    // Add more height to the main menu UI panel so we can fit our fancy button in!
                    RectTransform menuSize = MenuUI.GetComponent<RectTransform>();
                    MenuUI.GetComponent<RectTransform>().sizeDelta = new Vector2(menuSize.sizeDelta.x, menuSize.sizeDelta.y + 50f);

                    PluginsButton.transform.localPosition = new Vector2(btnPos.localPosition.x, (btnPos.localPosition.y + btnSize.rect.height));

                    // Set the plugins button text.
                    PluginsButton.name = "PluginsMenu";
                    PluginsButton.GetComponentInChildren<Text>().text = "Plugins";

                    //Setup our buttons click logic
                    var btn = PluginsButton.GetComponent<Button>();
                    btn.onClick.RemoveAllListeners();
                    btn.onClick.SetPersistentListenerState(0, UnityEventCallState.Off);
                    btn.onClick.AddListener(new UnityAction(this.OnClick));


                    MainMenu.mainmenu = UnityEngine.Object.FindObjectOfType<MainMenuUI>().gameObject;
                }
            }
        }

        private void OnClick()
        {
            MainMenu._active = true;
            MainMenu.mainmenu = UnityEngine.Object.FindObjectOfType<MainMenuUI>().gameObject;
            MainMenu.mainmenu.SetActive(false);
            MainMenu.plugins_panel.active = true;
        }

        private void OnGUI()
        {
            if (MainMenu.mainmenu == null) return;

            MainMenu.Render_Menu_Splash();
        }

        private static void Render_Menu_Splash()
        {
            if (MainMenu.title_style == null)
            {
                MainMenu.title_style = new GUIStyle(GUI.skin.GetStyle("label"));
                MainMenu.title_style.fontSize = 16;
                MainMenu.title_style.fontStyle = FontStyle.Bold;
                //MainMenu.title_style.normal.textColor = Color.white;
                //MainMenu.title_style.normal.textColor = this.blue_clr;
                MainMenu.title_style.padding = new RectOffset(3, 3, 3, 3);
                MainMenu.title_style.normal.background = null;
            }
            
            float hw = (Screen.width / 2f);
            var txtSZ = MainMenu.title_style.CalcSize(title_content);
            float X = 5f;// (hw - (txtSZ.x / 2f));
            //float Y = (Screen.height - 95f);
            float Y = (Screen.height - 25f);

            float pad = 3f;
            float pad2 = (pad * 2.0f);
            Rect pos = new Rect(X, Y, 300f, 25f);

            GUI.Label(pos, title_content, MainMenu.title_style);
        }
    }
}
