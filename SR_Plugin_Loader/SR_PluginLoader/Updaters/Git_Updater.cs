using SimpleJSON;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Text.RegularExpressions;

namespace SR_PluginLoader
{
    public class Git_Updater : Updater_Base
    {
        private static readonly Git_Updater _instance = new Git_Updater();
        public static Git_Updater instance { get { return _instance; } }
        public static readonly UPDATER_TYPE type = UPDATER_TYPE.GIT;

        private static WebClient _webClient = null;
        private static WebClient webClient { get { if (_webClient == null) { _webClient = GetClient(); } return _webClient; }  }
        private static JSONArray repo_cache = null;
        private static Dictionary<string, byte[]> remote_file_cache = new Dictionary<string, byte[]>();

        private bool host_is_github(string host)
        {
            var reg = new Regex(@"^\w+\.github\.com$");
            Match match = reg.Match(host);
            return match.Success;
        }

        private string Extract_File_Path_From_Github_URL(string url)
        {
            var uri = new Uri(url);
            if (!host_is_github(uri.Host)) return url;

            var reg = new Regex(@"^\w+/\w+/\w+/\w+/(.+)$");
            Match match = reg.Match(uri.AbsolutePath);
            if (match.Success) return match.Groups[1].Value;

            return url;
        }

        private string Extract_Repository_URL_From_Github_URL(string url)
        {
            var uri = new Uri(url);
            if (!host_is_github(uri.Host)) return url;
            
            var reg = new Regex(@"^(/\w+/\w+)/.+$");
            Match match = reg.Match(uri.AbsolutePath);

            string result = url;
            if (match.Success) result = String.Concat("https://api.github.com/repos", match.Groups[1].Value);

            return result;
        }

        private static WebClient GetClient()
        {
            // Add a handler for SSL certs because mono doesnt have any trusted ones by default
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback((sender, certificate, chain, policyErrors) => { return true; });
            WebClient client = new WebClient();
            // Add a useragent string so GitHub doesnt return 403 and also so they can have a chat if they like.
            client.Headers.Add(HttpRequestHeader.UserAgent, USER_AGENT);

            return client;
        }

        private static JSONArray Cache_Git_Repo(string repo_url)
        {
            if (repo_cache != null) return repo_cache;
            try
            {
                //  https://api.github.com/repos/dsisco11/SR_Plugin_Loader/git/trees/master?recursive=1
                string url = String.Format("{0}/git/trees/master?recursive=1", repo_url.TrimEnd(new char[] { '\\', '/' }));
                
                byte[] buf = null;
                string jsonStr = null;

                if (remote_file_cache.TryGetValue(url, out buf))
                {
                    jsonStr = Encoding.ASCII.GetString(buf);
                }
                else
                {
                    // Fetch repo information
                    jsonStr = webClient.DownloadString(url);
                    if (jsonStr == null || jsonStr.Length <= 0)
                    {
                        return null;
                    }
                    remote_file_cache[url] = Encoding.ASCII.GetBytes(jsonStr);
                    DebugHud.LogSilent("Cached repository: {0}", repo_url);
                }

                // Parse the json response from GitHub
                var git = SimpleJSON.JSON.Parse(jsonStr);
                var tree = git["tree"].AsArray;
                repo_cache = tree;

                return tree;
            }
            catch(WebException ex)
            {
                DebugHud.Log(ex);
            }

            return null;
        }

        public override FILE_UPDATE_STATUS Get_Update_Status(string remote_path, string local_file)
        {
            if (!File.Exists(local_file)) return FILE_UPDATE_STATUS.OUT_OF_DATE;

            string repo_url = Extract_Repository_URL_From_Github_URL(remote_path);
            string remote_file = Extract_File_Path_From_Github_URL(remote_path);
            
            // Time to check with GitHub and see if there is a newer version of the plugin loader out!
            try
            {
                var repo = Cache_Git_Repo(repo_url);
                if (repo == null)
                {
                    DebugHud.Log("[AutoUpdater] Unable to cache git repository!");
                    return FILE_UPDATE_STATUS.UP_TO_DATE;
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

                            // Check if the file for the currently installed loaders sha1 hash exists.
                            bool exist = false;
                            try
                            {
                                exist = Query_Remote_File_Exists(tmpurl);
                            }
                            catch(WebException wex)
                            {
                                if(wex.Status == WebExceptionStatus.ProtocolError)
                                {
                                    var response = wex.Response as HttpWebResponse;
                                    if (response != null)
                                    {
                                        if (response.StatusCode == HttpStatusCode.NotFound)// A file for this hash does not exhist on the github repo. So this must be a Dev version.
                                        {
                                            exist = false;
                                        }
                                    }
                                }
                            }

                            if(!exist)
                            {
                                //DebugHud.Log("[Updater] Dev file: {0}", Path.GetFileName(local_file));
                                return FILE_UPDATE_STATUS.DEV_FILE;
                            }

                            //DebugHud.Log("[Updater] Outdated file: {0}", Path.GetFileName(local_file));
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

        public FileStream Cache_And_Open_File(string remote_file)
        {
            string repo_url = Extract_Repository_URL_From_Github_URL(remote_file);
            var repo = Cache_Git_Repo(repo_url);
            string local_file = String.Format("{0}\\{1}", UnityEngine.Application.dataPath, Path.GetFileName(remote_file));
            var update_status = Get_Update_Status(local_file, remote_file);
            if (update_status == FILE_UPDATE_STATUS.OUT_OF_DATE)
            {
                byte[] data = webClient.DownloadData(remote_file);
                File.WriteAllBytes(local_file, data);
            }

            return File.OpenRead(local_file);
        }

        /*
        public override System.Collections.IEnumerator Download(string remote_file, string local_file, Updater_File_Download_Progress cb = null)
        {
            if (local_file == null) local_file = String.Format("{0}\\{1}", UnityEngine.Application.dataPath, Path.GetFileName(remote_file));

            byte[] data = webClient.DownloadData(remote_file);
            File.WriteAllBytes(local_file, data);

            yield return null;
        }
        */

        /// <summary>
        /// Checks if a specified file exists in the project GIT repository.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool Query_Remote_File_Exists(string url)
        {
            HttpWebResponse response = null;
            var request = (HttpWebRequest)WebRequest.Create(url);
            if(request == null)
            {
                DebugHud.Log("Unable to create an instance of HttpWebRequest!");
                return false;
            }
            request.UserAgent = USER_AGENT;
            request.Method = "HEAD";

            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException wex)
            {
                if (wex.Status == WebExceptionStatus.ProtocolError)
                {
                    var wres = wex.Response as HttpWebResponse;
                    if (wres != null)
                    {
                        if (wres.StatusCode == HttpStatusCode.NotFound)// A file for this hash does not exhist on the github repo. So this must be a Dev version.
                        {
                            return false;
                        }
                    }
                }

                DebugHud.Log(wex);
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
