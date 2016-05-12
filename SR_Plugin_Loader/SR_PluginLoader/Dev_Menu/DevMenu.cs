using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SR_PluginLoader
{
    class Dev_MenuTab
    {
        public string Title, Description;
        public uiTab Tab = null;
        public Dev_Menu_Type Type = Dev_Menu_Type.NONE;

        public Dev_MenuTab(string title, string desc)
        {
            Title = title;
            Description = desc;
        }
    }
    
    public enum SpawnCategory
    {
        NONE = 0,
        SLIMES,
        PLORTS,
        ANIMALS,
        FRUITS,
        VEGETABLES,
        MISC
    }

    enum Dev_Menu_Type
    {
        NONE=0,
        SPAWN,
    }

    class DevMenu : uiWindow
    {
        private uiListView list;
        private uiTabPanel tabPanel = null;
        private List<uiControl> panels = new List<uiControl>();
        private Dev_Menu_Type activeMenu = Dev_Menu_Type.NONE;
        private Dictionary<Dev_Menu_Type, Dev_MenuTab> Menus = new Dictionary<Dev_Menu_Type, Dev_MenuTab>()
        {
            { Dev_Menu_Type.SPAWN, new Dev_MenuTab("Spawn", "Pick and spawn items from a list.") },
        };


        public DevMenu()
        {
            onLayout += DevMenu_onLayout;
            Title = "Developer Tools";
            Set_Size(800, 600);
            Center();

            list = uiControl.Create<uiListView>(this);
            list.alignTop();
            list.alignLeftSide();
            list.Set_Margin(0, 4, 0, 0);

            tabPanel = uiControl.Create<uiTabPanel>(this);
            tabPanel.Autosize_Method = AutosizeMethod.FILL;
            //tabPanel.local_style.normal.background = null;

            foreach (var kvp in Menus)
            {
                string eStr = Enum.GetName(typeof(Dev_Menu_Type), kvp.Key);
                var tab = tabPanel.Add_Tab();
                tab.Scrollable = false;
                Menus[kvp.Key].Tab = tab;
                Menus[kvp.Key].Type = kvp.Key;

                switch (kvp.Key)
                {
                    case Dev_Menu_Type.SPAWN:
                        Create_Spawn_Panel(tab);
                        break;
                    default:
                        SLog.Warn("Unhandled Dev_Menu type: {0}", eStr);
                        break;
                }
                
                var itm = uiControl.Create<uiListItem>();
                itm.Title = kvp.Value.Title;
                itm.Description = kvp.Value.Description;
                itm.onSelected += (uiControl c) => { Set_Active_Menu(kvp.Value.Type); };
                list.Add(itm);
            }
        }

        private void DevMenu_onLayout(uiPanel c)
        {
            list.Set_Width(200f);
            list.FloodY();

            tabPanel.alignTop();
            tabPanel.moveRightOf(list);
        }

        private void Set_Active_Menu(Dev_Menu_Type newMenu)
        {
            SLog.Info("Set_Active_Menu: {0}", Enum.GetName(typeof(Dev_Menu_Type), newMenu));
            activeMenu = newMenu;
            Dev_MenuTab menu = null;
            if (Menus.TryGetValue(newMenu, out menu)) menu.Tab.Select();
        }
        
        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.BackQuote))// ` eg: ~
            {
                ToggleShow();
            }
        }


        private void Create_Spawn_Panel(uiTab tab)
        {
            // This list will contain different categories of spawnable items
            uiListView cList = uiControl.Create<uiListView>(tab);
            cList.Set_Margin(0, 4, 0, 0);
            cList.Set_Width(130f);
            tab.onLayout += (uiPanel p) =>
            {
                cList.alignTop();
                cList.alignLeftSide();
                cList.FloodY();
            };
            
            uiTabPanel catPanel = uiControl.Create<uiTabPanel>(tab);
            tab.onLayout += (uiPanel p) =>
            {
                catPanel.moveRightOf(cList);
                catPanel.FloodXY();
            };

            foreach (SpawnCategory cty in Enum.GetValues(typeof(SpawnCategory)))
            {
                if (cty == SpawnCategory.NONE) continue;
                string catStr = Enum.GetName(typeof(SpawnCategory), cty);
                var catBtn = uiControl.Create<uiListItem>(String.Concat("category_", catStr.ToLower()), cList);
                catBtn.Title = catStr.ToLower().CapitalizeFirst();
                catBtn.Description = null;

                uiTab cTab = null;
                switch(cty)
                {
                    case SpawnCategory.SLIMES:
                        cTab = Create_Spawn_Category_Menu(tab, catPanel, cty, Ident.ALL_SLIMES);
                        break;
                    case SpawnCategory.PLORTS:
                        cTab = Create_Spawn_Category_Menu(tab, catPanel, cty, Identifiable.PLORT_CLASS);
                        break;
                    case SpawnCategory.ANIMALS:
                        cTab = Create_Spawn_Category_Menu(tab, catPanel, cty, Ident.ALL_ANIMALS);
                        break;
                    case SpawnCategory.FRUITS:
                        cTab = Create_Spawn_Category_Menu(tab, catPanel, cty, Identifiable.FRUIT_CLASS);
                        break;
                    case SpawnCategory.VEGETABLES:
                        cTab = Create_Spawn_Category_Menu(tab, catPanel, cty, Identifiable.VEGGIE_CLASS);
                        break;

                    default:
                        SLog.Info("Unhandled Spawn menu category: {0}", catStr);
                        break;
                }

                catBtn.onSelected += (uiControl c) => {
                    Sound.Play(SoundId.BTN_CLICK);
                    cTab.Select();
                };

            }
        }

        private void Dev_Spawn_Item(Identifiable.Id ID)
        {
            if (Game.atMainMenu)
            {
                Sound.Play(SoundId.ERROR);
                SLog.Info("Failed to spawn item: {0}, We are at the main menu.", ID);
                return;
            }

            RaycastHit? ray = Player.Raycast();
            if (!ray.HasValue)
            {
                Sound.Play(SoundId.ERROR);
                SLog.Info("Failed to spawn item: {0}, Unable to perform raycast from player's view. Perhaps the ray distance is too far.", ID);
            }

            if (Util.TrySpawn(ID, ray.Value) == null)
            {
                Sound.Play(SoundId.ERROR);
                SLog.Info("Failed to spawn item: {0}, An unknown error occured", ID);
            }
            else Sound.Play(SoundId.BTN_CLICK);
        }
        
        private uiTab Create_Spawn_Category_Menu(uiPanel panel, uiTabPanel container, SpawnCategory cty, HashSet<Identifiable.Id> ITEMS)
        {
            // This list will just have a bunch of pictures of slimes that can be spawned
            string catStr = Enum.GetName(typeof(SpawnCategory), cty);
            float ICON_SIZE = 100f;
            uiTab tab = container.Add_Tab(catStr);
            uiListView list = uiControl.Create<uiListView>(catStr, tab);
            list.Layout = new Layout_IconList(ICON_SIZE);
            list.Autosize_Method = AutosizeMethod.FILL;
            list.Autosize = true;


            foreach (Identifiable.Id ID in ITEMS)
            {
                Sprite sprite = null;
                try
                {
                    sprite = Directors.lookupDirector.GetIcon(ID);
                }
                catch(KeyNotFoundException)
                {
                    continue;
                }
                catch(Exception ex)
                {
                    SLog.Debug(ex);
                }

                if (sprite == null) continue;// Exclude anything without an icon out of respect for the devs, we will just assume things without an icon aren't in the game just yet I suppose...

                var itm = uiControl.Create<uiListIcon>(list);
                if (sprite != null) itm.Icon = sprite.texture;
                else itm.Icon = TextureHelper.icon_unknown;

                itm.Title = Language.Translate(ID);
                itm.Set_Size(ICON_SIZE, ICON_SIZE);
                itm.onClicked += (uiControl c) => { Dev_Spawn_Item(ID); };
                itm.Selectable = false;
            }

            return tab;
        }
    }
}
