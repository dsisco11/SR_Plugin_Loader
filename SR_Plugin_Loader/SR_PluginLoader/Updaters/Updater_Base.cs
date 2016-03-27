using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace SR_PluginLoader
{
    public enum UPDATER_TYPE
    {
        NONE = 0,
        JSON,// 
        GIT,// Git system
        WEB// Simple web download
    }

    public enum FILE_UPDATE_STATUS
    {
        /// <summary>
        /// This file is perfectly up to date
        /// </summary>
        UP_TO_DATE=0,
        /// <summary>
        /// The file has an updated version available.
        /// </summary>
        OUT_OF_DATE,
        /// <summary>
        /// This file seems to be a developer compiled one, there is no history record for it. (mostly returned from github updaters)
        /// </summary>
        DEV_FILE
    }

    public delegate bool Updater_File_Type_Confirm(string ContentType);
    public delegate void Updater_File_Download_Progress(int read, int total_bytes);
    public delegate void Updater_File_Download_Completed(string filename);

    public abstract class Updater_Base
    {
        public static Updater_Base Get(UPDATER_TYPE ty)
        {
            switch (ty)
            {
                case UPDATER_TYPE.GIT:
                    return Git_Updater.instance;
                case UPDATER_TYPE.WEB:
                    return Web_Updater.instance;
            }

            return null;
        }
        public static readonly string USER_AGENT = "SR_Plugin_Loader  on GitHub";

        // This function is used by plugins, they pass their given update info url and the path to their current .dll file
        // How these are interpreted varies for each updater type.
        // The GIT updater actually searches the repository master branch and compares the current hash for the given file on the repo to the one on the users system
        // The JSON updater (when someone gets around to making it) will take a URL that leads to a JSON file containing the information for a single plugin, that json information should contain at the very LEAST: two fields (HASH, DOWNLOAD)
        //      HASH: is the git-format-sha1 hash for the currently released version of the plugin
        //      DOWNLOAD: is a direct URL link to the currently released version of the plugin DLL
        public virtual FILE_UPDATE_STATUS Get_Update_Status(string remote_file, string local_file)
        {
            return FILE_UPDATE_STATUS.DEV_FILE;
        }


        // Remember: when this function is used by plugins they will pass their given updater_method URL for 'remote_file'
        // In the case of things like the Git updater this is fine as that url will BE a link to the most current version of the plugin DLL
        // However in the case of the JSON updater that url will instead be a link to the JSON file containing the HASH and DOWNLOAD URL for said plugin.
        // So for the JSON updater this method needs to be overriden and made to download that JSON info and treat the DOWNLOAD url contained therein as if IT was passed as the 'remote_file'
        public virtual IEnumerator Download(string remote_file, string local_file, Updater_File_Type_Confirm confirm = null, Updater_File_Download_Progress prog_callback = null, Updater_File_Download_Completed download_completed = null)
        {
            DebugHud.LogSilent("Downloading: {0}", remote_file);
            if (local_file == null) local_file = String.Format("{0}\\{1}", UnityEngine.Application.dataPath, Path.GetFileName(remote_file));

            WebResponse resp = null;
            Stream stream = null;

            HttpWebRequest webRequest = WebRequest.Create(remote_file) as HttpWebRequest;
            webRequest.UserAgent = USER_AGENT;

            WebAsync webAsync = new WebAsync();
            IEnumerator e = webAsync.GetResponse(webRequest);
            while (e.MoveNext()) { yield return e.Current; }// wait for response to arrive
            while (!webAsync.isResponseCompleted) yield return null;// double check for clarity & safety

            RequestState result = webAsync.requestState;
            resp = result.webResponse;

            if (confirm != null)
            {
                if (confirm(resp.ContentType) == false)
                {
                    yield break;//exit routine
                }
            }

            stream = resp.GetResponseStream();
            int total = (int)resp.ContentLength;
            byte[] buf = new byte[total];
            const int CHUNK_SIZE = 2048;

            int read = 0;//how many bytes we have read so far (offset within the stream)
            int remain = total;//how many bytes are left to read
            int r = 0;

            while (remain > 0)
            {
                r = stream.Read(buf, read, Math.Min(remain, CHUNK_SIZE));
                read += r;
                remain -= r;
                if (prog_callback != null)
                {
                    try
                    {
                        prog_callback(read, total);
                    }
                    catch (Exception ex)
                    {
                        DebugHud.Log(ex);
                    }
                }
                yield return null;// yield execution until next frame
            }

            // It's good practice when overwriting files to write th enew version to a temporary location and then copy it overtop of the original.
            string temp_file = String.Format("{0}.temp", local_file);
            File.WriteAllBytes(temp_file, buf);
            File.Copy(temp_file, local_file, true);
            File.Delete(temp_file);

            if (download_completed != null) download_completed(local_file);
            yield break;//exit routine
        }
    }
}
