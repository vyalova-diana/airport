using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace FollowMeBackend.HttpClients.MaintenanceClient
{
    class MaintenanceClient
    {
        public static void ReportStatus(Guid id)
        {
           
            var host = "http://localhost:1488/cargos/reportStatus/" + id.ToString();
            //string str = null;
            var req = new StreamReader(WebRequest.Create(host).GetResponse().GetResponseStream());
            //str = req.ReadToEnd();
            //var jdata = JsonConvert.DeserializeObject<StatusUpdateResponse>(str);

            
            
            
            
        }
    }
}
