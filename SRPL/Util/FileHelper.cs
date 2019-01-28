using System;
using System.IO;
using System.Runtime.InteropServices;

namespace SRPL.Util
{
    public static class FileHelper
    {
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
    }
}
