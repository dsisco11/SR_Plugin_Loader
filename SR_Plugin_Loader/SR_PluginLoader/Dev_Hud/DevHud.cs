﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SR_PluginLoader
{

    class Dev_Menu
    {
        public string Title, Description;
        public uiScrollPanel Panel = null;
        public Dev_Menu_Type Type = Dev_Menu_Type.NONE;

        public Dev_Menu(string title, string desc)
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

    class DevHud : uiWindow
    {
        private uiListView list;
        private List<uiControl> panels = new List<uiControl>();
        private Dev_Menu_Type activeMenu = Dev_Menu_Type.NONE;
        private Dictionary<Dev_Menu_Type, Dev_Menu> Menus = new Dictionary<Dev_Menu_Type, Dev_Menu>()
        {
            { Dev_Menu_Type.SPAWN, new Dev_Menu("Spawn", "Pick and spawn items from a list.") },
        };


        public DevHud()
        {
            this.title = "Developer Tools";
            this.Set_Size(800, 600);
            this.Center();

            list = uiControl.Create<uiListView>(this);
            list.alignTop();
            list.alignLeftSide();

            
            foreach(var kvp in Menus)
            {
                string eStr = Enum.GetName(typeof(Dev_Menu_Type), kvp.Key);
                var panel = uiControl.Create<uiScrollPanel>(this);
                panel.visible = false;
                panel.local_style.normal.background = null;
                Menus[kvp.Key].Panel = panel;
                Menus[kvp.Key].Type = kvp.Key;

                switch (kvp.Key)
                {
                    case Dev_Menu_Type.SPAWN:
                        Create_Spawn_Panel(panel);
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

        private void Set_Active_Menu(Dev_Menu_Type newMenu)
        {
            DebugHud.Log("Set_Active_Menu: {0}", Enum.GetName(typeof(Dev_Menu_Type), newMenu));
            if (newMenu != activeMenu)
            {
                Dev_Menu_Type prevTy = activeMenu;
                activeMenu = newMenu;

                Dev_Menu menu = null;
                if (Menus.TryGetValue(newMenu, out menu)) menu.Panel.visible = true;

                Dev_Menu old = null;
                if (Menus.TryGetValue(prevTy, out old)) old.Panel.visible = false;

            }
        }

        public override void doLayout()
        {
            list.Set_Width(200f);
            list.floodY();


            foreach (var kvp in Menus)
            {
                uiControl panel = kvp.Value.Panel;
                panel.alignTop();
                panel.moveRightOf(list, 4f);
                panel.floodXY();
            }
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.BackQuote))// ` eg: ~
            {
                this.ToggleShow();
            }
        }


        private void Create_Spawn_Panel(uiPanel panel)
        {
            // This list will contain different categories of spawnable items
            uiListView cat_list = uiControl.Create<uiListView>(panel);
            panel.onLayout += (uiPanel p) =>
            {
                cat_list.alignTop();
                cat_list.alignLeftSide();
                cat_list.floodY();
                cat_list.Set_Width(150f);
            };

            Spawn_Category_Panels_Container cat_panel = uiControl.Create<Spawn_Category_Panels_Container>(panel);
            panel.onLayout += (uiPanel p) =>
            {
                cat_panel.moveRightOf(cat_list, 2f);
                cat_panel.floodXY();
            };

            foreach (SpawnCategory cty in Enum.GetValues(typeof(SpawnCategory)))
            {
                if (cty == SpawnCategory.NONE) continue;
                string catStr = Enum.GetName(typeof(SpawnCategory), cty);
                var cat = uiControl.Create<uiListItem>(String.Concat("category_", catStr.ToLower()), cat_list);
                cat.Title = catStr.ToLower().CapitalizeFirst();
                cat.onSelected += (uiControl c) => { cat_panel.Category = cty; };

                switch(cty)
                {
                    case SpawnCategory.SLIMES:
                        Create_Spawn_Category_Menu(panel, cat_panel, cty, Ident.ALL_SLIMES);
                        break;
                    case SpawnCategory.PLORTS:
                        Create_Spawn_Category_Menu(panel, cat_panel, cty, Identifiable.PLORT_CLASS);
                        break;
                    case SpawnCategory.ANIMALS:
                        Create_Spawn_Category_Menu(panel, cat_panel, cty, Ident.ALL_ANIMALS);
                        break;
                    case SpawnCategory.FRUITS:
                        Create_Spawn_Category_Menu(panel, cat_panel, cty, Identifiable.FRUIT_CLASS);
                        break;
                    case SpawnCategory.VEGETABLES:
                        Create_Spawn_Category_Menu(panel, cat_panel, cty, Identifiable.VEGGIE_CLASS);
                        break;
                        
                    default:
                        DebugHud.Log("Unhandled Spawn menu category: {0}", catStr);
                        break;
                }
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
            menu.visible = false;// Hidden by default
            menu.alignTop();
            menu.alignLeftSide();

            container.onLayout += (uiPanel p) =>
            {
                menu.floodXY();
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

                if (sprite == null) continue;// Exclude anything without an icon out of respect for the devs, we will just assume things without an icon aren't in the game just yet I suppose...

                var itm = uiControl.Create<uiListIcon>(menu);
                if (sprite != null) itm.Icon = sprite.texture;
                else itm.Icon = Loader.tex_unknown;

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
                else o.visible = false;
            }
            
            string nCatStr = Enum.GetName(typeof(SpawnCategory), curr);
            var n = this[nCatStr];
            if (n == null) DebugHud.Log("Cannot find control named: {0}", nCatStr);
            else n.visible = true;
        }
    }

    #endregion
}
