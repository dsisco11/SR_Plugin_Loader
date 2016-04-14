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

        bool Apply(GameObject obj);
        bool Purchase(GameObject obj);
        bool CanBuy();
    }

    public abstract class UpgradeBase : IUpgrade
    {
        private Plugin Parent;
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
        public string ID { get { if (Parent==null) { return String.Concat("unknown.", idBase); } return String.Concat(Parent, ".", idBase); } }
        private string idBase = "";

        public abstract bool Purchase(GameObject obj);

        public Func<bool> can_buy_func = null;
        public bool CanBuy() { if (Player.HasUpgrade(this)) { return false; } if (can_buy_func != null) { return can_buy_func(); } return true; }

        
        public UpgradeBase(Plugin parent, string id, int cost, string name, string desc, Action<GameObject> function, Texture2D icon)
        {
            Parent = parent;
            idBase = id;
            Cost = cost;
            Name = name;
            Description = desc;
            Icon = icon;
            ApplyFunction = function;

            if (Icon == null && Parent != null) Icon = Parent.icon;
        }

        public bool Apply(GameObject obj)
        {
            if (ApplyFunction == null) return false;

            ApplyFunction(obj);
            return true;
        }
    }

    /// <summary>
    /// An upgrade that modifies the players stats
    /// </summary>
    public class PlayerUpgrade : UpgradeBase
    {
        public override Upgrade_Type Type { get { return Upgrade_Type.PLAYER_UPGRADE; } }
        public override bool Purchase(GameObject sender) { return Upgrade_System.TryPurchase(sender.GetComponent<PersonalUpgradeUI>(), this); }

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

        public PlotUpgrade(Plugin parent, string id, int cost, string name, string desc, Action<GameObject> function, Texture2D icon = null) : base(parent, id, cost, name, desc, function, icon)
        {
            Upgrade_System.Register(this);
        }
    }
}
