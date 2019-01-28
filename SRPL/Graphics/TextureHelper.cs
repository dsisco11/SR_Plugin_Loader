using System;
using System.IO;
using System.Reflection;
using UnityEngine;

using SRPL.Util;
using SRPL.Logging;

namespace SRPL.Graphics
{
    [Flags]
    public enum TextureOpFlags
    {
        None = (1 << 0),
        /// <summary>
        /// Doesnt disable mipmap generation, but rather sets the mipmap bias to a negative value such that nothing but the highest mipmap level is used (the texture originally uploaded)
        /// </summary>
        NO_MIPMAPPING = (1 << 1),
        /// <summary>
        /// Sets the texture wrap mode to CLAMP so it does not repeat.
        /// </summary>
        NO_WRAPPING = (1 << 2),
    }
    
    public enum GRADIENT_DIR
    {
        LEFT_RIGHT,
        TOP_BOTTOM
    }

    public static class TextureHelper
    {
        private static Texture2D _transparent = null;
        public static Texture TRANSPARENT { get { if (_transparent == null) { _transparent = new Texture2D(1, 1); _transparent.SetPixel(0, 0, new Color(1f, 1f, 1f, 0f)); _transparent.Apply(); } return _transparent; } }

        public static Texture2D ICON_UNKNOWN = null;
        public static Texture2D ICON_ALERT = null;
        public static Texture2D ICON_CLOSE = null;
        public static Texture2D ICON_CLOSE_DARK = null;
        public static Texture2D ICON_LOGO = null;
        public static Texture2D ICON_LOGO_SAD = null;
        public static Texture2D ICON_CHECKBOX = null;
        public static Texture2D ICON_CHECKMARK = null;
        public static Texture2D ICON_ARROW_LEFT = null;
        public static Texture2D ICON_ARROW_RIGHT = null;
        public static Texture2D ICON_NODE_ARROW_RIGHT = null;
        public static Texture2D ICON_NODE_ARROW_DOWN = null;

        /// <summary>
        /// Assists in loading a texture from a file that is an embedded resource.
        /// </summary>
        /// <param name="name">The embedded resource name.</param>
        /// <param name="namespace_str">The main namespace of the calling Assembly.</param>
        /// <returns></returns>
        public static Texture LoadFromResource(string name, string namespace_str)
        {
            string asset_name = String.Format("{0}.Resources.{1}", namespace_str, name);
            using (Stream stream = Assembly.GetCallingAssembly().GetManifestResourceStream(asset_name))
            {
                byte[] data = FileHelper.Read_Stream(stream);
                if (data == null) SLog.Info("UNABLE TO LOAD SPECIFIED RESOURCE: {0}", name);
                return Load(data, name);
            }
        }

        /// <summary>
        /// Assists in loading a texture from a file that is an embedded resource.
        /// </summary>
        /// <param name="name">The embedded resource name.</param>
        /// <param name="namespace_str">The main namespace of the calling Assembly.</param>
        /// <param name="flags">A set of flags from the <c>TextureOpFlags</c> enum.</param>
        /// <returns></returns>
        public static Texture LoadFromResource(string name, string namespace_str, TextureOpFlags flags)
        {
            string asset_name = String.Format("{0}.Resources.{1}", namespace_str, name);
            using (Stream stream = Assembly.GetCallingAssembly().GetManifestResourceStream(asset_name))
            {
                byte[] data = FileHelper.Read_Stream(stream);
                if (data == null) SLog.Info("UNABLE TO LOAD SPECIFIED RESOURCE: {0}", name);
                return Load(data, flags, name);
            }
        }

        /// <summary>
        /// Assists in loading a texture from a byte array.
        /// </summary>
        public static Texture Load(byte[] data, string name = null)
        {
            if (data == null || data.Length <= 0) return null;

            Texture2D tex = null;
            //Determine our file type
            TextureType type = identifyTextureType(data);
            switch (type)
            {
                case TextureType.DXT:
                    loadTextureDXT(out tex, data);
                    break;
                case TextureType.PNG:
                case TextureType.JPEG:
                    loadTextureNonDXT(out tex, data);
                    break;
                default:
                    throw new NotImplementedException("Unable to determine that the given file was of a supported format!");
            }

            // Suspected that at some point the 'tex' var would become a bad pointer to another texture and setting filterMode
            // or ansioLevel was causing GUIStyle text character maps to render reallyyyy blurry, distorted, and misaligned. odd.
            if (tex != null)
            {
                // Likely would indicate a deeper problem elsewhere in the code, we shall see.
                // Just some default's that are nice to have in most cases.
                tex.wrapMode = TextureWrapMode.Repeat;
                //tex.filterMode = FilterMode.Trilinear;
                //tex.anisoLevel = 1;
            }

            if (tex != null && name != null) tex.name = name;
            return tex;
        }

        /// <summary>
        /// Assists in loading a texture from a stream.
        /// </summary>
        public static Texture Load(Stream stream, string name = null)
        {
            if (stream == null) return null;

            byte[] data = FileHelper.Read_Stream(stream);
            return Load(data, name);
        }

        /// <summary>
        /// Assists in loading a texture from a byte array.
        /// </summary>
        /// <param name="data">The texture file data.</param>
        /// <param name="name">A name to assign the texture instance.</param>
        /// <param name="flags">A set of flags from the <c>TextureOpFlags</c> enum.</param>
        /// <returns></returns>
        public static Texture Load(byte[] data, TextureOpFlags flags, string name = null)
        {
            if (data == null || data.Length <= 0) return null;
            Texture2D tex = (Texture2D)TextureHelper.Load(data, name);
            enforceFlags(ref tex, flags);
            return tex;
        }

        internal static void enforceFlags(ref Texture2D tex, TextureOpFlags flags)
        {
            if ((flags & TextureOpFlags.NO_MIPMAPPING) == TextureOpFlags.NO_MIPMAPPING) tex.mipMapBias = -(tex.mipmapCount);
            if ((flags & TextureOpFlags.NO_WRAPPING) == TextureOpFlags.NO_WRAPPING) tex.wrapMode = TextureWrapMode.Clamp;
        }

        public static Texture2D Rotate(Texture2D tex, TextureOpFlags flags)
        {
            Texture2D cpy = new Texture2D(tex.width, tex.height);
            for (int x = 0; x < tex.width; x++)
            {
                for (int y = 0; y < tex.height; y++)
                {
                    Color p = tex.GetPixel(x, y);
                    cpy.SetPixel(y, (tex.width - (x + 1)), p);
                }
            }

            cpy.Apply();
            enforceFlags(ref cpy, flags);
            return cpy;
        }

        public static Texture2D FlipHorizontal(Texture2D tex, TextureOpFlags flags)
        {
            Texture2D cpy = new Texture2D(tex.width, tex.height);
            for (int x = 0; x < tex.width; x++)
            {
                for (int y = 0; y < tex.height; y++)
                {
                    Color p = tex.GetPixel(x, y);
                    cpy.SetPixel((tex.width - (x + 1)), y, p);
                }
            }

            cpy.Apply();
            enforceFlags(ref cpy, flags);
            return cpy;
        }

        public static Texture2D FlipVertical(Texture2D tex, TextureOpFlags flags)
        {
            Texture2D cpy = new Texture2D(tex.width, tex.height);
            for (int x = 0; x < tex.width; x++)
            {
                for (int y = 0; y < tex.height; y++)
                {
                    Color p = tex.GetPixel(x, y);
                    cpy.SetPixel(x, (tex.height - (y + 1)), p);
                }
            }

            cpy.Apply();
            enforceFlags(ref cpy, flags);
            return cpy;
        }

        public static void SetBGColor(GUIStyleState style, float R, float G, float B, float A = 1.0f)
        {
            SetBGColor(style, new Color(R, G, B, A));
        }

        public static void SetBGColor(GUIStyleState style, Color clr)
        {
            if (style == null) return;

            style.background = null;
            style.background = new Texture2D(1, 1);
            style.background.SetPixel(0, 0, clr);
            style.background.Apply();
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
        public static Texture2D CreateGradientTexture(int pixels, GRADIENT_DIR dir, float shade1, float shade2, Color clr, bool exponential = false, float exponent = 1f)
        {
            var clr1 = new Color(shade1 * clr.r, shade1 * clr.g, shade1 * clr.b, clr.a);
            var clr2 = new Color(shade2 * clr.r, shade2 * clr.g, shade2 * clr.b, clr.a);
            return CreateGradientTexture(pixels, dir, clr1, clr2, exponential, exponent);
        }

        public static Texture2D CreateGradientTexture(int pixels, GRADIENT_DIR dir, float shade1, float shade2, bool exponential = false, float exponent = 1f)
        {
            var clr1 = new Color(shade1, shade1, shade1);
            var clr2 = new Color(shade2, shade2, shade2);
            return CreateGradientTexture(pixels, dir, clr1, clr2, exponential, exponent);
        }

        public static Texture2D CreateGradientTexture(int pixels, GRADIENT_DIR dir, Color clr1, Color clr2, bool exponential = false, float exponent = 1f)
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
        public static Texture2D CreateTexture(int width, int height, PixelColoringDelegate func)
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

        public static Texture2D CreateSheenTexture(int size, Color tint)
        {
            return CreateTexture(1, size, (int x, int y, int w, int h) => {
                float f = ((float)y / (float)h);
                float g = Mathf.Lerp(0.25f, 0.15f, f);
                if (y >= (h - 35)) g += 0.25f;

                return new Color(tint.r * g, tint.g * g, tint.b * g, tint.a);
            });
        }

        public static void SetBGGradient(GUIStyleState style, int pixels, GRADIENT_DIR dir, Color clr1, Color clr2, bool exponential = false, float exponent = 1f)
        {
            if (style == null) return;
            var tex = CreateGradientTexture(pixels, dir, clr1, clr2, exponential, exponent);
            style.background = tex;
        }

        public static void SetBGGradient(GUIStyleState style, int pixels, GRADIENT_DIR dir, Color clr1, Color clr2, float? exp)
        {
            if (style == null) return;
            var tex = CreateGradientTexture(pixels, dir, clr1, clr2, exp.HasValue, exp.HasValue ? exp.Value : 1f);
            style.background = tex;
        }

        public static void SetBGGradient(GUIStyleState style, int pixels, GRADIENT_DIR dir, Color clr, float darken)
        {
            if (style == null) return;

            float a = clr.a;
            Color newClr = (clr * darken);
            newClr.a = a;
            var tex = CreateGradientTexture(pixels, dir, clr, newClr);
            style.background = tex;
        }

        public static void SetBGGradient(GUIStyleState style, int pixels, GRADIENT_DIR dir, Color clr, float darken, float exp)
        {
            if (style == null) return;

            float a = clr.a;
            Color newClr = (clr * darken);
            newClr.a = a;
            var tex = CreateGradientTexture(pixels, dir, clr, newClr, false, exp);
            style.background = tex;
        }

        public static Texture2D TintTexture(Texture2D tex, Color clr)
        {
            for (int x = 0; x < tex.width; x++)
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

        /// <summary>
        /// Creates a copy of a texture pixel-by-pixel, it's slow but reliable for DEBUG purposes.
        /// </summary>
        public static Texture2D Clone(Texture2D tex)
        {
            Texture2D clone = new Texture2D(tex.width, tex.height, TextureFormat.ARGB32, true);
            for (int x = 0; x < tex.width; x++)
            {
                for (int y = 0; y < tex.height; y++)
                {
                    clone.SetPixel(x, y, tex.GetPixel(x, y));
                }
            }

            return clone;
        }

        internal enum TextureType
        {
            UNKNOWN = 0,
            PNG,
            JPEG,
            DXT,
        }

        /// <summary>
        /// Loads all of the predefined common texture resources.
        /// </summary>
        internal static void LoadCommon()
        {
            const string myNamespace = "SR_PluginLoader";
            TextureOpFlags FLAGS = (TextureOpFlags.NO_MIPMAPPING & TextureOpFlags.NO_WRAPPING);

            ICON_LOGO = (Texture2D)TextureHelper.LoadFromResource("logo.png", myNamespace, FLAGS);
            ICON_LOGO_SAD = (Texture2D)TextureHelper.LoadFromResource("logo_sad.png", myNamespace, FLAGS);
            ICON_UNKNOWN = (Texture2D)TextureHelper.LoadFromResource("mystery.png", myNamespace, FLAGS);
            ICON_ALERT = (Texture2D)TextureHelper.LoadFromResource("alert.png", myNamespace, FLAGS);
            ICON_CLOSE = (Texture2D)TextureHelper.LoadFromResource("close.png", myNamespace, FLAGS);
            ICON_CLOSE_DARK = (Texture2D)TextureHelper.LoadFromResource("close.png", myNamespace, FLAGS);
            ICON_CHECKBOX = (Texture2D)TextureHelper.LoadFromResource("checkbox.png", myNamespace, FLAGS);
            ICON_CHECKMARK = (Texture2D)TextureHelper.LoadFromResource("checkmark.png", myNamespace, FLAGS);

            ICON_ARROW_LEFT = (Texture2D)TextureHelper.LoadFromResource("arrow_left.png", myNamespace, FLAGS);
            ICON_ARROW_RIGHT = (Texture2D)TextureHelper.LoadFromResource("arrow_right.png", myNamespace, FLAGS);

            ICON_NODE_ARROW_RIGHT = (Texture2D)TextureHelper.LoadFromResource("node_arrow.png", myNamespace, FLAGS);
            ICON_NODE_ARROW_DOWN = Rotate(ICON_NODE_ARROW_RIGHT, FLAGS);

            TintTexture(ICON_CLOSE_DARK, new Color(1f, 1f, 1f, 0.5f));
        }

        // The series of bytes that indicate a particular file contains DXT encoded image data.
        internal static readonly byte[] DDS_HEADER = new byte[] { 0x44, 0x44, 0x53, 0x20 };
        // The series of bytes that would indicate a particular file contians PNG image data.
        internal static readonly byte[] PNG_HEADER = new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 };
        // The magic number that identifies the start of a JPEG header.
        internal static readonly byte[] JPEG_MAGIC = new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 };
        // The series of bytes for a JPEG header that would indicate it contains image data and not another type of file.
        internal static readonly byte[] JPEG_HEADER = new byte[] { 0x4A, 0x46, 0x49, 0x46, 0x00, };
        
        internal static TextureType identifyTextureType(byte[] data)
        {
            //Determine our file type
            if (checkHeader(data, DDS_HEADER))// Is it A DDS?
                return TextureType.DXT;
            else if (checkHeader(data, PNG_HEADER))// Is it PNG?
                return TextureType.PNG;
            else if (checkHeader(data, JPEG_MAGIC) && checkHeader(data, JPEG_HEADER, 6))// Is it JPEG?
                return TextureType.JPEG;

            return TextureType.UNKNOWN;
        }

        private static void loadTextureNonDXT(out Texture2D tex, byte[] data)
        {
            tex = new Texture2D(1, 1);
            tex.LoadImage(data);
        }

        private static void loadTextureDXT(out Texture2D tex, byte[] ddsBytes)
        {
            byte ddsSizeCheck = ddsBytes[4];
            if (ddsSizeCheck != 124)
                throw new Exception("Invalid DDS DXTn texture. Unable to read");  //this header byte should be 124 for DDS image files

            DDS_HEADER header = FileHelper.BytesToStructure<DDS_HEADER>(ddsBytes);
            uint fourCC = header.pixelFormat.dwFourCC;
            //PLog.Info("DDPF_FOURCC: {0}", fourCC);

            TextureFormat textureFormat = TextureFormat.DXT1;
            if (fourCC == DXT.MAKEFOURCC('D', 'X', 'T', '1')) textureFormat = TextureFormat.DXT1;
            else if (fourCC == DXT.MAKEFOURCC('D', 'X', 'T', '5')) textureFormat = TextureFormat.DXT5;

            if (textureFormat != TextureFormat.DXT1 && textureFormat != TextureFormat.DXT5)
                throw new Exception("Invalid TextureFormat. Only DXT1 and DXT5 formats are supported by this method.");


            int height = ddsBytes[13] * 256 + ddsBytes[12];
            int width = ddsBytes[17] * 256 + ddsBytes[16];

            int DDS_HEADER_SIZE = 128;
            byte[] dxtBytes = new byte[ddsBytes.Length - DDS_HEADER_SIZE];
            Buffer.BlockCopy(ddsBytes, DDS_HEADER_SIZE, dxtBytes, 0, ddsBytes.Length - DDS_HEADER_SIZE);

            tex = new Texture2D(width, height, textureFormat, false);
            tex.filterMode = FilterMode.Trilinear;
            tex.LoadRawTextureData(dxtBytes);
            tex.Apply();
            tex.Compress(true);
        }

        private static bool checkHeader(byte[] data, byte[] magic, int offset = 0)
        {
            if (magic == null || data == null || magic.Length <= 0 || (data.Length - offset) <= 0 || magic.Length > (data.Length - offset))
            {
                SLog.Info("Bad arguments.");
                return false;
            }

            for (int i = 0; i < magic.Length; i++)
                if (data[i + offset] != magic[i])
                    return false;

            return true;
        }
    }
}
