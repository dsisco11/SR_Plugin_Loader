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
        public static PluginManager plugin_manager = null;
        public static PluginStore plugin_store = null;
        private static bool _active = false;
        public static bool Active { get { return MainMenu._active; } }
        private static GUIContent title_content = null;

        private static GUIStyle title_style = null;
        private static Color blue_clr = new Color32(55, 149, 237, 255);//Blue color from our old Killstreaks.tf TTT server
        private static Color clr_gold_light = new Color32(250, 194, 73, 255);
        private static Color clr_gold = new Color32(206, 124, 37, 255);
        private static Color clr_brown = new Color32(40, 12, 0, 255);



        private void Awake()
        {
            title_content = new GUIContent(Loader.NAME);
            this.TrySpawn_PluginPanel();
            this.TrySpawn_PluginStore();
        }
        
        private void Update()
        {
            if (Levels.isSpecial(Application.loadedLevelName))
            {
                if (MainMenu.mainmenu == null)
                {
                    this.Extend_MainMenu();
                }
            }
        }

        private void OnLevelWasLoaded(int lvl)
        {
            MainMenu.mainmenu = null;
        }

        private void TrySpawn_PluginPanel()
        {
            if (MainMenu.plugin_manager != null) return;
            MainMenu.plugin_manager = uiControl.Create<PluginManager>();
            MainMenu.plugin_manager.onClosed += (uiWindow w) => { this.Show_MainMenu(); };
            MainMenu.plugin_manager.onShown += (uiWindow w) => { this.Hide_MainMenu(); };
        }

        private void TrySpawn_PluginStore()
        {
            if (MainMenu.plugin_store != null) return;
            MainMenu.plugin_store = uiControl.Create<PluginStore>();
            MainMenu.plugin_store.onClosed += (uiWindow w)=> { this.Show_MainMenu(); };
            MainMenu.plugin_store.onShown += (uiWindow w) => { this.Hide_MainMenu(); };
        }

        private void Extend_MainMenu()
        {
            if(MainMenu.mainmenu == null) MainMenu.mainmenu = UnityEngine.Object.FindObjectOfType<MainMenuUI>().gameObject;
            Add_Button("Plugins", "PluginsMenu", new UnityAction(this.Show_PluginManager));
            Add_Button("Plugin Store", "PluginStore", new UnityAction(this.Show_PluginStore), clr_gold_light, clr_gold, clr_brown);
        }

        private void Add_Button(string text, string name, UnityAction onclick_handler, Color? color=null, Color? hl_clr=null, Color? text_clr=null)
        {
            var menu = UnityEngine.Object.FindObjectOfType<MainMenuUI>();
            if (Levels.isSpecial(Application.loadedLevelName) && menu != null)
            {
                Transform MenuUI = menu.transform.GetChild(0);
                if (MenuUI.FindChild(name) == null)
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
                    GameObject newButton = UnityEngine.Object.Instantiate<GameObject>(btnPos.gameObject);
                    newButton.transform.SetParent(MenuUI);
                    // Add more height to the main menu UI panel so we can fit our fancy button in!
                    RectTransform menuSize = MenuUI.GetComponent<RectTransform>();
                    MenuUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                    MenuUI.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);

                    newButton.transform.localPosition = new Vector2(btnPos.localPosition.x, (btnPos.localPosition.y + btnSize.rect.height));

                    // Set the plugins button text.
                    newButton.name = name;
                    newButton.GetComponentInChildren<Text>().text = text;

                    //Setup our buttons click logic
                    Button btn = newButton.GetComponent<Button>();
                    
                    btn.onClick.RemoveAllListeners();
                    btn.onClick.SetPersistentListenerState(0, UnityEventCallState.Off);
                    btn.onClick.AddListener(onclick_handler);
                    
                    
                    var styler = newButton.AddComponent<ButtonStyler_Overrider>();
                    if (color.HasValue) styler.normalColor = color.Value;
                    if (hl_clr.HasValue) { styler.highlightedColor = hl_clr.Value; styler.pressedColor = hl_clr.Value; }
                    if(text_clr.HasValue) styler.normalText = text_clr.Value;
                }
            }
        }

        private void Show_MainMenu()
        {
            //MainMenu._active = true;
            MainMenu.mainmenu.SetActive(true);
            //MainMenu.mainmenu = UnityEngine.Object.FindObjectOfType<MainMenuUI>().gameObject;
        }

        private void Hide_MainMenu()
        {
            //MainMenu._active = false;
            MainMenu.mainmenu.SetActive(false);
            //MainMenu.mainmenu = UnityEngine.Object.FindObjectOfType<MainMenuUI>().gameObject;
        }

        private void Show_PluginManager()
        {
            MainMenu.plugin_manager.Show();
        }

        private void Show_PluginStore()
        {
            MainMenu.plugin_store.Show();
        }

        private void OnGUI()
        {
            if (MainMenu.mainmenu == null) return;
        }
    }
}
