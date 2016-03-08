using System;
using System.Collections.Generic;
using System.Text;

namespace SR_PluginLoader
{
    /*
    THIS IS FOR INTERNAL USE BY THE LIBRARY ONLY
    I WILL NOT BE DOCUMENTING THIS AREA
    */

    public class Hook_Dbg_Data
    {
        public HOOK_ID hook = HOOK_ID.NONE;
        public int id { get { return (int)hook; } set { this.hook = (HOOK_ID)value; } }
        public string method = null;
        public int offset = 0;

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
            new Hook_Dbg_Data() { hook = HOOK_ID.VacPak_Consume, method = "WeaponVacuum.ConsumeVacItem" },
            new Hook_Dbg_Data() { hook = HOOK_ID.VacPak_Can_Capture, method = "Vacuumable.canCapture" },
            new Hook_Dbg_Data() { hook = HOOK_ID.VacPak_Capture, method = "Vacuumable.capture" },
            new Hook_Dbg_Data() { hook = HOOK_ID.VacPak_Think, method = "WeaponVacuum.Update" },

            new Hook_Dbg_Data() { hook = HOOK_ID.Pre_Region_Update, method = "CellDirector.Update", offset = 0 },
            new Hook_Dbg_Data() { hook = HOOK_ID.Post_Region_Update, method = "CellDirector.Update", offset = -1 },
            new Hook_Dbg_Data() { hook = HOOK_ID.Spawn_Player_Upgrades_UI, method = "PersonalUpgradeUI.CreatePurchaseUI", offset=-1 },
            new Hook_Dbg_Data() { hook = HOOK_ID.Player_CanGetUpgrade, method = "PlayerState.CanGetUpgrade", offset=-1 },
            new Hook_Dbg_Data() { hook = HOOK_ID.Pre_Save_Game, method = "GameData.Save", offset = 0 },
            new Hook_Dbg_Data() { hook = HOOK_ID.Post_Save_Game, method = "GameData.Save", offset = -1 },
            new Hook_Dbg_Data() { hook = HOOK_ID.Pre_Load_Game, method = "GameData.Load", offset = 0 },
            new Hook_Dbg_Data() { hook = HOOK_ID.Post_Load_Game, method = "GameData.Load", offset = -1 },
            new Hook_Dbg_Data() { hook = HOOK_ID.Get_Available_Saves, method = "GameData.AvailableGames", offset = 0 },
            new Hook_Dbg_Data() { hook = HOOK_ID.Get_Save_Directory, method = "GameData.ToPath", offset = 0 },

            new Hook_Dbg_Data() { hook = HOOK_ID.Player_ApplyUpgrade, method="PlayerState.ApplyUpgrade", offset=-1 },
            new Hook_Dbg_Data() { hook = HOOK_ID.Player_Damaged, method = "PlayerState.Damage" },

            new Hook_Dbg_Data() { hook = HOOK_ID.Pre_Entity_Spawn, method = "DirectedActorSpawner.Spawn" },
            new Hook_Dbg_Data() { hook = HOOK_ID.EntitySpawner_Init, method = "DirectedActorSpawner.Start" },
            new Hook_Dbg_Data() { hook = HOOK_ID.Player_LoseEnergy, method = "PlayerState.SpendEnergy" },
            new Hook_Dbg_Data() { hook = HOOK_ID.Player_SetEnergy, method = "PlayerState.SetEnergy" },
            new Hook_Dbg_Data() { hook = HOOK_ID.Player_MoneyChanged, method = "PlayerState.AddCurrency" },

            new Hook_Dbg_Data() { hook = HOOK_ID.Pre_Economy_Init, method = "EconomyDirector.InitForLevel", offset=0 },
            new Hook_Dbg_Data() { hook = HOOK_ID.Post_Economy_Init, method = "EconomyDirector.InitForLevel", offset=-1 },

            new Hook_Dbg_Data() { hook = HOOK_ID.Pre_Silo_Input, method = "SiloCatcher.OnTriggerEnter", offset=0 },
            new Hook_Dbg_Data() { hook = HOOK_ID.Post_Silo_Input, method = "SiloCatcher.OnTriggerEnter", offset=-1 },

            new Hook_Dbg_Data() { hook = HOOK_ID.Pre_Silo_Output, method = "SiloCatcher.OnTriggerStay", offset=0 },
            new Hook_Dbg_Data() { hook = HOOK_ID.Post_Silo_Output, method = "SiloCatcher.OnTriggerStay", offset=-1 },
        };
    }
}
