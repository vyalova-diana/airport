using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FollowMeBackend;
using Newtonsoft.Json;

namespace FollowMeService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowMeController : ControllerBase
    {
        
        [HttpGet("status")]
        public string Get()
        {
            return JsonConvert.SerializeObject(Vehicle.Instance.GetVehicleStatus());
        }

        
        [HttpPost]
        public string Post(string planeID)
        {
            try
            {
                
                FileManager.Instance.Set(planeID, "../controllerStatus.txt", true); //добавляем новую задачу
                
                return "1";
            }
            catch (Exception ex)
            {
                
                return "0";
            }
        }
    }
}