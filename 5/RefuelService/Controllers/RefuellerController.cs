using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RefuelBackend;
using System.Threading;

namespace RefuelService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RefuellerController : ControllerBase
    {
        // GET: Refueller/status
        [HttpGet("status")]
        public string Get()
        {
            StatusManager sm = new StatusManager();
            return sm.GetStatus().ToString();
        }

        // POST: Refueller
        [HttpPost]
        public int Post(RefuelRequest newreqv)
        {
            //calculate time
            //request permission
            //go to refuelling
            //refuel
            //requset permission
            //return to base

            //return newreqv.planeID;

            RefuelTime rt = new RefuelTime();
            var sleeptime = rt.CountTime(newreqv.fuelNeeded);
            StatusManager sm = new StatusManager();
            var status = sm.GetStatus();

            string from = default(String);

            if (status == 0)
            {
                from = "'from': 'Garage', ";
            }
            else if (status == 1)
            {
                from = "'from': 'Gate', ";
            }
            var temp = newreqv.planeID.ToString();
            string to = "'to': '" + temp + "', ";


            string json = "{'service': 'Refuelling', 'identifier': '1'}";
            var host = "http://localhost:4567/"; //???
            var reqv = host + "/" + from + to + json;
            string str = null;

            var req = new StreamReader(HttpWebRequest.Create(reqv).GetResponse().GetResponseStream());
            str = req.ReadToEnd();
            if (str.Equals("Queued"))
            {
                //something
            }

            else if (str.Equals("Granted"))
            {
                sm.SaveStatus(2);
                Thread.Sleep(10000);
                sm.SaveStatus(3);
                Thread.Sleep(sleeptime);
                req = new StreamReader(HttpWebRequest.Create(reqv).GetResponse().GetResponseStream());
                str = req.ReadToEnd();

                if (str.Equals("Queued"))
                {
                    //something
                }

                else if (str.Equals("Granted"))
                {
                    sm.SaveStatus(2);
                    Thread.Sleep(10000);
                    sm.SaveStatus(0);
                }
            }
        }
    }
}
