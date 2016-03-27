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
        }

        private void TrySpawnPluginPanel()
        {
            MainMenu.plugin_manager = uiControl.Create<PluginManager>();
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
                    //MenuUI.GetComponent<RectTransform>().sizeDelta = new Vector2(menuSize.sizeDelta.x, menuSize.sizeDelta.y + 50f);
                    MenuUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                    MenuUI.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);

                    PluginsButton.transform.localPosition = new Vector2(btnPos.localPosition.x, (btnPos.localPosition.y + btnSize.rect.height));

                    // Set the plugins button text.
                    PluginsButton.name = "PluginsMenu";
                    PluginsButton.GetComponentInChildren<Text>().text = "Plugins";

                    //Setup our buttons click logic
                    var btn = PluginsButton.GetComponent<Button>();
                    /*
                    ColorBlock clr = PluginsButton.GetComponent<Button>().colors;
                    clr.normalColor = blue_clr;
                    btn.image.color = blue_clr;
                    PluginsButton.GetComponent<Button>().colors = clr;
                    */

                    btn.onClick.RemoveAllListeners();
                    btn.onClick.SetPersistentListenerState(0, UnityEventCallState.Off);
                    btn.onClick.AddListener(new UnityAction(this.OnClick));


                    MainMenu.mainmenu = UnityEngine.Object.FindObjectOfType<MainMenuUI>().gameObject;
                }
            }
        }

        private void OnClick()
        {
            MainMenu._active = false;
            MainMenu.mainmenu = UnityEngine.Object.FindObjectOfType<MainMenuUI>().gameObject;
            MainMenu.mainmenu.SetActive(false);
            //MainMenu.plugins_panel.active = true;
            MainMenu.plugin_manager.Show();
        }

        private void OnGUI()
        {
            if (MainMenu.mainmenu == null) return;
        }
    }
}
