using System.Collections.Generic;

namespace SR_PluginLoader
{
    public static class HOOK_SHAS
    {
        /// <summary>
        /// Maps hook names to a hash of the targeted function when we last verified it gets hooked correctly.
        /// </summary>
        public static Dictionary<string, string> SAFE_HASHES = new Dictionary<string, string>()
        {
            { "WeaponVacuum.ConsumeVacItem", "943cb10798b29970c10cb99220cdf12e2e18820b" },
            { "Vacuumable.canCapture", "6d0255377ab0b475cf687b845f33be872190c764" },
            { "Vacuumable.capture", "ffcdeecaf82998e14f412198cff60900b19f3bf7" },
            { "WeaponVacuum.Update", "53a0668a807495847d49cad626d136bfbbe515df" },
            { "GameData.Save", "168542ddf1bb1e9db56484cd427ae798dd1ba5fb" },
            { "GameData.Load", "9428acc5638c0d1653af57f2cc127307a80b1e75" },
            { "GameData.AvailableGames", "57c201632a8043fec892d46ea61b89349ede2c6c" },
            { "GameData.ToPath", "890b5f4da75c4f46a872d5cb3ca531a548b1dc2c" },
            { "DirectedActorSpawner.Spawn", "4405230dd15f667fe4cbe7efa97d070468e99642" },
            { "DirectedActorSpawner.Start", "aaf80c3525227fef096eaac7a91dae0131f3bfef" },
            { "PlayerState.CanGetUpgrade", "c35a98ae43995033a9de3a752a66d31a90189d4d" },
            { "PlayerState.ApplyUpgrade", "7f573a172f3efc8e7ba54b8ad26abf78ad4a3053" },
            { "PlayerState.Damage", "3ccb7adbde887ab8e8f71be0b8ef2bdd3daa792f" },
            { "PlayerState.SpendEnergy", "f8c5d3c1cd864d41f9b66fc619ebc7c5ed206bdb" },
            { "PlayerState.SetEnergy", "e4f1b7193df4bdf2def5f8c609d90b27d7d1de01" },
            { "PlayerState.AddCurrency", "14b1bdd1d6d7dd74b5285cd17495226a8c011c2f" },
            { "PlayerState.AddRads", "293215c463900a7c0d147eda510308faef02ea3a" },
            { "PlayerDeathHandler.OnDeath", "cfe6e05a02ee5a8f31f7f5cba9311f133018b275" },
            { "LockOnDeath.LockUntil", "3ba49e89abc4aabf64d1d902874bf3233bef71c7" },
            { "LockOnDeath.Update", "5f02bae1d9a193a89990ee439e36ab0853463c84" },
            { "CellDirector.Update", "9db3cd909784524d92e615ce6864b49c51b36173" },
            { "LandPlot.SetUpgrades", "bd60a08d7a91b422ce632e6c090708aa81597e73" },
            { "PersonalUpgradeUI.CreatePurchaseUI", "a3c77bb414437a174e9fef588dfd6dc0888a052f" },
            { "EmptyPlotUI.CreatePurchaseUI", "7e0f4756ee1be21e922901f6b2155c0f61571621" },
            { "GardenUI.CreatePurchaseUI", "9653114e66e8c4bb8f3a9973e85790faf7771499" },
            { "CorralUI.CreatePurchaseUI", "c34584c0c24aeb5da278fbb570e3acedebb17eba" },
            { "CoopUI.CreatePurchaseUI", "d907da5aedbdcdeb3ee8f8926967e43eca793a9a" },
            { "PondUI.CreatePurchaseUI", "e8e204d2319e4e14b87d670ad61c298971d522b1" },
            { "SiloUI.CreatePurchaseUI", "e849864940b1231b557a89000d6735f8ef368f2e" },
            { "IncineratorUI.CreatePurchaseUI", "6a94201a20efcfad0cc92215d10565935363aea6" },
            { "EconomyDirector.InitForLevel", "2970e88f31ffa1278e7e0bca49bc4037c42c92b2" },
            { "EconomyDirector.Update", "d7f836b07cf8e403ef1292157324dedb8033d51b" },
            { "SiloCatcher.OnTriggerEnter", "8f147c79a0cd5408d5b7fb7b9190fe9d3f2e85d3" },
            { "SiloCatcher.OnTriggerStay", "09c277cb1bc8cf1cc8a25c2d950a309da6f926cf" },
            { "SpawnResource.Start", "a19f662df0961326b59221565dc0f85bf9017930" },
            { "GardenCatcher.Awake", "4b5fe6e9e25f3eb4c529ae2d837e009f367cb64b" },
            { "GardenCatcher.OnTriggerEnter", "6f7d33d0509681771f8bdb7f92a5c2f5bca3938c" },
        };
    }
}
