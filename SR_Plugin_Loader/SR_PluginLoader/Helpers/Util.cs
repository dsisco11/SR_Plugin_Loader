using SimpleJSON;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
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


    public static class Util
    {
        public static int UnixTimestamp() { return (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds; }

        public static MessageBundle CreateMessageBundle(string bundleName, Dictionary<string, string> translations)
        {
            var rBundle = new ResourceBundle(translations);
            var bundle = new MessageBundle();
            bundle.Init(SRSingleton<GameContext>.Instance.MessageDirector, bundleName, rBundle, null);

            return bundle;
        }
        
        public static WebClient Get_Web_Client()
        {
            var webClient = new WebClient();
            // Add a useragent string so GitHub doesnt return 403 and also so they can have a chat if they like.
            webClient.Headers.Add("user-agent", Updater_Base.USER_AGENT);
            // Add a handler for SSL certs because mono doesnt have any trusted ones by default
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback((sender, certificate, chain, policyErrors) => { return true; });

            return webClient;
        }

        /// <summary>
        /// Gets a string listing information for all Components attached to all GameObjects attached to a specified GameObject.
        /// </summary>
        public static string Get_Unity_GameObject_Hierarchy_String(GameObject targ, int nest_level=0)
        {
            StringBuilder sb = new StringBuilder();
            // Format our pre-spacing string
            string space = "";
            for (int i = 0; i < nest_level; i++) space += "   ";

            // First let's print this objects name
            sb.AppendFormat("{0}- \"{1}\"  Children<{2}>", space, targ.name, targ.transform.childCount);

            List<Component> comps = targ.GetComponents<Component>().ToList();
            sb.Append("  Components("+comps.Count+"): {");
            // Now let's list all of the script components attached to it
            sb.Append(String.Join(", ", comps.Select(c => c.GetType().Name).ToArray()));
            // End the components list
            sb.AppendLine("}");

            if (nest_level < 9)
            {
                // Start the process of listing all attached GameObjects
                for (int idx = 0; idx < targ.transform.childCount; idx++)// start at offset 1 to avoid printing ourself again!
                {
                    Transform trans = targ.transform.GetChild(idx);
                    if (trans.gameObject == trans.parent || trans.parent == null) continue;
                    string str = Get_Unity_GameObject_Hierarchy_String(trans.gameObject, nest_level + 1);
                    if(!String.IsNullOrEmpty(str)) sb.AppendLine(str.TrimEnd(new char[] { '\n' }));
                }
            }

            return sb.ToString().TrimEnd(new char[] { '\n' });
        }

        /// <summary>
        /// Translates a Unity GUI object's transforms into a screenspace Rect area.
        /// </summary>
        public static Rect Get_Unity_UI_Object_Area(GameObject obj)
        {
            RectTransform rt = obj.GetComponent<RectTransform>();
            if (rt == null) return new Rect(obj.transform.localPosition, new Vector2(5, 5));

            Vector2 hSZ = (rt.sizeDelta * 0.5f);
            return new Rect(new Vector2(rt.position.x - hSZ.x, Screen.height - (rt.position.y + hSZ.y)), rt.sizeDelta);
            //return new Rect(rt.anchoredPosition + new Vector2(rt.position.x, rt.position.y), rt.sizeDelta);
        }

        /// <summary>
        /// Translates a Unity GUI object's transforms into a screenspace Rect area.
        /// </summary>
        public static Vector2 Get_Unity_UI_Object_AnchorPos(GameObject obj)
        {
            if (obj == null) return Vector2.zero;
            RectTransform rt = (obj.transform as RectTransform);
            if (rt == null) return Vector2.zero;
            
            return new Vector2(rt.anchoredPosition.x, Screen.height - (rt.anchoredPosition.y));
            //return new Rect(rt.anchoredPosition + new Vector2(rt.position.x, rt.position.y), rt.sizeDelta);
        }

        #region Hashing

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
        
        public static string Git_File_Sha1_Hash(string file)
        {
            string hash_empty = "e69de29bb2d1d6434b8b29ae775ad8c2e48c5391";//this is the correct hash that should be given for an empty file.
            if (!File.Exists(file)) return hash_empty;

            byte[] buf = File.ReadAllBytes(file);
            return Git_Blob_Sha1_Hash(buf);
        }

        public static string Git_Blob_Sha1_Hash(byte[] buf)
        {
            // Encoding.ASCII is 7bit encoding but we want 8bit, so we need to use "iso-8859-1"
            Encoding enc = Encoding.GetEncoding("iso-8859-1");
            string head = String.Format("blob {0}\0{1}", buf.Length, enc.GetString(buf));

            /*
            StringBuilder dat = new StringBuilder();
            dat.Append(head);
            dat.Append(Encoding.ASCII.GetString(buf));
            //DebugHud.Log("HEAD: size({0}) content: '{1}'", dat.Length, dat.ToString());
            byte[] blob_buf = Encoding.ASCII.GetBytes(dat.ToString());
            */

            SHA1 sha = SHA1.Create();
            byte[] hash = sha.ComputeHash( enc.GetBytes(head) );

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }

            string hash_foobar = "323fae03f4606ea9991df8befbb2fca795e648fa";// Correct GIT hash for a file containing only "foobar\n"
            bool match = (String.Compare(sb.ToString(), hash_foobar)==0);
            //DebugHud.Log("[SHA1 HASH TEST] Match<{0}>  Hash: {1}  HEAD: '{2}'", (match?"TRUE":"FALSE"), sb.ToString(), head);

            return sb.ToString();
        }
        #endregion

        public static void Log_Resource_Names()
        {
            var thisExe = System.Reflection.Assembly.GetCallingAssembly();
            string[] resources = thisExe.GetManifestResourceNames();

            foreach(var res in resources)
            {
                DebugHud.Log(res);
            }
        }

        public static Stream Get_Resource_Stream(string name, string namespace_str)
        {
            string asset_name = String.Format("{0}.Resources.{1}", namespace_str, name);
            return Assembly.GetCallingAssembly().GetManifestResourceStream(asset_name);
        }

        public static byte[] Load_Resource(string name, string namespace_str = "SR_PluginLoader")
        {
            try
            {
                using (Stream stream = Get_Resource_Stream(name, namespace_str))
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

        #region TEXTURES
        [Obsolete("Use TextureHelper.Load_From_Resource instead!", true)]
        public static Texture2D Load_Texture_Resource(string name, string namespace_str)
        {
            return (Texture2D)TextureHelper.Load_From_Resource(name, namespace_str);
        }

        [Obsolete("Use TextureHelper.Load_From_Data instead!", true)]
        public static Texture2D Load_Texture_From_Data(byte[] data)
        {
            return (Texture2D)TextureHelper.Load(data);
        }
        /// <summary>
        /// Helper function to load an array of bytes as a struct instance. God I wish I had done this whole loader in C++
        /// </summary>
        public static T BytesToStructure<T>(byte[] bytes)
        {
            int size = Marshal.SizeOf(typeof(T));
            IntPtr ptr = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(bytes, 0, ptr, size);
                return (T)Marshal.PtrToStructure(ptr, typeof(T));
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }
        #endregion

        #region UI Helpers

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
            return Util.Create_Texture(1, size, (int x, int y, int w, int h) => {
                float f = ((float)y / (float)h);
                float g = Util.Lerp(0.25f, 0.15f, f);
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

        public static void Set_BG_Gradient(GUIStyleState style, int pixels, GRADIENT_DIR dir, Color clr1, Color clr2, float? exp)
        {
            if (style == null) return;
            var tex = Get_Gradient_Texture(pixels, dir, clr1, clr2, exp.HasValue, exp.HasValue ? exp.Value : 1f);
            style.background = tex;
        }

        public static void Set_BG_Gradient(GUIStyleState style, int pixels, GRADIENT_DIR dir, Color clr, float darken)
        {
            if (style == null) return;

            float a = clr.a;
            var tex = Get_Gradient_Texture(pixels, dir, clr, (clr*darken).SetAlpha(a));
            style.background = tex;
        }

        public static void Set_BG_Gradient(GUIStyleState style, int pixels, GRADIENT_DIR dir, Color clr, float darken, float exp)
        {
            if (style == null) return;

            float a = clr.a;
            var tex = Get_Gradient_Texture(pixels, dir, clr, (clr*darken).SetAlpha(a), false, exp);
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

        #endregion

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

        public static string FormatByteArray(byte[] bytes)
        {
            var sb = new StringBuilder("new byte[] { ");
            foreach (var b in bytes)
            {
                sb.Append(b + ", ");
            }
            sb.Append("}");
            return sb.ToString();
        }

        public static string JSON_Escape_String(string str)
        {
            return str.Replace("\n", "\\n").Replace("\r", "\\r").Replace("\t", "\\t");
        }

        /// <summary>
        /// Returns the radius of an objects collision bounds
        /// </summary>
        public static float Get_Object_Radius(GameObject obj)
        {
            return PhysicsUtil.RadiusOfObject(obj);
        }

        #region Item Spawning

        public static GameObject TrySpawn(Identifiable.Id id, Vector3 pos, Quaternion? quat=null)
        {
            if (!quat.HasValue) quat = Quaternion.identity;
            return (GameObject)GameObject.Instantiate(Ident.GetPrefab(id), pos, quat.Value);
        }

        public static GameObject TrySpawn(Identifiable.Id id, RaycastHit ray)
        {
            var prefab = Ident.GetPrefab(id);
            float r = Get_Object_Radius(prefab);

            Vector3 pos = (ray.point + (ray.normal * r * 3f));
            return (GameObject)GameObject.Instantiate(prefab, pos, Quaternion.identity);
        }

        #endregion

        public static HashSet<Identifiable.Id> Combine_Ident_Lists(IEnumerable<HashSet<Identifiable.Id>> lists)
        {
            HashSet<Identifiable.Id> final = new HashSet<Identifiable.Id>();
            foreach (HashSet<Identifiable.Id> list in lists)
            {
                foreach (var o in list)
                {
                    final.Add(o);
                }
            }
            return final;
        }

        #region Prefab Injection
        public static void Inject_Into_Prefabs<Script>(HashSet<Identifiable.Id> ID_LIST) where Script : MonoBehaviour
        {
            // Attempt to inject our own MonoBehaviour class into some prefabs.
            foreach (Identifiable.Id id in ID_LIST)
            {
                var pref = Ident.GetPrefab(id);
                if (pref != null) pref.AddComponent<Script>();
            }
        }

        public static void Inject_Into_Prefabs<Script>(HashSet<LandPlot.Id> ID_LIST) where Script : MonoBehaviour
        {
            // Attempt to inject our own MonoBehaviour class into some prefabs.
            foreach (LandPlot.Id id in ID_LIST)
            {
                var pref = Directors.lookupDirector.GetPlotPrefab(id);
                if (pref != null) pref.AddComponent<Script>();
            }
        }

        public static void Inject_Into_Prefabs<Script>(HashSet<SpawnResource.Id> ID_LIST) where Script : MonoBehaviour
        {
            // Attempt to inject our own MonoBehaviour class into some prefabs.
            foreach (SpawnResource.Id id in ID_LIST)
            {
                var pref = Directors.lookupDirector.GetResourcePrefab(id);
                if (pref != null) pref.AddComponent<Script>();
            }
        }
        #endregion

        #region Mesh

        public static Mesh Get_Mesh_From_Identifiable(Identifiable.Id id)
        {
            var pref = Ident.GetPrefab(id);
            if (pref == null) return null;

            var inst = (GameObject)GameObject.Instantiate(pref, Vector3.zero, Quaternion.identity);

            MeshFilter mf = inst.GetComponentInChildren<MeshFilter>();
            GameObject.Destroy(inst);
            if (mf == null) return null;

            if (mf.mesh != null) return mf.mesh;
            return mf.sharedMesh;
        }

        public static MeshRenderer Get_MeshRenderer_From_Identifiable(Identifiable.Id id)
        {
            var pref = Ident.GetPrefab(id);
            if (pref == null) return null;
            
            var inst = (GameObject)GameObject.Instantiate(pref, Vector3.zero, Quaternion.identity);
            var rend = pref.GetComponent<MeshRenderer>();
            GameObject.Destroy(inst);
            return rend;
        }
        #endregion

    }
}
