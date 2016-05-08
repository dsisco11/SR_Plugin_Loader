using SR_PluginLoader;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

namespace SR_Plugin_Namespace
{

    // Please note that this project references assemblies located in the SlimeRancher game folder, specifically the default location for the steam version on windows.
    // If you are getting missing assembly reference errors then you need to correct the locations for those files.
    public static class SR_Plugin
    {
        // https://github.com/dsisco11/SR_Plugin_Loader/wiki/Specifying-Plugin-Data
        public static Plugin_Data PLUGIN_INFO = new Plugin_Data()
        {
            NAME = "GardenMastery",
            AUTHOR = "Sisco++",
            DESCRIPTION = @"Adds a new upgrade for garden plots that, when purchased, will place a seed bag next to the garden input terminal.
Firing a plantable food item into the bag will cause it to be automatically planted in the garden when the current crop is finished!",
            VERSION = new Plugin_Version(0, 1),
            UPDATE_METHOD = new Update_Method("https://github.com/dsisco11/SR_Plugin_Loader/raw/master/Plugins/GardenMastery.dll", UPDATER_TYPE.GIT),
            DEPENDENCIES = new List<Plugin_Dependency>() { },
            ICON = "sack_o_seeds_icon.png",
            PREVIEW = "sack_o_seeds_preview.png"
        };

        public static void Load(Plugin plugin, GameObject go)
        {
            go.AddComponent<GardenMastery.GardenMastery>();
            Init_Upgrades(plugin);
        }

        public static void Unload(Plugin plugin, GameObject go)
        {
            // When unloading the plugin let's go ahead and also unload any hooks that might have been registered under it's name.
            SiscosHooks.unregister_all(plugin);
        }

        public static PlotUpgrade SACK_O_SEEDS_UPGRADE = null;


        internal static void Init_Upgrades(Plugin plugin)
        {
            SACK_O_SEEDS_UPGRADE = new PlotUpgrade(plugin,
                    LandPlot.Id.GARDEN,
                    "sack_o_seeds",
                    5,
                    "Sack'o'Seeds",
                    @"
    Adds a seed bag to your garden.
    Shoot a plantable item into the bag and it will be planted in the garden once the current crop expires.",
                    (GameObject obj) =>
                    {
                        GameObject sack_prefab = ModelHelper.Get_Prefab("sack_o_seeds");
                        if (sack_prefab == null)
                        {
                            Stream stream = Util.Get_Resource_Stream("sack_o_seeds.obj", "GardenMastery");
                            if (stream == null)
                            {
                                DebugHud.Log("Cannot find resource file!");
                            }
                            else
                            {
                                sack_prefab = ModelHelper.Create_Model_Prefab("sack_o_seeds", stream, resolve_model_mats, new Model_Prefab_Transform[] { Model_Prefab_Transform.Flip_Z });
                                if (sack_prefab == null) throw new ArgumentNullException("Cannot to load \"Sack'O'Seeds\" model!");

                                Transform trans = sack_prefab.transform.Find("PHYS_0_Trigger");
                                if (trans == null) throw new ArgumentNullException("Cannot find \"Physics_Trigger\" group!");
                                GameObject trigger = trans.gameObject;
                                Collider col = trigger.GetComponent<Collider>();
                                col.isTrigger = true;
                                col.enabled = true;
                            }
                        }

                        if (sack_prefab != null)
                        {
                            Vector3 spawn_pos = obj.transform.TransformPoint(new Vector3(-5.5f, 0f, -1.25f));
                            GameObject sack = (GameObject)GameObject.Instantiate(sack_prefab, spawn_pos, obj.transform.rotation);
                            sack.transform.Rotate(Vector3.up, 180f);

                            if (sack == null) throw new Exception("Failed to spawn model.");

                            sack.SetActive(true);
                            Rigidbody rb = sack.GetComponent<Rigidbody>();
                            rb.constraints = RigidbodyConstraints.FreezeAll;
                            rb.isKinematic = true;

                            var script = sack.AddComponent<GardenMastery.SackOSeeds>();
                            script.Set_Plot(obj.GetComponent<LandPlot>(), SACK_O_SEEDS_UPGRADE);
                        }
                    },
                    (LandPlot plot) =>
                    {
                        PlotID pid = new PlotID(plot);
                        GardenMastery.SackOSeeds sack = null;
                        foreach (var obj in GardenMastery.SackOSeeds.ALL)
                        {
                            if (obj.ID == pid)
                            {
                                sack = obj;
                                break;
                            }
                        }

                        if (sack == null) throw new Exception("Unable to remove SackOSeeds upgrade for plot " + pid + ". Cannot find upgrade instance!");
                        GameObject.Destroy(sack);
                    },
                    (Texture2D)TextureHelper.Load_From_Resource("sack_o_seeds_icon.png", "GardenMastery"),
                    (Texture2D)TextureHelper.Load_From_Resource("sack_o_seeds_preview.png", "GardenMastery")
                );
        }

        public static Stream resolve_model_mats(string fileName)
        {
            string file = Path.GetFileName(fileName);
            return Util.Get_Resource_Stream(file, "GardenMastery");
        }
}
}
