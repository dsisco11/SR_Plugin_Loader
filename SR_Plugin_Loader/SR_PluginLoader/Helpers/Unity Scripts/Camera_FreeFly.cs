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
    public class Camera_FreeFly : MonoBehaviour
    {
        #region Variables
        /// <summary>
        /// Is the free-fly camera currently active? Is the user currently using it?
        /// </summary>
        public static bool FLYING { get { return _is_flying; } }
        private static bool _is_flying = false;
        private static Camera cam = null;
        private static Camera_FreeFly instance = null;
        private static SimpleSmoothMouseLook mouseInput = null;
        public static Camera_FreeFly Instance { get { if (instance == null) { instance = Spawn(); } return instance; } }
        #endregion

        #region Settings

        float flySpeed = 0.5f;
        bool shift = false;
        bool ctrl = false;
        float accelerationAmount = 3f;
        float accelerationRatio = 1f;
        float slowDownRatio = 0.5f;
        #endregion

        internal static Camera_FreeFly Spawn()
        {
            cam = new Camera();
            mouseInput = cam.gameObject.AddComponent<SimpleSmoothMouseLook>();
            return cam.gameObject.AddComponent<Camera_FreeFly>();
        }
        

        public void Toggle()
        {
            if(_is_flying)
            {
                _is_flying = false;
                Camera.main.enabled = true;
                cam.enabled = false;
            }
            else
            {
                _is_flying = true;
                Camera.main.enabled = false;
                cam.enabled = true;
                cam.transform.position = Camera.main.transform.position;
                cam.transform.rotation = Camera.main.transform.rotation;
            }
        }

        private void Update()
        {
            if (!_is_flying) return;
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
                transform.Translate(-Camera.main.transform.forward * flySpeed * Input.GetAxis("Vertical"));
            }
            if (Input.GetAxis("Horizontal") != 0)
            {
                transform.Translate(-Camera.main.transform.right * flySpeed * Input.GetAxis("Horizontal"));
            }
            if (Input.GetKey(KeyCode.E))
            {
                transform.Translate(Camera.main.transform.up * flySpeed * 0.5f);
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                transform.Translate(-Camera.main.transform.up * flySpeed * 0.5f);
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                Player.gameObject.transform.position = transform.position; //Moves the player to the flycam's position. Make sure not to just move the player's camera.
            }
        }

    }
}
