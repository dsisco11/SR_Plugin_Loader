using System;
using System.Collections.Generic;
using System.Text;

namespace SR_PluginLoader
{
    /*
    THIS IS FOR INTERNAL USE BY THE LIBRARY ONLY
    I WILL NOT BE DOCUMENTING THIS AREA
    */
    public enum debug_positioning
    {
        Instruction = 0,
        OpCode,
        Branch_Start,
        Branch_Exit,
        Cond_Branch_Start,
        Cond_Branch_Exit,
        Field_Ref,
        Method_Ref
    }

    public class Hook_Dbg_Data
    {
        public HOOK_ID hook = HOOK_ID.NONE;
        public int id { get { return (int)hook; } set { this.hook = (HOOK_ID)value; } }
        public string name = null;
        public bool is_post = false;
        public debug_positioning method = debug_positioning.Instruction;
        public debug_positioning relative_method = debug_positioning.Instruction;
        public int pos = 0;
        public int relative = 0;
        public string arg = null;

        public Hook_Dbg_Data()
        {
        }
    }


    /// <summary>
    /// Contains a list of all hooks and their method translations for debugging purposes
    /// </summary>
    public static class HOOKS
    {
        public static Hook_Dbg_Data[] HooksList = new Hook_Dbg_Data[] {

            new Hook_Dbg_Data() { hook = HOOK_ID.VacPak_Consume, name = "WeaponVacuum.ConsumeVacItem" },
            new Hook_Dbg_Data() { hook = HOOK_ID.VacPak_Can_Capture, name = "Vacuumable.canCapture" },
            new Hook_Dbg_Data() { hook = HOOK_ID.VacPak_Capture, name = "Vacuumable.capture" },
            new Hook_Dbg_Data() { hook = HOOK_ID.VacPak_Think, name = "WeaponVacuum.Update" },

            new Hook_Dbg_Data() { hook = HOOK_ID.Pre_Save_Game, name = "GameData.Save", pos = 0 },
            new Hook_Dbg_Data() { hook = HOOK_ID.Post_Save_Game, name = "GameData.Save", pos = -1 },
            new Hook_Dbg_Data() { hook = HOOK_ID.Pre_Load_Game, name = "GameData.Load", pos = 0 },
            new Hook_Dbg_Data() { hook = HOOK_ID.Post_Load_Game, name = "GameData.Load", pos = -1 },
            new Hook_Dbg_Data() { hook = HOOK_ID.Get_Available_Saves, name = "GameData.AvailableGames", pos = 0 },
            new Hook_Dbg_Data() { hook = HOOK_ID.Get_Save_Directory, name = "GameData.ToPath", pos = 0 },

            new Hook_Dbg_Data() { hook = HOOK_ID.Pre_Entity_Spawn, name = "DirectedActorSpawner.Spawn" },
            new Hook_Dbg_Data() { hook = HOOK_ID.EntitySpawner_Init, name = "DirectedActorSpawner.Start" },
            
            new Hook_Dbg_Data() { hook = HOOK_ID.Player_CanGetUpgrade, name = "PlayerState.CanGetUpgrade", pos=-1 },
            new Hook_Dbg_Data() { hook = HOOK_ID.Player_ApplyUpgrade, name="PlayerState.ApplyUpgrade", pos=-1 },
            new Hook_Dbg_Data() { hook = HOOK_ID.Player_Damaged, name = "PlayerState.Damage" },
            new Hook_Dbg_Data() { hook = HOOK_ID.Player_LoseEnergy, name = "PlayerState.SpendEnergy" },
            new Hook_Dbg_Data() { hook = HOOK_ID.Player_SetEnergy, name = "PlayerState.SetEnergy" },
            new Hook_Dbg_Data() { hook = HOOK_ID.Player_MoneyChanged, name = "PlayerState.AddCurrency" },

            new Hook_Dbg_Data() { hook = HOOK_ID.Pre_Region_Spawn_Cycle, name = "CellDirector.Update", pos = 4, method = debug_positioning.Cond_Branch_Start },
            new Hook_Dbg_Data() { hook = HOOK_ID.Post_Region_Spawn_Cycle, name = "CellDirector.Update", pos = 4, method = debug_positioning.Cond_Branch_Exit, relative = -1 },

            new Hook_Dbg_Data() { hook = HOOK_ID.Spawn_Player_Upgrades_UI, name = "PersonalUpgradeUI.CreatePurchaseUI", pos=-1 },

            new Hook_Dbg_Data() { hook = HOOK_ID.Pre_Economy_Init, name = "EconomyDirector.InitForLevel", pos=0 },
            new Hook_Dbg_Data() { hook = HOOK_ID.Post_Economy_Init, name = "EconomyDirector.InitForLevel", pos=-1 },

            new Hook_Dbg_Data() { hook = HOOK_ID.Pre_Silo_Input, name = "SiloCatcher.OnTriggerEnter", pos=-1, method = debug_positioning.Cond_Branch_Start },
            new Hook_Dbg_Data() { hook = HOOK_ID.Post_Silo_Input, name = "SiloCatcher.OnTriggerEnter", pos=-1, is_post=true, method = debug_positioning.Cond_Branch_Exit, relative=-1 },

            new Hook_Dbg_Data() { hook = HOOK_ID.Pre_Silo_Output, name = "SiloCatcher.OnTriggerStay", pos=-1, method = debug_positioning.Cond_Branch_Start },
            new Hook_Dbg_Data() { hook = HOOK_ID.Post_Silo_Output, name = "SiloCatcher.OnTriggerStay", pos=-1, is_post=true, method = debug_positioning.Cond_Branch_Exit, relative=-1 },
        };
    }
}
