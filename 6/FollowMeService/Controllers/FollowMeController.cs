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
    [Route("[controller]")]
   
    public class FollowMeController : ControllerBase
    {
        [HttpGet]
        [Route("{planeID}")]
        public string AddTask(string planeID)
        {
            Console.WriteLine("ADDTask {0}", planeID);
            try
            {
                if (Vehicle.Instance.GetVehicleStatus().status == "Idle")
                {
                    if (planeID != "" && planeID != null && planeID != " ")
                    {
                        var cur = FileManager.Instance.Get(-1, "../controllerStatus.txt");
                        if (cur == "empty line")
                        {
                            FileManager.Instance.SetStraight(planeID, "../controllerStatus.txt", true); //перезаписываем на новую задачу

                        }
                        else
                        {
                            FileManager.Instance.Set(planeID, "../controllerStatus.txt", true); //добавляем новую задачу
                        }
                        return "1";
                    }
                    else
                    {
                        return "0";
                    }

                }
                else
                {
                    return "0";
                }

                //if (Vehicle.Instance.GetVehicleStatus().status=="Idle")
                //{
                //    FileManager.Instance.Set(planeID, "../controllerStatus.txt", true); //добавляем новую задачу
                //    return "1";
                //}
                //else
                //{
                //    return "0";
                //}


            }
            catch
            {

                return "0";
            }
        }

        [Route("status")]
        
        public string Get()
        {
            return JsonConvert.SerializeObject(Vehicle.Instance.GetVehicleStatus());
        }


      
    }
}