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


        public DevMenu() : base()
        {
            Title = "Developer Tools";
            Set_Size(800, 600);
            Center();
            onLayout += DevMenu_onLayout;

            list = uiControl.Create<uiListView>(this);
            list.alignTop();
            list.alignLeftSide();

            tabPanel = uiControl.Create<uiTabPanel>(this);
            tabPanel.Autosize_Method = AutosizeMethod.FILL;
            //tabPanel.local_style.normal.background = null;

            foreach (var kvp in Menus)
            {
                string eStr = Enum.GetName(typeof(Dev_Menu_Type), kvp.Key);
                var tab = tabPanel.Add_Tab();
                Menus[kvp.Key].Tab = tab;
                Menus[kvp.Key].Type = kvp.Key;

                switch (kvp.Key)
                {
                    case Dev_Menu_Type.SPAWN:
                        //Create_Spawn_Panel(tab);
                        break;
                    default:
                        DebugHud.Log("Unhandled Dev_Menu type: {0}", eStr);
                        break;
                }
                
                var itm = uiControl.Create<uiListItem>();
                itm.Title = kvp.Value.Title;
                itm.Description = kvp.Value.Description;
                itm.onSelected += (uiControl c) => { this.Set_Active_Menu(kvp.Value.Type); };
                list.Add(itm);
            }
        }

        private void DevMenu_onLayout(uiPanel c)
        {
            list.Set_Width(200f);
            list.FloodY();

            tabPanel.alignTop();
            tabPanel.moveRightOf(list, 4f);
        }

        private void Set_Active_Menu(Dev_Menu_Type newMenu)
        {
            DebugHud.Log("Set_Active_Menu: {0}", Enum.GetName(typeof(Dev_Menu_Type), newMenu));
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
            uiListView cat_list = uiControl.Create<uiListView>(tab);
            tab.onLayout += (uiPanel p) =>
            {
                cat_list.alignTop();
                cat_list.alignLeftSide();
                cat_list.FloodY();
                cat_list.Set_Width(150f);
            };

            Spawn_Category_Panels_Container cat_panel = uiControl.Create<Spawn_Category_Panels_Container>(tab);
            tab.onLayout += (uiPanel p) =>
            {
                cat_panel.moveRightOf(cat_list, 2f);
                cat_panel.FloodXY();
            };
            foreach (SpawnCategory cty in Enum.GetValues(typeof(SpawnCategory)))
            {
                if (cty == SpawnCategory.NONE) continue;
                string catStr = Enum.GetName(typeof(SpawnCategory), cty);
                var cat = uiControl.Create<uiListItem>(String.Concat("category_", catStr.ToLower()), cat_list);
                cat.Title = catStr.ToLower().CapitalizeFirst();
                cat.onSelected += (uiControl c) => { cat_panel.Category = cty; };

                /*
                    switch(cty)
                    {
                        case SpawnCategory.SLIMES:
                            Create_Spawn_Category_Menu(tab, cat_panel, cty, Ident.ALL_SLIMES);
                            break;
                        case SpawnCategory.PLORTS:
                            Create_Spawn_Category_Menu(tab, cat_panel, cty, Identifiable.PLORT_CLASS);
                            break;
                        case SpawnCategory.ANIMALS:
                            Create_Spawn_Category_Menu(tab, cat_panel, cty, Ident.ALL_ANIMALS);
                            break;
                        case SpawnCategory.FRUITS:
                            Create_Spawn_Category_Menu(tab, cat_panel, cty, Identifiable.FRUIT_CLASS);
                            break;
                        case SpawnCategory.VEGETABLES:
                            Create_Spawn_Category_Menu(tab, cat_panel, cty, Identifiable.VEGGIE_CLASS);
                            break;

                        default:
                            DebugHud.Log("Unhandled Spawn menu category: {0}", catStr);
                            break;
                    }
                    */
            }

            //cat_list.Category = SpawnCategory.SLIMES;
        }

        private void Dev_Spawn_Item(Identifiable.Id ID)
        {
            if (Game.atMainMenu)
            {
                Sound.Play(SoundId.ERROR);
                DebugHud.Log("Failed to spawn item: {0}, We are at the main menu.", ID);
                return;
            }

            RaycastHit? ray = Player.Raycast();
            if (!ray.HasValue)
            {
                Sound.Play(SoundId.ERROR);
                DebugHud.Log("Failed to spawn item: {0}, Unable to perform raycast from player's view. Perhaps the ray distance is too far.", ID);
            }

            if (Util.TrySpawn(ID, ray.Value) == null)
            {
                Sound.Play(SoundId.ERROR);
                DebugHud.Log("Failed to spawn item: {0}, An unknown error occured", ID);
            }
            else Sound.Play(SoundId.POSITIVE);
        }
        
        private void Create_Spawn_Category_Menu(uiPanel panel, Spawn_Category_Panels_Container container, SpawnCategory cty, HashSet<Identifiable.Id> ITEMS)
        {
            // This list will just have a bunch of pictures of slimes that can be spawned
            string catStr = Enum.GetName(typeof(SpawnCategory), cty);
            float ICON_SIZE = 100f;
            uiListView menu = null;
            menu = uiControl.Create<uiListView>(catStr, container);
            menu.Layout = new Layout_IconList(ICON_SIZE);
            menu.isVisible = false;// Hidden by default
            menu.alignTop();
            menu.alignLeftSide();

            container.onLayout += (uiPanel p) =>
            {
                menu.FloodXY();
            };


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
                    DebugHud.LogSilent(ex);
                }

                if (sprite == null) continue;// Exclude anything without an icon out of respect for the devs, we will just assume things without an icon aren't in the game just yet I suppose...

                var itm = uiControl.Create<uiListIcon>(menu);
                if (sprite != null) itm.Icon = sprite.texture;
                else itm.Icon = TextureHelper.icon_unknown;

                itm.Title = Language.Translate(ID);
                itm.Set_Size(ICON_SIZE, ICON_SIZE);
                itm.onClicked += (uiControl c) => { Dev_Spawn_Item(ID); };
                itm.Selectable = false;
            }
        }
    }


    #region Spawner Menu
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

    public class Spawn_Category_Panels_Container : uiPanel
    {
        private SpawnCategory _category = SpawnCategory.NONE;
        public SpawnCategory Category { get { return _category; } set { var old = _category; _category = value; change_categorys(old, _category); } }


        public Spawn_Category_Panels_Container() { }

        private void change_categorys(SpawnCategory old, SpawnCategory curr)
        {
            if (old != SpawnCategory.NONE)
            {
                string oCatStr = Enum.GetName(typeof(SpawnCategory), old);
                var o = this[oCatStr];
                if (o == null) DebugHud.Log("Cannot find control named: {0}", oCatStr);
                else o.isVisible = false;
            }
            
            string nCatStr = Enum.GetName(typeof(SpawnCategory), curr);
            var n = this[nCatStr];
            if (n == null) DebugHud.Log("Cannot find control named: {0}", nCatStr);
            else n.isVisible = true;
        }
    }
    #endregion
}
