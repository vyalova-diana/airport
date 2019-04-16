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
            try
            {
                var a = Vehicle.Instance.GetVehicleStatus();
                return a;
            }
            catch
            {
                return "error";
            }

        }

        [HttpGet("invoke")]
        public int Invoke()
        {
            try
            {
                FileManager.Instance.Set("3", "../controllerStatus.txt", true);
                return 0;
            }
            catch
            {
                return 1;
            }
        }

        // POST: Refueller
        [HttpPost]
        public int Post(RefuelRequest newreqv)
        {
            try
            {
                if (Vehicle.Instance.GetVehicleStatus().Equals("0"))
                {
                    string toStatus = "2" + " " + newreqv.planeID.ToString() + " " + newreqv.fuelNeeded.ToString();
                    FileManager.Instance.Set(toStatus, "../controllerStatus.txt", true);
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            catch
            {
                return 1;
            }
        }
    }
}
