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
        public static string AreYouReady(string id)
        {
            var host = "http://localhost:7014/plane/ready_followme/" + id.ToString();
            string str = null;
            var req = new StreamReader(HttpWebRequest.Create(host).GetResponse().GetResponseStream());
            str = req.ReadToEnd();
            return str;
        }
    }
}
