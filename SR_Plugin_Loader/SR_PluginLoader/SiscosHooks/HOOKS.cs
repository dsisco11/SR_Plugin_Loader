using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace SR_PluginLoader
{
    /// <summary>
    /// List of available hooks.
    /// Naming scheme is as follows: (Pre_/Post_)ClassName_Event
    /// ClassName may be shortened when appropriate to keep hook names intuitive.
    /// </summary>
    [DebuggerDisplay("Hook = {name}")]
    public class HOOK_ID
    {
#region BLAH BLAH
        protected int id = -1;
        private static int _idx = 0;
        private string name = null;
        public static int Count { get { return (_idx+1); } }
        
        private HOOK_ID()
        {
            this.id = ++_idx;
        }

        private HOOK_ID(int i)
        {
            this.id = i;
            if (i >= _idx) _idx = (i + 1);
        }

        private HOOK_ID(HOOK_ID i)
        {
            this.id = i.id;
            if (id >= _idx) _idx = (id + 1);
        }

        public bool Equals(HOOK_ID obj)
        {
            return (this.id == obj.id);
        }

        public bool Equals(int i)
        {
            return (this.id == i);
        }

        static public explicit operator int(HOOK_ID hook)
        {
            return hook.id;
        }
        
        static public implicit operator HOOK_ID(int i)
        {
            return new HOOK_ID(i);
        }

        // ugh, oh god is this awful.
        // Update: not so awful now that it caches the name...
        public override string ToString()
        {
            if (this.name != null) return this.name;
            
            Type type = typeof(HOOK_ID);
            //PropertyInfo[] properties = type.GetProperties(BindingFlags.Static | BindingFlags.Public);
            FieldInfo[] fields = type.GetFields(BindingFlags.Static | BindingFlags.Public);
            
            foreach (FieldInfo field in fields)
            {
                if (field.FieldType != typeof(HOOK_ID)) continue;
                HOOK_ID obj = (HOOK_ID)field.GetValue(this);

                HOOK_ID hk = (HOOK_ID)obj;
                if (hk.id != this.id) continue;
                this.name = field.Name;
                break;
            }

            return this.name;
        }
#endregion

        public static readonly HOOK_ID NONE = new HOOK_ID(0);

        #region VacPak Events
        public static readonly HOOK_ID VacPak_Think = new HOOK_ID();
        public static readonly HOOK_ID VacPak_Can_Capture = new HOOK_ID();
        public static readonly HOOK_ID VacPak_Capture = new HOOK_ID();
        public static readonly HOOK_ID VacPak_Consume = new HOOK_ID();
        #endregion

        #region Entity Events
        public static readonly HOOK_ID Pre_Entity_Spawn = new HOOK_ID();
        public static readonly HOOK_ID EntitySpawner_Init = new HOOK_ID();

        public static readonly HOOK_ID Spawned_Entity = new HOOK_ID();
        public static readonly HOOK_ID Spawned_Slime = new HOOK_ID();
        public static readonly HOOK_ID Spawned_Animal = new HOOK_ID();
        public static readonly HOOK_ID Spawned_Food = new HOOK_ID();

        [ObsoleteAttribute("This hook name is obsolete, use Spawned_Entity instead.", false)]
        public static readonly HOOK_ID Entity_Spawned = new HOOK_ID(Spawned_Entity);
        [ObsoleteAttribute("This hook name is obsolete, use Spawned_Slime instead.", false)]
        public static readonly HOOK_ID Slime_Spawned = new HOOK_ID(Spawned_Slime);
        [ObsoleteAttribute("This hook name is obsolete, use Spawned_Animal instead.", false)]
        public static readonly HOOK_ID Animal_Spawned = new HOOK_ID(Spawned_Animal);
        [ObsoleteAttribute("This hook name is obsolete, use Spawned_Food instead.", false)]
        public static readonly HOOK_ID Food_Spawned = new HOOK_ID(Spawned_Food);

        public static readonly HOOK_ID Spawned_Land_Plot = new HOOK_ID();
        public static readonly HOOK_ID Spawned_Garden_Patch = new HOOK_ID();

        #endregion

        public static readonly HOOK_ID Pre_Economy_Init = new HOOK_ID();
        public static readonly HOOK_ID Post_Economy_Init = new HOOK_ID();

        #region Player Events
        public static readonly HOOK_ID Player_ApplyUpgrade = new HOOK_ID();
        public static readonly HOOK_ID Player_CanBuyUpgrade = new HOOK_ID();
        public static readonly HOOK_ID Player_Damaged = new HOOK_ID();
        public static readonly HOOK_ID Player_LoseEnergy = new HOOK_ID();
        public static readonly HOOK_ID Player_SetEnergy = new HOOK_ID();
        public static readonly HOOK_ID Player_MoneyChanged = new HOOK_ID();
        public static readonly HOOK_ID Player_AddRads = new HOOK_ID();
        public static readonly HOOK_ID Player_Death = new HOOK_ID();

        public static readonly HOOK_ID Pre_Player_Sleep = new HOOK_ID();
        public static readonly HOOK_ID Post_Player_Sleep = new HOOK_ID();
        #endregion

        public static readonly HOOK_ID Pre_Region_Spawn_Cycle = new HOOK_ID();
        public static readonly HOOK_ID Post_Region_Spawn_Cycle = new HOOK_ID();

        public static readonly HOOK_ID Get_Available_Saves = new HOOK_ID();
        public static readonly HOOK_ID Get_Save_Directory = new HOOK_ID();

        public static readonly HOOK_ID Ext_Game_Saved = new HOOK_ID();
        public static readonly HOOK_ID Game_Saved = new HOOK_ID();

        public static readonly HOOK_ID Pre_Game_Loaded = new HOOK_ID();
        public static readonly HOOK_ID Ext_Pre_Game_Loaded = new HOOK_ID();

        public static readonly HOOK_ID Post_Game_Loaded = new HOOK_ID();
        public static readonly HOOK_ID Ext_Post_Game_Loaded = new HOOK_ID();


        public static readonly HOOK_ID Plot_Load_Upgrades = new HOOK_ID();
        public static readonly HOOK_ID Spawn_Plot_Upgrades_UI = new HOOK_ID();
        public static readonly HOOK_ID Ext_Spawn_Plot_Upgrades_UI = new HOOK_ID();

        public static readonly HOOK_ID Spawn_Player_Upgrades_UI = new HOOK_ID();
        
        #region Silo Events
        public static readonly HOOK_ID Pre_Silo_Input = new HOOK_ID();
        public static readonly HOOK_ID Post_Silo_Input = new HOOK_ID();

        public static readonly HOOK_ID Pre_Silo_Output = new HOOK_ID();
        public static readonly HOOK_ID Post_Silo_Output = new HOOK_ID();
        #endregion

        public static readonly HOOK_ID ResourcePatch_Init = new HOOK_ID();

        #region Garden Events
        public static readonly HOOK_ID Pre_Garden_Init = new HOOK_ID();
        public static readonly HOOK_ID Post_Garden_Init = new HOOK_ID();

        public static readonly HOOK_ID Garden_Got_Input = new HOOK_ID();
        public static readonly HOOK_ID Pre_Garden_Set_Type = new HOOK_ID();
        public static readonly HOOK_ID Post_Garden_Set_Type = new HOOK_ID();
        #endregion

        #region Script-Handled Events
        // The following are all script-handled events.
        // Which is to say that they aren't fired via code injected into the game itself but rather from custom MonoBehaviour scripts created by the loader at runtime.
        public static readonly HOOK_ID Level_Loaded = new HOOK_ID();
        public static readonly HOOK_ID MainMenu_Loaded = new HOOK_ID();
        #endregion

    }

}