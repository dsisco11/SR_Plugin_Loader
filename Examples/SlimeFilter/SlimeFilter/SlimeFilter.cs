using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngineInternal;
using SiscosHooks;
using SR_PluginLoader;

namespace SlimeFilter
{
    class SlimeFilter : MonoBehaviour
    {
        public void Awake()
        {
            SiscoHook.register(this, HOOK_ID.Vac_Can_Capture, this.Vac_Can_Capture);
        }

        private Sisco_Return Vac_Can_Capture(ref object sender, ref object[] args)
        {
            if (!Input.GetKey(KeyCode.LeftAlt) && !Input.GetKey(KeyCode.RightAlt)) return null;
            Vacuumable vac = (Vacuumable)sender;
            SlimeFaceAnimator anim = vac.GetComponent<SlimeFaceAnimator>();
            if (anim != null)
            {
                // Here we are telling the hook to make the root function which fired it, to return FALSE
                // in this case returning false from said function tells the game that this object cannot be vacuumed up!
                return new Sisco_Return() { has_custom_return = true, return_value = false };
            }

            return null;
        }
        
        private bool findMainMenuUI()
        {
            return (UnityEngine.Object.FindObjectOfType<MainMenuUI>().gameObject != null);
        }
    }
}
