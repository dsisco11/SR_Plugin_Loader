using SR_PluginLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GardenMastery
{
    class SackOSeedsCatcher : MonoBehaviour
    {
        public SackOSeeds activator;

        private void OnTriggerEnter(Collider col)
        {
            if (activator.Seed != Identifiable.Id.NONE) return;
            
            var _ident = col.GetComponent<Identifiable>();
            if (_ident == null) return;
            Identifiable.Id ID = _ident.id;

            if (!activator.IsAccepted(ID)) return;

            activator.Set_Seed(ID);
            GameObject.Destroy(col.gameObject);
            Sound.Play(SoundId.PURCHASED_PLOT_UPGRADE);
        }

    }
}
