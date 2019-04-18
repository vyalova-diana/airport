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

           var req = (HttpWebRequest) WebRequest.Create("https://groundcontrol.v2.vapor.cloud/updateTFStatus/");
            req.ContentType = "application/json";
            req.Method = "POST";
            var SW = new StreamWriter(req.GetRequestStream());

            var stringContent = JsonConvert.SerializeObject(stat);
            SW.Write(stringContent);
            SW.Flush();
            SW.Close();

            var resp= (HttpWebResponse)req.GetResponse();
            var SR = new StreamReader(resp.GetResponseStream());
            
            var jdata = JsonConvert.DeserializeObject<StatusUpdateResponse>(SR.ReadToEnd());

            return jdata;
        }
        public static AirplanePositionResponse FindAirplane(LocateAirplaneRequest r)
        {
            var req = (HttpWebRequest)WebRequest.Create("https://groundcontrol.v2.vapor.cloud/getTFInformation/");
            req.ContentType = "application/json";
            req.Method = "POST";
            var SW = new StreamWriter(req.GetRequestStream());

            var stringContent = JsonConvert.SerializeObject(r);
            SW.Write(stringContent);
            SW.Flush();
            SW.Close();

            var resp = (HttpWebResponse)req.GetResponse();
            var SR = new StreamReader(resp.GetResponseStream());

            
            var jdata = JsonConvert.DeserializeObject<AirplanePositionResponse>(SR.ReadToEnd());

            return jdata;
        }
        public static PermissionResponse AskPermission(PermissionRequest r)
        {
            var req = (HttpWebRequest)WebRequest.Create("https://groundcontrol.v2.vapor.cloud/askForPermission/");
            req.ContentType = "application/json";
            req.Method = "POST";
            var SW = new StreamWriter(req.GetRequestStream());


            var stringContent = JsonConvert.SerializeObject(r);
            SW.Write(stringContent);
            SW.Flush();
            SW.Close();

            var resp = (HttpWebResponse)req.GetResponse();
            var SR = new StreamReader(resp.GetResponseStream());

           
            var jdata = JsonConvert.DeserializeObject<PermissionResponse>(SR.ReadToEnd());

            return jdata;
        }

    }
}
