using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SR_PluginLoader
{
    public class dGizmo_BB : dGizmo
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


        public dGizmo_BB(GameObject gm, Color color) : base(GizmoType.BOUNDING_BOX)
        {
            this.color = color;
            Bounds bounds = new Bounds(gm.transform.position, Vector3.one);

            MeshRenderer[] list = gm.transform.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer mr in list) { bounds.Encapsulate(mr.bounds); }

            bounds.center -= gm.transform.position;

            Build(bounds);
        }

        public dGizmo_BB(Bounds bounds, Color color) : base(GizmoType.BOUNDING_BOX)
        {
            this.color = color;
            Build(bounds);
        }

        void Build(Bounds bounds)
        {
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


            BuildLine(v3FrontTopLeft, v3FrontTopRight);
            BuildLine(v3FrontTopRight, v3FrontBottomRight);
            BuildLine(v3FrontBottomRight, v3FrontBottomLeft);
            BuildLine(v3FrontBottomLeft, v3FrontTopLeft);

            BuildLine(v3BackTopLeft, v3BackTopRight);
            BuildLine(v3BackTopRight, v3BackBottomRight);
            BuildLine(v3BackBottomRight, v3BackBottomLeft);
            BuildLine(v3BackBottomLeft, v3BackTopLeft);

            BuildLine(v3FrontTopLeft, v3BackTopLeft);
            BuildLine(v3FrontTopRight, v3BackTopRight);
            BuildLine(v3FrontBottomRight, v3BackBottomRight);
            BuildLine(v3FrontBottomLeft, v3BackBottomLeft);

            const float CROSS_SIZE = 0.2f;
            Color clr = new Color(1f, 0.3f, 0.3f, 0.5f);
            Lines.Add(new GizmoLine((Vector3.up * -CROSS_SIZE) + v3Center, (Vector3.up * CROSS_SIZE) + v3Center, clr));
            Lines.Add(new GizmoLine((Vector3.right * -CROSS_SIZE) + v3Center, (Vector3.right * CROSS_SIZE) + v3Center, clr));
            Lines.Add(new GizmoLine((Vector3.forward * -CROSS_SIZE) + v3Center, (Vector3.forward * CROSS_SIZE) + v3Center, clr));
        }

        private void BuildLine(Vector3 v1, Vector3 v2)
        {
            Lines.Add(new GizmoLine(v1, v2, color));
        }
    }
}
