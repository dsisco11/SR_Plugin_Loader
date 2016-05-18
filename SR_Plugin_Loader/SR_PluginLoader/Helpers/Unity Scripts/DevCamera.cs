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
        public static bool FLYING { get { return cam.enabled; } set { cam.enabled = value; instance.update_cams(); } }
        private static Camera cam = null, defaultCam=null;
        private static DevCamera instance = null;
        public static DevCamera Instance { get { if (instance == null) { instance = Spawn(); } return instance; } }

        bool EnableFog { get { return enable_fog; } set { enable_fog = value; RenderSettings.fog = value; } }
        bool enable_fog = true;
        #endregion

        #region Settings

        float flySpeed = 0.5f;
        float speedUpRatio = 3.0f;
        float slowDownRatio = 0.5f;
        #endregion

        internal static DevCamera Spawn()
        {
            defaultCam = Camera.main;
            var gm = new GameObject("Camera-Flying");
            cam = gm.AddComponent<Camera>();
            cam.tag = "MainCamera";// Tagging a camera as "MainCamera" in unity allows it to become Camera.main when enabled.
            cam.enabled = false;// Disabled by default of course!
            cam.cameraType = CameraType.Game;
            return gm.AddComponent<DevCamera>();            
        }
        
        public void Toggle() { FLYING = !FLYING; }

        private void update_cams()
        {
            gameObject.SetActive(cam.enabled);
            if (FLYING)
            {
                cam.CopyFrom(defaultCam);
                SnapToPlayer();

                RenderSettings.fog = enable_fog;
            }
            else
            {
                RenderSettings.fog = true;
            }


            defaultCam.enabled = !cam.enabled;
            ViewModel.Toggle(!FLYING);

            if (FLYING) Player.Freeze();
            else Player.Unfreeze();
        }

        void SnapToPlayer()
        {
            transform.position = defaultCam.transform.position;
            transform.rotation = defaultCam.transform.rotation;

            targetDirection = transform.rotation.eulerAngles;
            _mouseAbsolute = Vector2.zero;
            _smoothMouse = Vector2.zero;
        }

        void Update()
        {
            Rotate();
            Move();

            if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                Player.Teleport(transform.position, transform.eulerAngles);
                FLYING = false;
            }

            if (Input.GetKeyDown(KeyCode.KeypadPeriod)) { SnapToPlayer(); }
            if (Input.GetKeyDown(KeyCode.F)) { EnableFog = !EnableFog; }
        }

        void Move()
        {
            if (!FLYING) return;

            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) { flySpeed *= speedUpRatio; }
            if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift)) { flySpeed /= speedUpRatio; }
            if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl)) { flySpeed *= slowDownRatio; }
            if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl)) { flySpeed /= slowDownRatio; }

            if (Input.GetAxis("Vertical") != 0) { transform.Translate(transform.forward * flySpeed * Input.GetAxis("Vertical"), Space.World); }
            if (Input.GetAxis("Horizontal") != 0) { transform.Translate(transform.right * flySpeed * Input.GetAxis("Horizontal"), Space.World); }
            if (Input.GetKey(KeyCode.Space)) { transform.Translate(transform.up * flySpeed, Space.World); }
        }


        #region Mouse Input

        Vector2 _mouseAbsolute;
        Vector2 _smoothMouse;

        public Vector2 clampInDegrees = new Vector2(360, 180);
        public Vector3 targetDirection;

        void Start()
        {
            // Set target direction to the camera's initial orientation.
            targetDirection = transform.eulerAngles;
        }

        void Rotate()
        {
            // Allow the script to clamp based on a desired target value.
            var targetOrientation = Quaternion.Euler(targetDirection);

            // Get raw mouse input for a cleaner reading on more sensitive mice.
            var mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

            // Scale input against the sensitivity setting and multiply that against the smoothing value.
            float smoothing = Player.Input.MouseLookSmoothWeight;
            Vector2 sensitivity = Player.Input.MouseLookSensitivity;
            mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity.x * smoothing, sensitivity.y * smoothing));

            // Interpolate mouse movement over time to apply smoothing delta.
            _smoothMouse.x = Mathf.Lerp(_smoothMouse.x, mouseDelta.x, 1f / smoothing);
            _smoothMouse.y = Mathf.Lerp(_smoothMouse.y, mouseDelta.y, 1f / smoothing);

            // Find the absolute mouse movement value from point zero.
            _mouseAbsolute += _smoothMouse;

            // Clamp and apply the local x value first, so as not to be affected by world transforms.
            if (clampInDegrees.x < 360)
                _mouseAbsolute.x = Mathf.Clamp(_mouseAbsolute.x, -clampInDegrees.x * 0.5f, clampInDegrees.x * 0.5f);

            var xRotation = Quaternion.AngleAxis(-_mouseAbsolute.y, targetOrientation * Vector3.right);
            transform.rotation = xRotation;

            // Then clamp and apply the global y value.
            if (clampInDegrees.y < 360)
                _mouseAbsolute.y = Mathf.Clamp(_mouseAbsolute.y, -clampInDegrees.y * 0.5f, clampInDegrees.y * 0.5f);

            transform.rotation *= targetOrientation;

            var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, transform.InverseTransformDirection(Vector3.up));
            transform.rotation *= yRotation;
        }
        #endregion
    }
}
