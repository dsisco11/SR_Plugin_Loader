using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SR_PluginLoader
{
    internal enum PrefabType
    {
        NONE=0,
        ENTITY,
        PLOT,
        RESOURCE,
    }

    class Entity_Pref_Spawn_Hook : Prefab_Spawn_Hook
    {
        protected override void Start() { Kind = PrefabType.ENTITY; base.Start(); }
    }

    class Plot_Pref_Spawn_Hook : Prefab_Spawn_Hook
    {
        protected override void Start() { Kind = PrefabType.PLOT; base.Start(); }
    }
    
    class Resource_Pref_Spawn_Hook : Prefab_Spawn_Hook
    {
        protected override void Start() { Kind = PrefabType.RESOURCE; base.Start(); }
    }


    class Prefab_Spawn_Hook : MonoBehaviour
    {
        public PrefabType Kind = PrefabType.NONE;
        /// <summary>
        /// Start occurs after all other behaviour scripts for th eobject have initiated and can now be interacted with.
        /// </summary>
        protected virtual void Start()
        {
            switch (Kind)
            {
                case PrefabType.ENTITY:
                    Handle_Entity();
                    break;
                case PrefabType.PLOT:
                    Handle_Land_Plot();
                    break;
                case PrefabType.RESOURCE:
                    Handle_Garden_Patch();
                    break;
                default:
                    throw new ArgumentException(String.Format("Unhandled PrefabType: {0}", this.Kind));
            }
        }

        private void Handle_Entity()
        {
            Identifiable ident = base.gameObject.GetComponent<Identifiable>();
            Identifiable.Id ID = ident ? ident.id : Identifiable.Id.NONE;

            object return_value = new object();
            SiscosHooks.call(HOOK_ID.Spawned_Entity, base.gameObject, ref return_value, new object[] { ID });

            if (Identifiable.IsSlime(ID) || Identifiable.IsLargo(ID) || Identifiable.IsGordo(ID))
                SiscosHooks.call(HOOK_ID.Spawned_Slime, base.gameObject, ref return_value, new object[] { ID });

            if (Identifiable.IsAnimal(ID))
                SiscosHooks.call(HOOK_ID.Spawned_Animal, base.gameObject, ref return_value, new object[] { ID });

            if (Identifiable.IsFood(ID))
                SiscosHooks.call(HOOK_ID.Spawned_Food, base.gameObject, ref return_value, new object[] { ID });

        }

        private void Handle_Land_Plot()
        {
            LandPlot plot = base.gameObject.GetComponent<LandPlot>();
            LandPlot.Id ID = plot ? plot.id : LandPlot.Id.NONE;

            object return_value = new object();
            SiscosHooks.call(HOOK_ID.Spawned_Land_Plot, base.gameObject, ref return_value, new object[] { ID });
        }

        private void Handle_Garden_Patch()
        {
            SpawnResource plot = base.gameObject.GetComponent<SpawnResource>();
            SpawnResource.Id ID = plot ? plot.id : SpawnResource.Id.NONE;

            object return_value = new object();
            SiscosHooks.call(HOOK_ID.Spawned_Garden_Patch, base.gameObject, ref return_value, new object[] { ID });
        }
    }
}
