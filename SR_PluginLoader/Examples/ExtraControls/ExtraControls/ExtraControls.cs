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

namespace ExtraControls
{
    class ExtraControls : MonoBehaviour
    {
        public void init()
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

        private void OnLevelWasLoaded(int lvl)
        {
            //abort if we just loaded the main menu.
            if (lvl == 0) return;

            //run this function on a seperate thread so we don't bog down the players game.
            // Doing it this way is ALSO a terrible way to achieve the results we want(finding any SiloCatchers that don't have our custom logic class on them)
            // But atm I don't have a hook to detect when a new silo is made...
            StartCoroutine("start_silo_scanning");
        }

        private IEnumerator start_silo_scanning()
        {
            while (true)
            {
                SiloCatcher[] objs = GameObject.FindObjectsOfType<SiloCatcher>();

                foreach (SiloCatcher o in objs)
                {
                    if (o.gameObject.GetComponent<SiloCatcherMod>() == null)
                    {
                        o.gameObject.AddComponent<SiloCatcherMod>();
                    }
                }

                yield return new WaitForSeconds(1.0f);
            }
        }
    }
}
