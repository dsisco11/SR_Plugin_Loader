//#define USING_VANILLA // This turns off any references to methods that arent available when using a "vanilla" assembly-csharp file, eg: one that hasnt had the installer run on it.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SR_PluginLoader
{
    public static class Player
    {
        private static PlayerState player { get { return SRSingleton<GameContext>.Instance.PlayerState; } }
        private static GameObject pObj { get { return SRSingleton<GameContext>.Instance.Player; } }

        public static GameObject gameObject { get { return pObj; } }
        public static PlayerState state { get { return player; } }
        public static WeaponVacuum Weapon { get { return pObj.GetComponentInChildren<WeaponVacuum>(); } }
        public static EnergyJetpack Jetpack { get { return pObj.GetComponent<EnergyJetpack>(); } }

        /// <summary>
        /// Returns the number of a certain item that the player has in their inventory.
        /// </summary>
        /// <returns></returns>
        public static int Get_Inv_Item_Count(Identifiable.Id id)
        {
#if !USING_VANILLA
            for (int s = 0; s< player.Ammo.slotCount; s++)
            {
                Identifiable.Id sid = player.Ammo.GetSlotName(s);
                if(sid == id)
                {
                    return player.Ammo.GetSlotCount(s);
                }
            }
#endif
            return 0;
        }

        /// <summary>
        /// Returns an array of GameObject which are currently being sucked in by the players weapon.
        /// </summary>
        /// <returns></returns>
        public static List<Identifiable> Get_Captive_Items()
        {
            List<Identifiable> ret = new List<Identifiable>();
#if !USING_VANILLA
            foreach (Joint joint in Player.Weapon.Get_Joints())
            {
                if (joint == null || joint.connectedBody == null) continue;

                Identifiable ident = joint.connectedBody.GetComponent<Identifiable>();
                if (ident != null)
                {
                    ret.Add(ident);
                }
            }
#endif
            return ret;
        }

        public static int Get_Captive_Item_Count(Identifiable.Id id)
        {
            return Get_Captive_Items().Count(o => o.id == id);
        }
        
    }
}
