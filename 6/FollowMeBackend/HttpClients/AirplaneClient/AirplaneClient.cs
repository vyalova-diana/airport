using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace FollowMeBackend.HttpClients
{
    public class AirplaneClient
    {
        public static string StatusUpdate(string id,string status)
        {
            var req = (HttpWebRequest)WebRequest.Create("http://localhost:7014/planes/update_status/");
            req.ContentType = "application/json";
            req.Method = "GET";
            var SW = new StreamWriter(req.GetRequestStream());

            var stringContent = id.ToString() + "/" + status.ToString();
            SW.Write(stringContent);
            SW.Flush();
            SW.Close();

            var resp = (HttpWebResponse)req.GetResponse();
            var SR = new StreamReader(resp.GetResponseStream());

            var jdata = SR.ReadToEnd();

            return jdata;

           
        }
        public static string GetStatus(string id)
        {
            var req = (HttpWebRequest)WebRequest.Create("http://localhost:7014/planes/");
            req.ContentType = "application/json";
            req.Method = "GET";
            var SW = new StreamWriter(req.GetRequestStream());

            var stringContent = id.ToString();
            SW.Write(stringContent);
            SW.Flush();
            SW.Close();

            var resp = (HttpWebResponse)req.GetResponse();
            var SR = new StreamReader(resp.GetResponseStream());

            var jdata = JsonConvert.DeserializeObject<AirplaneStatusResp>(SR.ReadToEnd());

            
            return jdata.Status;
            
        }
        public static string IsFollowing(string id)
        {
            var req = (HttpWebRequest)WebRequest.Create("http://localhost:7014//planes/is_following/");
            req.ContentType = "application/json";
            req.Method = "GET";
            var SW = new StreamWriter(req.GetRequestStream());

            var stringContent = id.ToString();
            SW.Write(stringContent);
            SW.Flush();
            SW.Close();

            var resp = (HttpWebResponse)req.GetResponse();
            var SR = new StreamReader(resp.GetResponseStream());

            var jdata = SR.ReadToEnd();

            return jdata;

           

        }
    }
}
