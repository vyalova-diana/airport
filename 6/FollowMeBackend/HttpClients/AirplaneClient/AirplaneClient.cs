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
            var host = "http://localhost:7014/planes/update_status/" + id.ToString()+"/"+status.ToString();
            string str = null;
            var req = new StreamReader(HttpWebRequest.Create(host).GetResponse().GetResponseStream());
            str = req.ReadToEnd();
            return str;
        }
        public static string GetStatus(string id)
        {
            var host = "http://localhost:7014/planes/" + id.ToString();
            string str = null;
            var req = new StreamReader(HttpWebRequest.Create(host).GetResponse().GetResponseStream());
            str = req.ReadToEnd();
            var jdata = JsonConvert.DeserializeObject<AirplaneStatusResp>(str);
            return jdata.Status;
            
        }
        public static string IsFollowing(string id)
        {
            var host = "http://localhost:7014//planes/is_following/" + id.ToString();
            string str = null;
            var req = new StreamReader(HttpWebRequest.Create(host).GetResponse().GetResponseStream());
            str = req.ReadToEnd();

            return str;

        }
    }
}
