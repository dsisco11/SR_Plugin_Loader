using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SR_PluginLoader
{
    public static class Player
    {
        private static GameObject player { get { return SRSingleton<GameContext>.Instance.Player; } }
        public static WeaponVacuum Weapon { get { return player.GetComponentInChildren<WeaponVacuum>(); } }
        public static EnergyJetpack Jetpack { get { return player.GetComponent<EnergyJetpack>(); } }

        /// <summary>
        /// Returns the number of a certain item that the player has in their inventory.
        /// </summary>
        /// <returns></returns>
        public static int Get_Inv_Item_Count(Identifiable.Id id)
        {

            return 0;
        }
        
    }
}
