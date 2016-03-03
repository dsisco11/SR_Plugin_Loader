using System;
using System.Collections.Generic;
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

        private static GUIStyle title_style = null;
        private static Color blue_clr = new Color(255f / 55f, 255f / 149f, 255f / 237f);//Blue color from our old Killstreaks.tf TTT server


        public void Awake()
        {
            this.TrySpawnPluginPanel();
        }

        private void Update()
        {
            if(MainMenu.mainmenu == null)
            {
                this.TrySpawnPluginMenu();
            }
        }

        private void OnLevelWasLoaded(int lvl)
        {
            MainMenu.mainmenu = null;
        }

        private void TrySpawnPluginPanel()
        {
            MainMenu.plugins_panel_root = new GameObject("PluginsPanel");
            MainMenu.plugins_panel = MainMenu.plugins_panel_root.AddComponent<PluginsPanel>();
            MainMenu.plugins_download_panel = MainMenu.plugins_panel_root.AddComponent<PluginsDownloadPanel>();
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

            if (MainMenu.Active)
            {
            }
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

            string title = Loader.NAME;
            float hw = (Screen.width / 2f);
            var txtSZ = MainMenu.title_style.CalcSize(new GUIContent(title));
            float X = 5f;// (hw - (txtSZ.x / 2f));
            //float Y = (Screen.height - 95f);
            float Y = (Screen.height - 25f);

            float pad = 3f;
            float pad2 = (pad * 2.0f);
            Rect pos = new Rect(X, Y, 300f, 25f);

            //GUI.skin.label.normal.textColor = this.blue_clr;
            //GUI.Box(new Rect(pos.x - pad, pos.y - pad, pos.width + pad2, pos.height + pad2), "");
            //Color prev_clr = GUI.color;
            //GUI.color = this.blue_clr;
            //GUI.contentColor = this.blue_clr;
            GUI.Label(pos, title, MainMenu.title_style);
            //GUI.color = prev_clr;
            //GUI.contentColor = Color.white;
        }
    }
}
