//#define USING_VANILLA // This turns off any references to methods that arent available when using a "vanilla" assembly-csharp file, eg: one that hasnt had the installer run on it.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SR_PluginLoader
{
    /// <summary>
    /// Provides methods to alter player state and obtain player info.
    /// </summary>
    public static class Player
    {
        private static PlayerState player { get { return SRSingleton<SceneContext>.Instance.PlayerState; } }
        private static GameObject pObj { get { return SRSingleton<SceneContext>.Instance.Player; } }

        public static GameObject gameObject { get { return pObj; } }
        public static PlayerState state { get { return player; } }
        public static WeaponVacuum Weapon { get { if(pObj==null) { return null; } return pObj.GetComponentInChildren<WeaponVacuum>(); } }
        public static EnergyJetpack Jetpack { get { if (pObj == null) { return null; } return pObj.GetComponent<EnergyJetpack>(); } }

        #region Getters / Setters
        /// <summary>
        /// The players current health. To alter health <see cref="Player.Damage(int)"/>
        /// </summary>
        public static int Health { get { return player.GetCurrHealth(); } set { player.SetHealth(value); } }

        /// <summary>
        /// The players current energy level. To alter energy level <see cref=""/>
        /// </summary>
        public static int Energy { get { return player.GetCurrEnergy(); } set { player.SetEnergy(value); } }

        /// <summary>
        /// The players current radiation level. To alter radiation level <see cref=""/>
        /// </summary>
        public static int Rads { get { return player.GetCurrRad(); } set { player.SetRad(value); } }

        /// <summary>
        /// How much currency the player has. To add or remove currency <see cref="Player.SpendCurrency(int, bool)"/>
        /// </summary>
        public static int Currency { get { return player.GetCurrency(); } }

        /// <summary>
        /// How many keys the player has. <seealso cref="Player.AddKeys(int)"/> <seealso cref="Player.SpendKeys(int)"/>
        /// </summary>
        public static int Keys { get { return player.GetKeys(); } set { player.SetKeys(value); } }

#if !USING_VANILLA
        public static int MaxHealth { get { return player.maxHealth; } set { player.maxHealth = value; Health = Health; } }
        public static int MaxEnergy { get { return player.maxEnergy; } set { player.maxEnergy = value; Energy = Energy; } }
        public static int MaxRads { get { return player.maxRads; } set { player.maxRads = value; Rads = Rads; } }
        public static int MaxAmmo { get { return player.maxAmmo; } set { player.maxAmmo = value; } }
#else
        public static int MaxHealth, MaxEnergy, MaxRads, MaxAmmo;
#endif
        #endregion

        #region Inventory Helpers
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
#endregion

        #region VacPak Helpers
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
        
        /// <summary>
        /// Returns the number of objects which are currently being sucked in by the players weapon.
        /// </summary>
        /// <param name="id">The type of object to count</param>
        /// <returns></returns>
        public static int Get_Captive_Item_Count(Identifiable.Id id)
        {
            return Get_Captive_Items().Count(o => o.id == id);
        }

        /// <summary>
        /// Returns the <c>Vector3</c> position that the player is currently looking at.
        /// </summary>
        public static Vector3? RaycastPos()
        {
            Ray ray = new Ray(Weapon.vacOrigin.transform.position, Weapon.vacOrigin.transform.up);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit, float.MaxValue, -1610612997))
            {
                return raycastHit.point;
            }

            return null;
        }

        /// <summary>
        /// Returns the players current view raycast.
        /// </summary>
        public static RaycastHit? Raycast()
        {
            Ray ray = new Ray(Weapon.vacOrigin.transform.position, Weapon.vacOrigin.transform.up);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit, float.MaxValue, -1610612997))
            {
                return raycastHit;
            }

            return null;
        }
        #endregion

        #region Upgrade Helpers

        public static void GiveUpgrade(PlayerUpgrade upgrade)
        {
            Upgrade_System.PlayerUpgrades.Add(upgrade);
            upgrade.Apply(player.gameObject);
        }

        public static bool HasUpgrade(string ID) { return (Upgrade_System.PlayerUpgrades.Exists(u => (String.Compare(u.ID, ID)==0)) || Upgrade_System.Player_Upgrades_Missing.Exists(o => (String.Compare(o, ID)==0))); }
        public static bool HasUpgrade(IUpgrade upgrade) { return HasUpgrade(upgrade.ID); }
        public static bool HasUpgrade(PlayerState.Upgrade upgrade) { return player.HasUpgrade(upgrade); }

        public static bool CanBuyUpgrade(string ID)
        {
            IUpgrade up = Upgrade_System.Get_Upgrade(Upgrade_Type.PLAYER_UPGRADE, ID);
            if (up == null) return false;

            return CanBuyUpgrade(up);
        }
        public static bool CanBuyUpgrade(IUpgrade upgrade)
        {
            return upgrade.CanBuy();
        }
        public static bool CanBuyUpgrade(PlayerState.Upgrade upgrade) { return player.CanGetUpgrade(upgrade); }
    #endregion
        
        #region Misc

        public static void Damage(int dmg) { player.Damage(dmg); }
        public static void AddRads(float rads) { player.AddRads(rads); }

        public static void SpendEnergy(float energy) { player.SpendEnergy(energy); }
        public static void SpendCurrency(int adjust, bool forcedLoss = false) { player.SpendCurrency(adjust, forcedLoss); }

        public static void AddKeys(int num = 1) { for (int i = 0; i < num; i++) { player.AddKey(); } }
        public static bool SpendKeys(int num = 1) {
            if (Player.Keys < num) return false;
            for (int i = 0; i < num; i++) { player.SpendKey(); }
            return true;
        }
    #endregion

    }
}
