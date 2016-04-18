using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace SR_PluginLoader
{
    public enum Upgrade_Type
    {
        INVALID = 0,
        PLAYER_UPGRADE,
        PLOT_UPGRADE
    }

    public interface IUpgrade
    {
        Upgrade_Type Type { get; }
        /// <summary>
        /// How many credits this upgrade costs
        /// </summary>
        int Cost { get; }

        /// <summary>
        /// The icon that will represent this upgrade in the PurchaseUI
        /// </summary>
        Texture2D Icon { get; }
        Sprite Sprite { get; }

        /// <summary>
        /// The title of this upgrade
        /// </summary>
        string Name { get; }
        string Description { get; }
        /// <summary>
        /// A unique identifier string for the upgrade
        /// </summary>
        string ID { get; }
        bool IsBought { get; }

        bool Apply(GameObject obj);
        bool Purchase(GameObject obj);
        bool CanBuy();
    }

    public abstract class UpgradeBase : IUpgrade
    {
        private Plugin Parent;
        /// <summary>
        /// List of other upgrades that must be obtained before this one may.
        /// </summary>
        protected List<IUpgrade> prereqs = new List<IUpgrade>();
        public virtual Upgrade_Type Type { get { return Upgrade_Type.INVALID; } }
        /// <summary>
        /// How many credits this upgrade costs
        /// </summary>
        public int Cost { get; private set; }

        /// <summary>
        /// The function callback that is fired when this upgrade needs to apply itself.
        /// The callback will be pased a <c>GameObject</c> as it's parameter, this is the GameObject it should be applied to.
        /// </summary>
        protected Action<GameObject> ApplyFunction { get; private set; }

        /// <summary>
        /// The icon that will represent this upgrade in the PurchaseUI
        /// </summary>
        public Texture2D Icon { get; private set; }
        public Sprite Sprite { get { if (Icon == null) { return null; } return Sprite.Create(Icon, new Rect(0, 0, Icon.width, Icon.height), new Vector2(0.5f, 0.5f)); } }

        /// <summary>
        /// The title of this upgrade
        /// </summary>
        public string Name { get; private set; }
        public string Description { get; private set; }

        /// <summary>
        /// A unique identifier string for the upgrade
        /// </summary>
        public string ID { get { return _id; } protected set { _id = value.ToLower(); } }
        private string _id = null;

        public abstract bool Purchase(GameObject obj);
        public Func<bool> can_buy_func = null;
        public bool CanBuy() { if (!PrereqsMet()) { return false; } if (Player.HasUpgrade(this)) { return false; } if (can_buy_func != null) { return can_buy_func(); } return true; }
        /// <summary>
        /// Returns <c>true</c> is the player has this upgrade.
        /// </summary>
        public bool IsBought { get { return Player.HasUpgrade(this); } }

        
        public UpgradeBase(Plugin parent, string id, int cost, string name, string desc, Action<GameObject> function, Texture2D icon)
        {
            Parent = parent;
            if (Parent == null) ID = String.Concat("unknown.", id).ToLower();
            else ID = String.Concat(Parent.data.NAME, ".", Parent.data.AUTHOR, ".", id).ToLower();

            Cost = cost;
            Name = name;
            Description = desc;
            Icon = icon;
            ApplyFunction = function;

            if (Icon == null && Parent != null) Icon = Parent.icon;
            if (Icon == null) Icon = Loader.tex_unknown;
            
        }

        public bool Apply(GameObject obj)
        {
            if (ApplyFunction == null) return false;

            ApplyFunction(obj);
            return true;
        }

        private bool PrereqsMet()
        {
            foreach(IUpgrade up in prereqs)
            {
                if (!up.IsBought) return false;
            }

            return true;
        }

        /// <summary>
        /// Adds a another upgrade as a prerequisite for this one.
        /// The player will have to obtain the other upgrade before this one is available.
        /// </summary>
        /// <param name="upgrade"></param>
        public void Requires(IUpgrade upgrade)
        {
            prereqs.Add(upgrade);
        }
    }

    /// <summary>
    /// An upgrade that modifies the players stats
    /// </summary>
    public class PlayerUpgrade : UpgradeBase
    {
        public override Upgrade_Type Type { get { return Upgrade_Type.PLAYER_UPGRADE; } }
        public override bool Purchase(GameObject sender) { return Upgrade_System.TryPurchase(sender.GetComponent<PersonalUpgradeUI>(), this); }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent">A reference to your plugin, also used to uniquely identify the upgrade in save files</param>
        /// <param name="id">A string that will be used to uniquely identify this upgrade in save files</param>
        /// <param name="cost">How many credits the upgrade costs</param>
        /// <param name="name">The title text for the upgrade when shown in the upgrade kiosk</param>
        /// <param name="desc">The description text for the upgrade when shown in the upgrade kiosk</param>
        /// <param name="function">Function that applies the upgrade's effects.</param>
        /// <param name="icon">An icon used to represent the plugin when shown in the upgrade kiosk</param>
        public PlayerUpgrade(Plugin parent, string id, int cost, string name, string desc, Action<GameObject> function, Texture2D icon=null) : base(parent, id, cost, name, desc, function, icon)
        {
            Upgrade_System.Register(this);
        }
    }

    /// <summary>
    /// An upgrade that alters a land plot
    /// </summary>
    public class PlotUpgrade : UpgradeBase
    {
        public override Upgrade_Type Type { get { return Upgrade_Type.PLOT_UPGRADE; } }
        public override bool Purchase(GameObject sender) { return Upgrade_System.TryPurchase(sender, this); }
        public LandPlot.Id Kind { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent">A reference to your plugin, also used to uniquely identify the upgrade in save files</param>
        /// <param name="kind">What kind of plot item this upgrade is for; Coop, Silo, Corral, Pond, Garden, etc.</param>
        /// <param name="id">A string that will be used to uniquely identify this upgrade in save files</param>
        /// <param name="cost">How many credits the upgrade costs</param>
        /// <param name="name">The title text for the upgrade when shown in the upgrade kiosk</param>
        /// <param name="desc">The description text for the upgrade when shown in the upgrade kiosk</param>
        /// <param name="function">Function that applies the upgrade's effects.</param>
        /// <param name="icon">An icon used to represent the plugin when shown in the upgrade kiosk</param>
        public PlotUpgrade(Plugin parent, LandPlot.Id kind, string id, int cost, string name, string desc, Action<GameObject> function, Texture2D icon = null) : base(parent, id, cost, name, desc, function, icon)
        {
            Kind = kind;
            Upgrade_System.Register(this);
        }
    }
}
