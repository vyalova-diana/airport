using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Net;

namespace TicketWindow.Models
{
    public class ScheduleHttpClient
    {
        public static string ScheduleRequest(int flightID)
        {
            var host = "http://localhost:61120/Schedule/" + flightID.ToString();
            string str = null;
            var req = new StreamReader(HttpWebRequest.Create(host).GetResponse().GetResponseStream());
            str = req.ReadToEnd();
            return str;
        }
    }
}
