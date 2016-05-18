using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SR_PluginLoader
{
    /// <summary>
    /// Will render the bounds of an objects BoxCollider if it has one
    /// Except it doesn't work right now, the OpenGL matricies aren't right somehow.
    /// </summary>
    public class Debug_Visualizer : MonoBehaviour
    {
        public Color color = Color.blue;
        public static Material lineMat;
        private List<BBDraw> boxes = new List<BBDraw>();
        private Transform trans = null;

        static Debug_Visualizer()
        {
            if (!lineMat)
            {
                // Unity has a built-in shader that is useful for drawing
                // simple colored things.
                var shader = Shader.Find("Hidden/Internal-Colored");
                lineMat = new Material(shader);
                lineMat.hideFlags = HideFlags.HideAndDontSave;
                // Turn on alpha blending
                lineMat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                lineMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                // Turn backface culling off
                lineMat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
                // Turn off depth writes
                lineMat.SetInt("_ZWrite", 1);
            }
        }

        void Awake() { Retarget(transform); }                
        
        public void Untarget() { boxes.Clear(); trans = null; }

        public void Retarget(Transform tr)
        {
            trans = tr;
            boxes.Clear();
            Bounds bounds = new Bounds(trans.position, Vector3.one);

            Collider[] list = trans.GetComponentsInChildren<Collider>();
            foreach(Collider col in list)
            {
                bounds.Encapsulate(col.bounds);
                Bounds bb = col.bounds;
                bb.center -= trans.position;
                boxes.Add(new BBDraw(bb, Color.grey));
            }

            bounds.center -= trans.position;
            boxes.Add(new BBDraw(bounds, Color.blue));
        }

        void OnRenderObject()
        {
            Draw();
        }

        public void Draw(Color? clr=null)
        {
            if (trans == null) return;
            Color c = color;
            if (clr.HasValue) c = clr.Value;

            GL.PushMatrix();
            try
            {
                GL.MultMatrix(trans.localToWorldMatrix);
                GL.Begin(GL.LINES);
                lineMat.SetPass(0);
                GL.Color(c);
                foreach (BBDraw box in boxes) { box.Draw(); }

                GL.End();
            }
            catch(Exception ex)
            {
                SLog.Error(ex);
            }
            finally
            {
                GL.PopMatrix();
            }
        }
    }

    public class BBDraw
    {
        Color color;
        #region Verticies

        private Vector3 v3FrontTopLeft;
        private Vector3 v3FrontTopRight;
        private Vector3 v3FrontBottomLeft;
        private Vector3 v3FrontBottomRight;
        private Vector3 v3BackTopLeft;
        private Vector3 v3BackTopRight;
        private Vector3 v3BackBottomLeft;
        private Vector3 v3BackBottomRight;
        #endregion

        public BBDraw(Bounds bounds, Color c)
        {
            color = c;
            Vector3 v3Center = bounds.center;
            Vector3 v3Extents = bounds.extents;

            v3FrontTopLeft = new Vector3(v3Center.x - v3Extents.x, v3Center.y + v3Extents.y, v3Center.z - v3Extents.z);  // Front top left corner
            v3FrontTopRight = new Vector3(v3Center.x + v3Extents.x, v3Center.y + v3Extents.y, v3Center.z - v3Extents.z);  // Front top right corner
            v3FrontBottomLeft = new Vector3(v3Center.x - v3Extents.x, v3Center.y - v3Extents.y, v3Center.z - v3Extents.z);  // Front bottom left corner
            v3FrontBottomRight = new Vector3(v3Center.x + v3Extents.x, v3Center.y - v3Extents.y, v3Center.z - v3Extents.z);  // Front bottom right corner
            v3BackTopLeft = new Vector3(v3Center.x - v3Extents.x, v3Center.y + v3Extents.y, v3Center.z + v3Extents.z);  // Back top left corner
            v3BackTopRight = new Vector3(v3Center.x + v3Extents.x, v3Center.y + v3Extents.y, v3Center.z + v3Extents.z);  // Back top right corner
            v3BackBottomLeft = new Vector3(v3Center.x - v3Extents.x, v3Center.y - v3Extents.y, v3Center.z + v3Extents.z);  // Back bottom left corner
            v3BackBottomRight = new Vector3(v3Center.x + v3Extents.x, v3Center.y - v3Extents.y, v3Center.z + v3Extents.z);  // Back bottom right corner

            /*
            v3FrontTopLeft = transform.TransformPoint(v3FrontTopLeft);
            v3FrontTopRight = transform.TransformPoint(v3FrontTopRight);
            v3FrontBottomLeft = transform.TransformPoint(v3FrontBottomLeft);
            v3FrontBottomRight = transform.TransformPoint(v3FrontBottomRight);
            v3BackTopLeft = transform.TransformPoint(v3BackTopLeft);
            v3BackTopRight = transform.TransformPoint(v3BackTopRight);
            v3BackBottomLeft = transform.TransformPoint(v3BackBottomLeft);
            v3BackBottomRight = transform.TransformPoint(v3BackBottomRight);
            */
        }

        public void Draw()
        {
            GL.Color(color);

            DrawLine(v3FrontTopLeft, v3FrontTopRight);
            DrawLine(v3FrontTopRight, v3FrontBottomRight);
            DrawLine(v3FrontBottomRight, v3FrontBottomLeft);
            DrawLine(v3FrontBottomLeft, v3FrontTopLeft);

            DrawLine(v3BackTopLeft, v3BackTopRight);
            DrawLine(v3BackTopRight, v3BackBottomRight);
            DrawLine(v3BackBottomRight, v3BackBottomLeft);
            DrawLine(v3BackBottomLeft, v3BackTopLeft);

            DrawLine(v3FrontTopLeft, v3BackTopLeft);
            DrawLine(v3FrontTopRight, v3BackTopRight);
            DrawLine(v3FrontBottomRight, v3BackBottomRight);
            DrawLine(v3FrontBottomLeft, v3BackBottomLeft);
        }

        private void DrawLine(Vector3 A, Vector3 B)
        {
            GL.Vertex(A);
            GL.Vertex(B);
        }
    }
}
