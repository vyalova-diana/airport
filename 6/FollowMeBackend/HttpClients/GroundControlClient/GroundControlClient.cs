using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace FollowMeBackend.HttpClients.GroundControlClient
{
    public class GroundControlClient
    {
        public static StatusUpdateResponse StatusUpdate(Status stat)
        {
            var stringContent = new StringContent(JsonConvert.SerializeObject(stat), Encoding.UTF8, "application/json");
            var host = "http://localhost:****/updateTFStatus/" + stringContent;
            string str = null;
            var req = new StreamReader(WebRequest.Create(host).GetResponse().GetResponseStream());
            str = req.ReadToEnd();
            var jdata = JsonConvert.DeserializeObject<StatusUpdateResponse>(str);

            return jdata;
        }
        public static AirplanePositionResponse FindAirplane(LocateAirplaneRequest r)
        {

            var stringContent = new StringContent(JsonConvert.SerializeObject(r), Encoding.UTF8, "application/json");
            var host = "http://localhost:****/getTFInformation/" + stringContent;
            string str = null;
            var req = new StreamReader(WebRequest.Create(host).GetResponse().GetResponseStream());
            str = req.ReadToEnd();
            var jdata = JsonConvert.DeserializeObject<AirplanePositionResponse>(str);

            return jdata;
        }
        public static PermissionResponse AskPermission(PermissionRequest r)
        {

            var stringContent = new StringContent(JsonConvert.SerializeObject(r), Encoding.UTF8, "application/json");
            var host = "http://localhost:****/askForPermission/" + stringContent;
            string str = null;
            var req = new StreamReader(WebRequest.Create(host).GetResponse().GetResponseStream());
            str = req.ReadToEnd();
            var jdata = JsonConvert.DeserializeObject<PermissionResponse>(str);

            return jdata;
        }

    }
}
