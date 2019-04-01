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
            return Vehicle.Instance.GetVehicleStatus();
        }

        // POST: Refueller
        [HttpPost]
        public string Post(RefuelRequest newreqv)
        {
            try
            {
                string toStatus = "2" + " " + newreqv.planeID.ToString() + " " + newreqv.fuelNeeded.ToString();
                FileManager.Instance.Set(toStatus, "../controllerStatus.txt", true);
                return toStatus;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
