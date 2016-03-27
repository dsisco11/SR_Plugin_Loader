﻿using SimpleJSON;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace SR_PluginLoader
{
    public enum GRADIENT_DIR
    {
        LEFT_RIGHT,
        TOP_BOTTOM
    }


    public static class Utility
    {
        public static WebClient Get_Web_Client()
        {
            var webClient = new WebClient();
            // Add a useragent string so GitHub doesnt return 403 and also so they can have a chat if they like.
            webClient.Headers.Add("user-agent", Updater_Base.USER_AGENT);
            // Add a handler for SSL certs because mono doesnt have any trusted ones by default
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback((sender, certificate, chain, policyErrors) => { return true; });

            return webClient;
        }

        public static string SHA(string data)
        {
            System.Security.Cryptography.SHA1 sha1 = System.Security.Cryptography.SHA1.Create();
            byte[] hash = sha1.ComputeHash(Encoding.ASCII.GetBytes(data));

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }

            return sb.ToString();
        }

        public static string SHA(string format, params object[] args)
        {
            string data = String.Format(format, args);
            System.Security.Cryptography.SHA1 sha1 = System.Security.Cryptography.SHA1.Create();
            byte[] hash = sha1.ComputeHash(Encoding.ASCII.GetBytes(data));

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }

            return sb.ToString();
        }

        public static string Get_File_Sha1(string file)
        {
            if (!File.Exists(file)) return null;
            var buf = File.ReadAllBytes(file);
            string data = Encoding.ASCII.GetString(buf);
            string data_str = String.Format("blob {0}\0{1}", data.Length, data);

            System.Security.Cryptography.SHA1 sha1 = System.Security.Cryptography.SHA1.Create();
            byte[] hash = sha1.ComputeHash(Encoding.ASCII.GetBytes(data_str));

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }

            return sb.ToString();
        }

        public static void Log_Resource_Names()
        {
            var thisExe = System.Reflection.Assembly.GetExecutingAssembly();
            string[] resources = thisExe.GetManifestResourceNames();

            foreach(var res in resources)
            {
                DebugHud.Log(res);
            }
        }

        public static byte[] Load_Resource(string name)
        {
            try
            {
                string asset_name = String.Format("SR_PluginLoader.Resources.{0}", name);
                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(asset_name))
                {
                    if (stream == null) return null;

                    byte[] buf = new byte[stream.Length];
                    int read = stream.Read(buf, 0, (int)stream.Length);
                    if (read < (int)stream.Length)
                    {
                        int remain = ((int)stream.Length - read);
                        int r = 0;
                        while (r < remain && remain > 0)
                        {
                            r = stream.Read(buf, read, remain);
                            read += r;
                            remain -= r;
                        }
                    }

                    return buf;
                }
            }
            catch (Exception ex)
            {
                DebugHud.Log(ex);
                return null;
            }
        }

        public static byte[] Read_Stream(Stream stream)
        {
            if (stream == null) return null;

            byte[] buf = new byte[stream.Length];
            int read = stream.Read(buf, 0, (int)stream.Length);
            if (read < (int)stream.Length)
            {
                int remain = ((int)stream.Length - read);
                int r = 0;
                while (r < remain && remain > 0)
                {
                    r = stream.Read(buf, read, remain);
                    read += r;
                    remain -= r;
                }
            }

            return buf;
        }
        
        public static void Set_BG_Color(GUIStyleState style, float R, float G, float B, float A = 1.0f)
        {
            Set_BG_Color(style, new Color(R, G, B, A));
        }

        public static void Set_BG_Color(GUIStyleState style, Color clr)
        {
            if (style == null) return;

            style.background = null;
            style.background = new Texture2D(1, 1);
            style.background.SetPixel(0, 0, clr);
            style.background.Apply();
        }

        public static float Lerp(float a, float b, float f)
        {
            float fv = Math.Abs(f);
            return (a * fv) + (b * (1f - fv));
        }
        
        /// <summary>
        /// Create a gradient texture from with a base color other then white-black
        /// </summary>
        /// <param name="pixels"></param>
        /// <param name="dir"></param>
        /// <param name="shade1"></param>
        /// <param name="shade2"></param>
        /// <param name="clr"></param>
        /// <param name="exponential"></param>
        /// <param name="exponent"></param>
        /// <returns></returns>
        public static Texture2D Get_Gradient_Texture(int pixels, GRADIENT_DIR dir, float shade1, float shade2, Color clr, bool exponential = false, float exponent = 1f)
        {
            var clr1 = new Color(shade1 * clr.r, shade1 * clr.g, shade1 * clr.b, clr.a);
            var clr2 = new Color(shade2 * clr.r, shade2 * clr.g, shade2 * clr.b, clr.a);
            return Get_Gradient_Texture(pixels, dir, clr1, clr2, exponential, exponent);
        }

        public static Texture2D Get_Gradient_Texture(int pixels, GRADIENT_DIR dir, float shade1, float shade2, bool exponential = false, float exponent = 1f)
        {
            var clr1 = new Color(shade1, shade1, shade1);
            var clr2 = new Color(shade2, shade2, shade2);
            return Get_Gradient_Texture(pixels, dir, clr1, clr2, exponential, exponent);
        }

        public static Texture2D Get_Gradient_Texture(int pixels, GRADIENT_DIR dir, Color clr1, Color clr2, bool exponential = false, float exponent = 1f)
        {
            int xsz = 1;
            int ysz = 1;
            if (dir == GRADIENT_DIR.TOP_BOTTOM) ysz = pixels;
            else xsz = pixels;

            var texture = new Texture2D(xsz, ysz, TextureFormat.RGBA32, true);

            for (int i = 0; i < pixels; i++)
            {
                float t = ((float)i / (float)pixels);
                if (exponential) t = (float)Math.Pow((double)t, exponent);
                if (dir == GRADIENT_DIR.TOP_BOTTOM) t = (1f - t);

                var clr = Color.Lerp(clr1, clr2, t);
                /*
                float R = Lerp(clr1.r, clr2.r, t);
                float G = Lerp(clr1.g, clr2.g, t);
                float B = Lerp(clr1.b, clr2.b, t);
                float A = Lerp(clr1.a, clr2.a, t);
                */

                int x = 0;
                int y = 0;

                if (dir == GRADIENT_DIR.TOP_BOTTOM) y = i;
                else x = i;

                texture.SetPixel(x, y, clr);
                //texture.SetPixel(x, y, new Color(R, G, B, A));
            }

            texture.anisoLevel = 4;
            texture.filterMode = FilterMode.Trilinear;
            texture.Apply(true);
            return texture;
        }


        public delegate Color PixelColoringDelegate(int x, int y, int width, int height);
        public static Texture2D Create_Texture(int width, int height, PixelColoringDelegate func)
        {
            var texture = new Texture2D(width, height, TextureFormat.RGBA32, true);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    texture.SetPixel(x, y, func(x, y, width, height));
                }
            }

            texture.anisoLevel = 4;
            texture.filterMode = FilterMode.Trilinear;
            texture.Apply(true);
            return texture;
        }

        public static Texture2D Create_Sheen_Texture(int size, Color tint)
        {
            return Utility.Create_Texture(1, size, (int x, int y, int w, int h) => {
                float f = ((float)y / (float)h);
                float g = Utility.Lerp(0.25f, 0.15f, f);
                if (y >= (h - 35)) g += 0.25f;

                return new Color(tint.r*g, tint.g*g, tint.b*g, tint.a);
            });
        }
        
        public static void Set_BG_Gradient(GUIStyleState style, int pixels, GRADIENT_DIR dir, Color clr1, Color clr2, bool exponential = false, float exponent = 1f)
        {
            if (style == null) return;
            var tex = Get_Gradient_Texture(pixels, dir, clr1, clr2, exponential, exponent);
            style.background = tex;
        }

        public static Texture2D Tint_Texture(Texture2D tex, Color clr)
        {
            for(int x=0; x<tex.width; x++)
            {
                for (int y = 0; y < tex.height; y++)
                {
                    Color p = tex.GetPixel(x, y);
                    p *= clr;
                    tex.SetPixel(x, y, p);
                }
            }

            tex.Apply();
            return tex;
        }

        public static bool floatEq(float a, float b, float epsilon = 0.001f)
        {
            float absA = Math.Abs(a);
            float absB = Math.Abs(b);
            float diff = Math.Abs(a - b);

            if (a == b)
            { // shortcut, handles infinities
                return true;
            }
            else if (a == 0 || b == 0 || diff < float.Epsilon)
            {
                // a or b is zero or both are extremely close to it
                // relative error is less meaningful here
                return diff < (epsilon * float.Epsilon);
            }
            else { // use relative error
                return diff / (absA + absB) < epsilon;
            }
        }
        
        public static string JSON_Escape_String(string str)
        {
            return str.Replace("\n", "\\n").Replace("\r", "\\r").Replace("\t", "\\t");
        }
    }
}
