﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SR_PluginLoader
{
    public static class DebugUI
    {
        /// <summary>
        /// The panel that holds all of the DebugUI controls, so we can easily deactivate it if needed.
        /// </summary>
        private static uiPanel Root = null;
        public static uiPanel ROOT { get { return Root; } }

        internal static Material DEBUG_LINE_MAT;
        /// <summary>
        /// A convenient label to output the players coordinates onscreen
        /// </summary>
        internal static uiVarText lbl_player_pos = null;
        /// <summary>
        /// A label that shows the current debug drawing mode for uiControls
        /// </summary>
        internal static uiText lbl_debug_mode = null;


        public static void Setup()
        {
            SiscosHooks.register(null, HOOK_ID.Level_Loaded, onLevelLoaded);
            Create_Mat();
            Init();


            Root = uiControl.Create<uiPanel>();
            Root.Name = "DebugUI";
            Root.Set_Padding(5);
            Root.FloodXY();
            Root.local_style.normal.background = null;

            lbl_player_pos = uiControl.Create<uiVarText>(Root);

            lbl_player_pos.alignLeftSide();
            lbl_player_pos.alignTop();
            lbl_player_pos.Text = "Player Pos:";

            lbl_debug_mode = uiControl.Create<uiText>(Root);
            lbl_debug_mode.alignLeftSide();
            lbl_debug_mode.moveBelow(lbl_player_pos);
            lbl_debug_mode.isVisible = false;//only shows when the debug drawing mode isnt NONE

            uiControl.dbg_mouse_tooltip_style = new GUIStyle();
            uiControl.dbg_mouse_tooltip_style.normal.textColor = Color.white;
            Util.Set_BG_Color(uiControl.dbg_mouse_tooltip_style.normal, new Color(0f, 0f, 0f, 0.5f));
        }

        private static Sisco_Return onLevelLoaded(ref object sender, ref object[] args, ref object return_value)
        {
            Init();
            return null;
        }

        public static void Init()
        {
            if (Camera.main == null) return;
            if (Camera.main.gameObject.GetComponent<DebugUI_Script>() != null) return;

            Camera.main.gameObject.AddComponent<DebugUI_Script>();
        }

        private static void Create_Mat()
        {
            // Unity has a built-in shader that is useful for drawing
            // simple colored things.
            var shader = Shader.Find("Hidden/Internal-Colored");
            DEBUG_LINE_MAT = new Material(shader);
            DEBUG_LINE_MAT.hideFlags = HideFlags.HideAndDontSave;
            // Turn on alpha blending
            DEBUG_LINE_MAT.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            DEBUG_LINE_MAT.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            DEBUG_LINE_MAT.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            DEBUG_LINE_MAT.SetInt("_ZWrite", 0);

        }

        public static void Draw_Rect(Rect r)
        {
            const float Z = 0f;
            const float o = 0f;// 1f;
            float rx = (float)Math.Round(r.x)+0.5f;// center of the pixel
            float ry = (float)Math.Round(r.y)+0.5f;// center of the pixel
            float rw = (float)Math.Round(r.width)-1;
            float rh = (float)Math.Round(r.height)-1;

            // Left
            GL.Vertex3(rx + o, ry + 0, Z);//we don't add the offset to this point so that we don't get a single clear pixel at the upper left corner of the rectangle.
            GL.Vertex3(rx + o, ry + rh, Z);
            // Bottom
            GL.Vertex3(rx + o, ry + rh, Z);
            GL.Vertex3(rx + rw, ry + rh, Z);
            // Right
            GL.Vertex3(rx + rw, ry + rh, Z);
            GL.Vertex3(rx + rw, ry + 0, Z);
            // Top
            GL.Vertex3(rx + rw, ry + 0, Z);
            GL.Vertex3(rx + o, ry + 0, Z);
        }

        public static bool isVisible { get { return Root.isVisible; } }

        public static void Hide() { Root.isVisible = false; }
        public static void Show() { Root.isVisible = true; }
        public static void Toggle() { Root.isVisible = !Root.isVisible; }
    }

    public class DebugUI_Script : MonoBehaviour
    {
        private void Start() { DebugUI.Hide(); }

        private void Update()
        {
            // Implement some convinient debugging functions at the press of a button!

            if (Input.GetKeyUp(KeyCode.F2))
            {
                DebugUI.Toggle();// Toggling on/off
                //uiControl.DEBUG_DRAW_MODE = uiDebugDrawMode.NONE;// Reset the draw mode
            }

            if (Input.GetKeyUp(KeyCode.F3))// Toggle rendering uiControl area outlines on/off
            {
                if (!DebugUI.isVisible) DebugUI.Show();
                if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                {
                    uiControl.DEBUG_DRAW_MODE = uiDebugDrawMode.NONE;
                    DebugUI.Hide();
                }
                else
                {
                    int d = 1;
                    if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) d = -1;

                    int mode = ((int)uiControl.DEBUG_DRAW_MODE + d) % uiControl.DEBUG_DRAW_MODE_MAX;
                    if (mode < 0) mode += uiControl.DEBUG_DRAW_MODE_MAX;

                    uiControl.DEBUG_DRAW_MODE = (uiDebugDrawMode)mode;
                }

                DebugUI.lbl_debug_mode.isVisible = !(uiControl.DEBUG_DRAW_MODE == uiDebugDrawMode.NONE);
                DebugUI.lbl_debug_mode.Text = Enum.GetName(typeof(uiDebugDrawMode), uiControl.DEBUG_DRAW_MODE);
            }

            if (Input.GetKeyUp(KeyCode.F10))// Toggle the ingame HUD and Viewmodel on/off
            {
                HUD.Toggle();
                ViewModel.Toggle(HUD.isActive);
            }            
        }

        private void OnGUI()
        {
            if (Event.current.type != EventType.Repaint) return;
            if(uiControl.DEBUG_DRAW_MODE == uiDebugDrawMode.UNITY_RAYCAST_TARGET)// Special mode
            {
                if(EventSystem.current.IsPointerOverGameObject())
                {
                    //RaycastHit hit = new RaycastHit();
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    GameObject obj = null;
                    List<RaycastResult> hits = new List<RaycastResult>();
                    var pointer = new PointerEventData(EventSystem.current);
                    pointer.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                    EventSystem.current.RaycastAll(pointer, hits);

                    GL.PushMatrix();
                    DebugUI.DEBUG_LINE_MAT.SetPass(0);
                    GL.Begin(GL.LINES);
                    GL.Color(uiControl.color_purple);
                    if (hits.Count > 0)
                    {
                        //DebugHud.Log("Drawing: {0} {{{1}}}", hits.Count, String.Join(", ", hits.Select(h => String.Format("{0}({1})[p:{2}]", h.gameObject.name, h.gameObject.GetType().Name, h.gameObject.transform.parent.gameObject.name)).ToArray()));
                        foreach (RaycastResult res in hits)
                        {
                            obj = res.gameObject;
                            if (obj == null) continue;
                            Rect area = Util.Get_Unity_UI_Object_Area(obj);
                            DebugUI.Draw_Rect(area);
                        }
                    }

                    GL.End();
                    GL.PopMatrix();
                }
            }
            else if (uiControl.DEBUG_DRAW_MODE != uiDebugDrawMode.NONE)
            {
                var eType = Event.current.rawType;
                if (eType != EventType.Repaint) return;

                GL.PushMatrix();
                DebugUI.DEBUG_LINE_MAT.SetPass(0);
                GL.Color(Color.white);

                if (uiControl.DEBUG_DRAW_MODE != uiDebugDrawMode.NONE) uiControl.Draw_Debug_Outlines();

                GL.PopMatrix();

                uiControl.Debug_Draw_Tooltip();
            }
        }

        private void LateUpdate()
        {
            if (Player.isValid && DebugUI.lbl_player_pos.isVisible) DebugUI.lbl_player_pos.Value = Player.Pos.ToString();

            // Figure out which uiControl the mouse is overtop
            uiControl.debug_current_mouse_over = null;
            uiControl.dbg_mouse_tooltip.text = null;

            if(uiControl.DEBUG_DRAW_MODE != uiDebugDrawMode.NONE && Input.mousePresent && DebugUI.ROOT.isVisible)
            {
                Vector2 mousePos = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
                IEnumerable<uiControl> list = uiControl.ALL.Select(o => o.Value).Where(o => !o.isChild).Where(o => o!=DebugUI.ROOT);// It's for debug functionality screw it, we don't NEED to optimize it.
                //DebugHud.Log("{0}", String.Join(", ", list.Select(o => o.Name).ToArray()));
                const int MAX_LOOP = 999;// Limit for safety
                int loop = 0;

                while(++loop < MAX_LOOP)
                {
                    bool done = true;
                    // Since we early exit the loop once we find a control the mouse lies within;
                    // we need to counteract the fact that controls are effectively on layers according to their draw order with the last drawn controls on the 
                    // topmost layer and where input events cascade down through the layers from top to bottom.
                    foreach (uiControl c in list.Reverse())
                    {
                        if (c.couldTakeMouseEvent(mousePos))
                        {
                            uiControl.debug_current_mouse_over = c;
                            if (c.isParent)
                            {
                                list = (c as uiPanel).Get_Children();
                                done = false;
                            }
                            break;// exit foreach
                        }
                    }
                    if(done) break;// exit while
                }

                if(uiControl.debug_current_mouse_over != null)
                {
                    uiControl.dbg_mouse_tooltip.text = uiControl.debug_current_mouse_over.FullName;

                    const float tt_width_max = 600;
                    const float mouseHeight = 12;
                    Vector2 sz = uiControl.dbg_mouse_tooltip_style.CalcSize(uiControl.dbg_mouse_tooltip);
                    if (sz.x > tt_width_max) sz.x = tt_width_max;
                    sz.y = uiControl.dbg_mouse_tooltip_style.CalcHeight(uiControl.dbg_mouse_tooltip, sz.x);

                    uiControl.dbg_mouse_tooltop_area.Set(mousePos.x, mousePos.y+sz.y+3+mouseHeight, sz.x, sz.y);
                }
            }
        }
    }
}