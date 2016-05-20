using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SR_PluginLoader
{
    public static class MaterialHelper
    {
        public static Material mat_line = null;
        public static Material mat_bright = null;


        public static void Setup()
        {
            mat_line = Create_LineMat(UnityEngine.Rendering.BlendMode.SrcAlpha, UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat_bright = Create_LineMat(UnityEngine.Rendering.BlendMode.SrcAlpha, UnityEngine.Rendering.BlendMode.One);
        }


        internal static Material Create_LineMat(UnityEngine.Rendering.BlendMode SrcBlend, UnityEngine.Rendering.BlendMode DstBlend)
        {
            // Unity has a built-in shader that is useful for drawing
            // simple colored things.
            var shader = Shader.Find("Hidden/Internal-Colored");
            var mat = new Material(shader);
            mat.hideFlags = HideFlags.HideAndDontSave;

            mat.SetInt("_SrcBlend", (int)SrcBlend);
            mat.SetInt("_DstBlend", (int)DstBlend);

            mat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);// Turn backface culling off
            mat.SetInt("_ZWrite", 1);// Turn on depth writes

            return mat;
        }

    }
}
