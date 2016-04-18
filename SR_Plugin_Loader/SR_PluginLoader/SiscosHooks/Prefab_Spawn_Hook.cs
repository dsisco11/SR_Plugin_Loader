using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SR_PluginLoader
{
    class Prefab_Spawn_Hook : MonoBehaviour
    {
        /// <summary>
        /// Start occurs after all other behaviour scripts for th eobject have initiated and can now be interacted with.
        /// </summary>
        private void Start()
        {
            Identifiable ident = base.gameObject.GetComponent<Identifiable>();
            Identifiable.Id ID = ident? ident.id : Identifiable.Id.NONE;

            object return_value = new object();
            SiscosHooks.call(HOOK_ID.Entity_Spawned, base.gameObject, ref return_value, new object[] { ID });

            if (Identifiable.IsSlime(ID) || Identifiable.IsLargo(ID) || Identifiable.IsGordo(ID))
                SiscosHooks.call(HOOK_ID.Slime_Spawned, base.gameObject, ref return_value, new object[] { ID });

            if (Identifiable.IsAnimal(ID))
                SiscosHooks.call(HOOK_ID.Animal_Spawned, base.gameObject, ref return_value, new object[] { ID });

            if (Identifiable.IsFood(ID))
                SiscosHooks.call(HOOK_ID.Food_Spawned, base.gameObject, ref return_value, new object[] { ID });
        }
    }
}
