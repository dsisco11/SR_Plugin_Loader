using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace SR_PluginLoader
{
    class Upgrade_System
    {
        private static bool setup = false;
        /// <summary>
        /// A complete map of all existing custom upgrades
        /// </summary>
        private static Dictionary<Upgrade_Type, List<IUpgrade>> Upgrades = new Dictionary<Upgrade_Type, List<IUpgrade>>();
        /// <summary>
        /// The list of custom upgrades the player has bought
        /// </summary>
        public static List<IUpgrade> PlayerUpgrades { get; protected set; }
        /// <summary>
        /// A list of all the player upgrade id strings we werent able to load an upgrade instance for, we keep these so that if a plugin fails to load but a player had the upgrade for it, they will not lose the upgrade when they DO get the plugin to load again.
        /// </summary>
        public static List<string> Player_Upgrades_Missing { get; protected set; }

        private static MessageBundle plyUpgradesBundle = null;


        internal static void Setup()
        {
            if (setup) return;
            setup = true;

            PlayerUpgrades = new List<IUpgrade>();
            Player_Upgrades_Missing = new List<string>();

            Upgrades.Add(Upgrade_Type.PLAYER_UPGRADE, new List<IUpgrade>());
            Upgrades.Add(Upgrade_Type.PLOT_UPGRADE, new List<IUpgrade>());

            SiscosHooks.register(null, HOOK_ID.Game_Saved, onGameSaved);
            SiscosHooks.register(null, HOOK_ID.Post_Game_Loaded, onGameLoaded);
            SiscosHooks.register(null, HOOK_ID.Spawn_Player_Upgrades_UI, onSpawn_PlayerUpgrades_Kiosk);
        }


        public static void Register(IUpgrade upgrade)
        {
            if (!Upgrades.ContainsKey(upgrade.Type)) Upgrades.Add(upgrade.Type, new List<IUpgrade>() { upgrade });

            var old = Upgrades[upgrade.Type].FirstOrDefault(o => String.Compare(o.ID, upgrade.ID) == 0);
            if (old != null) Upgrades[upgrade.Type].Remove(old);

            Upgrades[upgrade.Type].Add(upgrade);
        }

        public static IUpgrade Get_Upgrade(Upgrade_Type type, string ID)
        {
            if (!Upgrades.ContainsKey(type)) return null;

            return Upgrades[Upgrade_Type.PLAYER_UPGRADE].FirstOrDefault(o => (String.Compare(o.ID, ID)==0));
        }

        #region Purchasing
        public static bool TryPurchase(PersonalUpgradeUI kiosk, PlayerUpgrade upgrade)
        {
            if(Player.HasUpgrade(upgrade))
            {
                kiosk.PlayErrorCue();
                kiosk.Error("e.already_has_personal_upgrade");
            }
            else if (!Player.CanBuyUpgrade(upgrade))
            {
                kiosk.PlayErrorCue();
                kiosk.Error("e.ineligible_for_personal_upgrade");
            }
            else if (Player.Currency >= upgrade.Cost)
            {
                kiosk.Play(SRSingleton<GameContext>.Instance.UITemplates.purchasePersonalUpgradeCue);
                Player.SpendCurrency((int)upgrade.Cost, false);
                Player.GiveUpgrade(upgrade);
                kiosk.Close();
                return true;
            }
            else
            {
                kiosk.PlayErrorCue();
                kiosk.Error("e.insuf_coins");
            }
            return false;
        }

        public static bool TryPurchase(GameObject sender, PlotUpgrade upgrade)
        {
            return false;
        }
        #endregion


        #region Event Handlers
        private static Sisco_Return onGameLoaded(ref object sender, ref object[] args, ref object return_value)
        {
            string fileName = String.Concat((string)args[0], ".pug");

            if(File.Exists(fileName))
            {
                string str = File.ReadAllText(fileName);
                string[] list = str.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (list.Length <= 0) return null;

                foreach (string id in list)
                {
                    PlayerUpgrade upgrade = Get_Upgrade(Upgrade_Type.PLAYER_UPGRADE, id) as PlayerUpgrade;
                    if (upgrade == null)
                    {
                        Player_Upgrades_Missing.Add(id);
                        DebugHud.Log("[Upgrades] Unable to find Upgrade, ID: {0}", id);
                    }
                    else
                    {
                        Player.GiveUpgrade(upgrade);
                    }
                }
            }
            return null;
        }

        private static Sisco_Return onGameSaved(ref object sender, ref object[] args, ref object return_value)
        {
            string fileName = String.Concat((string)args[0], ".pug");

            // Always write to a temp file first when saving data
            string tmpFile = String.Concat(fileName, ".tmp");
            List<string> upgrades_list = new List<string>(Player_Upgrades_Missing);
            upgrades_list.AddRange( PlayerUpgrades.Select(o => o.ID).ToArray() );

            File.WriteAllText(tmpFile, String.Join("\n", upgrades_list.ToArray()));
            File.Copy(tmpFile, fileName, true);
            File.Delete(tmpFile);
            return null;
        }
        
        private static Sisco_Return onSpawn_PlayerUpgrades_Kiosk(ref object sender, ref object[] args, ref object return_value)
        {
            if(!Upgrades.ContainsKey(Upgrade_Type.PLAYER_UPGRADE)) return null;

            var kiosk = sender as PersonalUpgradeUI;
            GameObject panel = return_value as GameObject;
            var ui = panel.GetComponent<PurchaseUI>();
            
            foreach (IUpgrade up in Upgrades[Upgrade_Type.PLAYER_UPGRADE])
            {
                ui.AddButton(new PurchaseUI.Purchasable(up.Name, up.Sprite, up.Sprite, up.Description, up.Cost, PediaDirector.Id.BASICS, new UnityAction(() => { up.Purchase(kiosk.gameObject); }), Player.CanBuyUpgrade(up)));
            }

            return null;
        }
        #endregion
    }
}
