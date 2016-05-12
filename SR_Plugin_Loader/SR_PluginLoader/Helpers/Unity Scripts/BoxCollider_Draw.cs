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
    public class BoxCollider_Draw : MonoBehaviour
    {
        public Color color = Color.green;

        private Vector3 v3FrontTopLeft;
        private Vector3 v3FrontTopRight;
        private Vector3 v3FrontBottomLeft;
        private Vector3 v3FrontBottomRight;
        private Vector3 v3BackTopLeft;
        private Vector3 v3BackTopRight;
        private Vector3 v3BackBottomLeft;
        private Vector3 v3BackBottomRight;

        static Material lineMaterial;

        private Vector3 pos;
        static void CreateLineMaterial()
        {
            if (!lineMaterial)
            {
                // Unity has a built-in shader that is useful for drawing
                // simple colored things.
                var shader = Shader.Find("Hidden/Internal-Colored");
                lineMaterial = new Material(shader);
                lineMaterial.hideFlags = HideFlags.HideAndDontSave;
                // Turn on alpha blending
                lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                // Turn backface culling off
                lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
                // Turn off depth writes
                lineMaterial.SetInt("_ZWrite", 0);
            }
        }
        private void Start()
        {
            CreateLineMaterial();
        }


        private void Update()
        {
            CalcPositons();
        }

        private void OnRenderObject()
        {
            DoRender();
        }

        void OnPostRender()
        {
            //DoRender();
        }

        void DoRender()
        {
            GL.PushMatrix();
            GL.LoadIdentity();
            //GL.MultMatrix(transform.localToWorldMatrix);

            lineMaterial.SetPass(0);


            DrawBox();
            GL.PopMatrix();
        }

        void CalcPositons()
        {
            Bounds bounds;
            BoxCollider bc = GetComponent<BoxCollider>();
            if (bc != null)
                bounds = bc.bounds;
            else
            return;
            
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

            v3FrontTopLeft = transform.TransformPoint(v3FrontTopLeft);
            v3FrontTopRight = transform.TransformPoint(v3FrontTopRight);
            v3FrontBottomLeft = transform.TransformPoint(v3FrontBottomLeft);
            v3FrontBottomRight = transform.TransformPoint(v3FrontBottomRight);
            v3BackTopLeft = transform.TransformPoint(v3BackTopLeft);
            v3BackTopRight = transform.TransformPoint(v3BackTopRight);
            v3BackBottomLeft = transform.TransformPoint(v3BackBottomLeft);
            v3BackBottomRight = transform.TransformPoint(v3BackBottomRight);
        }

        private void DrawLine(Vector3 A, Vector3 B)
        {
            GL.Vertex(A);
            GL.Vertex(B);
        }

        private void DrawBox()
        {
            GL.Begin(GL.LINES);
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

            GL.End();
        }
    }
}
