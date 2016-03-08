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

        public static PlayerState state { get { return player; } }
        public static WeaponVacuum Weapon { get { return pObj.GetComponentInChildren<WeaponVacuum>(); } }
        public static EnergyJetpack Jetpack { get { return pObj.GetComponent<EnergyJetpack>(); } }

        /// <summary>
        /// Returns the number of a certain item that the player has in their inventory.
        /// </summary>
        /// <returns></returns>
        public static int Get_Inv_Item_Count(Identifiable.Id id)
        {
            for (int s = 0; s< player.Ammo.slotCount; s++)
            {
                Identifiable.Id sid = player.Ammo.GetSlotName(s);
                if(sid == id)
                {
                    return player.Ammo.GetSlotCount(s);
                }
            }
            return 0;
        }
        
    }
}
