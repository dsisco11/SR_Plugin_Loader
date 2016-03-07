namespace SR_PluginLoader
{
    /// <summary>
    /// List of available hooks.
    /// Naming scheme is as follows: (Pre_/Post_)ClassName_Event
    /// ClassName may be shortened when appropriate to keep hook names intuitive.
    /// </summary>
    public enum HOOK_ID
    {
        NONE = 0,
        VacPak_Think,
        Pre_Entity_Spawn,
        EntitySpawner_Init,
        Pre_Economy_Init,
        Post_Economy_Init,
        Player_ApplyUpgrade,
        Player_CanGetUpgrade,
        Player_Damaged,
        Player_LoseEnergy,
        Player_SetEnergy,
        Vac_Can_Capture,
        Vac_Capture,
        CellDirector_Pre_Update,
        CellDirector_Post_Update,
        Get_Available_Saves,
        Get_Save_Directory,
        Pre_Save_Game,
        Post_Save_Game,
        Pre_Load_Game,
        Post_Load_Game,
        Spawn_Player_Upgrades_UI
    }

}