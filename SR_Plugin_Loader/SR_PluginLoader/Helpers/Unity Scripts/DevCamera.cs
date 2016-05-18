using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SR_PluginLoader
{
    /// <summary>
    /// Provides a free roaming camera for debug purposes.
    /// </summary>
    public class DevCamera : MonoBehaviour
    {
        #region Variables
        /// <summary>
        /// Is the free-fly camera currently active? Is the user currently using it?
        /// </summary>
        public static bool FLYING { get { return cam.enabled; } set { cam.enabled = value; update_cams(); } }
        private static Camera cam = null, defaultCam=null;
        private static DevCamera instance = null;
        private static SimpleSmoothMouseLook mouseInput = null;
        public static DevCamera Instance { get { if (instance == null) { instance = Spawn(); } return instance; } }
        #endregion

        #region Settings

        float flySpeed = 0.5f;
        bool shift = false;
        bool ctrl = false;
        float accelerationAmount = 3f;
        float accelerationRatio = 1f;
        float slowDownRatio = 0.5f;
        #endregion

        internal static DevCamera Spawn()
        {
            defaultCam = Camera.main;
            var gm = new GameObject("Camera-Flying");
            cam = gm.AddComponent<Camera>();
            cam.tag = "MainCamera";// Tagging a camera as "MainCamera" in unity allows it to become Camera.main when enabled.
            cam.enabled = false;// Disabled by default of course!
            mouseInput = gm.AddComponent<SimpleSmoothMouseLook>();
            return gm.AddComponent<DevCamera>();
        }
        
        public void Toggle() { FLYING = !FLYING; }

        private static void update_cams()
        {
            cam.transform.position = defaultCam.transform.position;
            cam.transform.rotation = defaultCam.transform.rotation;

            defaultCam.enabled = !cam.enabled;
            ViewModel.Toggle(!FLYING);

            if (FLYING) Player.Unfreeze();
            else Player.Freeze();
        }

        private void Update()
        {
            if (!FLYING) return;

            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            {
                shift = true;
                flySpeed *= accelerationRatio;
            }

            if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
            {
                shift = false;
                flySpeed /= accelerationRatio;
            }
            if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
            {
                ctrl = true;
                flySpeed *= slowDownRatio;
            }
            if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl))
            {
                ctrl = false;
                flySpeed /= slowDownRatio;
            }

            if (Input.GetAxis("Vertical") != 0)
            {
                transform.Translate(-transform.forward * flySpeed * Input.GetAxis("Vertical"));
            }
            if (Input.GetAxis("Horizontal") != 0)
            {
                transform.Translate(-transform.right * flySpeed * Input.GetAxis("Horizontal"));
            }

            
            if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                Player.Teleport(transform.position, transform.eulerAngles);
                FLYING = false;
            }
        }

    }
}
