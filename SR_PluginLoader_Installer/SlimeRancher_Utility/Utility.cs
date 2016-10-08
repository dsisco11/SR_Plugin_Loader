using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SlimeRancher
{
    public static class Util
    {

        public static bool IsLinux
        {
            get
            {
                int p = (int)Environment.OSVersion.Platform;
                return (p == 4) || (p == 6) || (p == 128);
            }
        }

        public static bool IsWindows
        {
            get
            {
                PlatformID p = Environment.OSVersion.Platform;
                return (p == PlatformID.Win32NT || p == PlatformID.Win32S || p == PlatformID.Win32Windows || p == PlatformID.WinCE);
            }
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
            byte[] hash = sha.ComputeHash(enc.GetBytes(head));

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }

            string hash_foobar = "323fae03f4606ea9991df8befbb2fca795e648fa";// Correct GIT hash for a file containing only "foobar\n"
            bool match = (String.Compare(sb.ToString(), hash_foobar) == 0);
            //DebugHud.Log("[SHA1 HASH TEST] Match<{0}>  Hash: {1}  HEAD: '{2}'", (match?"TRUE":"FALSE"), sb.ToString(), head);

            return sb.ToString();
        }

    }
}
