using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Text;

namespace SR_PluginLoader
{
    public class Git_Updater : Updater_Base
    {
        private static readonly Git_Updater _instance = new Git_Updater();
        public static Git_Updater instance { get { return _instance;  } }
        public static readonly UPDATER_TYPE type = UPDATER_TYPE.GIT;

        private static WebClient web = new WebClient();
        private static JSONArray repo_cache = null;

        private static JSONArray Cache_Git_Repo()
        {
            if (repo_cache != null) return repo_cache;

            // Add a useragent string so GitHub doesnt return 403 and also so they can have a chat if they like.
            web.Headers.Add("user-agent", USER_AGENT);
            // Add a handler for SSL certs because mono doesnt have any trusted ones by default
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback((sender, certificate, chain, policyErrors) => { return true; });

            // Fetch repo information
            string url = "https://api.github.com/repos/dsisco11/SR_Plugin_Loader/git/trees/master?recursive=1";
            string jsonStr = web.DownloadString(url);
            if (jsonStr == null || jsonStr.Length <= 0)
            {
                return null;
            }

            // Parse the json response from GitHub
            var git = SimpleJSON.JSON.Parse(jsonStr);
            var tree = git["tree"].AsArray;
            repo_cache = tree;

            return tree;
        }

        public FILE_UPDATE_STATUS Get_Update_Status(string local_file, string remote_file)
        {
            // Time to check with GitHub and see if there is a newer version of the plugin loader out!
            try
            {
                var repo = Cache_Git_Repo();
                if (repo == null)
                {
                    DebugHud.Log("[AutoUpdater] Unable to cache git repository!");
                }

                // Find the plugin loaders DLL installation file
                foreach (JSONNode file in repo)
                {
                    if (String.Compare(file["path"], remote_file) == 0)
                    {
                        // Compare the SHA1 hash for the dll on GitHub to the hash for the one currently installed
                        string nSHA = file["sha"];
                        string cSHA = Utility.Get_File_Sha1(local_file);

                        if (String.Compare(nSHA, cSHA) != 0)
                        {
                            // ok they don't match, now let's just make double sure that it isn't because we are using an unreleased developer version
                            // First find the url for the file on GitHub
                            string tmpurl = file["url"];
                            // now we want to replace it's hash with ours and check if it exists!
                            tmpurl = tmpurl.Replace(nSHA, cSHA);

                            // Try and download the file info for the currently installed loaders sha1 hash
                            try
                            {
                                Check_Git_File(tmpurl);
                            }
                            catch
                            {
                                // A file for this hash does not exhist on the github repo. So this must be a Dev version.
                                return FILE_UPDATE_STATUS.DEV_FILE;
                            }

                            return FILE_UPDATE_STATUS.OUT_OF_DATE;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DebugHud.Log(ex);
            }
            return FILE_UPDATE_STATUS.UP_TO_DATE;//no update
        }

        public static bool Check_Git_File(string url)
        {
            HttpWebResponse response = null;
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers[HttpRequestHeader.UserAgent] = USER_AGENT;
            request.Method = "HEAD";

            DebugHud.Log("Querying: {0}", url);

            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                DebugHud.Log(ex);
                return false;
            }
            finally
            {
                if (response != null) response.Close();
            }

            return true;
        }
    }
}
