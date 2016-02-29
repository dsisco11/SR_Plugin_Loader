using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ExtraControls
{
    /*
    ===============================================================================
        THIS MOD ALTERS THE PLORT COLLECTORS SUCH THAT WHEN THE PLAYER USES
        THEIR VAC-PAK ON ONE IT WILL AUTO COLLECT ALL PLORTS WITHIN THE CORRAL.
    ===============================================================================
    */
    class SiloCatcherMod : MonoBehaviour
    {
        private float forceCollectLock = 0f;
        private PlortCollector myCollector = null;

        public void Start()
        {
            this.findCollector();
        }

        public void OnTriggerStay(Collider collider)
        {
            SiloCatcher ctch = base.GetComponentInParent<SiloCatcher>();
            if (ctch == null) return;
            if (!ctch.allowsOutput) return;

            SiloActivator activator = collider.gameObject.GetComponentInParent<SiloActivator>();
            if (activator == null) return;
            if (activator.enabled == false) return;

            //ok we are being suctioned AND we are a plort collector, guess it's time to try and force a collection!
            this.force_collection();
        }

        private void force_collection()
        {
            if (Time.fixedTime < this.forceCollectLock) return;//we are locked
            this.forceCollectLock = (Time.fixedTime + 3.0f);//locked for a few seconds

            if (this.myCollector == null) return;
            this.myCollector.DoCollection();
            this.myCollector.FromSerializable(Time.fixedTime + (3600f * this.myCollector.collectPeriod));//reset this collectors next scheduled collection.
        }

        private void findCollector()
        {
            var nearDistSqr = Mathf.Infinity;
            PlortCollector nearest = null;

            PlortCollector[] collectors = GameObject.FindObjectsOfType<PlortCollector>();
            foreach (PlortCollector obj in collectors)
            {
                var objectPos = obj.transform.position;
                var distanceSqr = (objectPos - transform.position).sqrMagnitude;
                if (distanceSqr < nearDistSqr)
                {
                    nearest = obj;
                    nearDistSqr = distanceSqr;
                }
            }

            this.myCollector = nearest;
        }
    }
}
