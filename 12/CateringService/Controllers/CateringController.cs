using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using CateringDB;

namespace CateringService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CateringController : ControllerBase
    {
        // POST: Catering
        [HttpPost("Catering")]
        public int Post([FromBody] int planeId)
        {
            List<int> FoodCount = new List<int>();
            Food food = new Food();
            food.planeID = planeId;

            //получить количество минут полета номер рейса от Наташи
            HttpClient httpClient1 = new HttpClient();
            httpClient1.BaseAddress = new Uri("https://localhost:61120/");
            httpClient1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response1 = httpClient1.GetAsync("GetTime/" + planeId).Result;

            if (response1.IsSuccessStatusCode)
            {
                HttpContent responseContent1 = response1.Content;
                var json1 = responseContent1.ReadAsStringAsync().Result;
                var rs1 = JsonConvert.DeserializeObject<string>(json1);

                string[] str = rs1.Split(new char[] { ';' });

                food.timeFlying = Convert.ToInt32(str[0]);
                food.flightID = Convert.ToInt32(str[1]);

                if (food.timeFlying == -1 || food.timeFlying == 0)
                {
                    return 1;
                }

                //получить два числа количество еды ( Normal - 0 , Vegan - 1 ) от Леры
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri("https://localhost:7009/");
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = httpClient.GetAsync("get/food/" + food.flightID).Result;

                if (response.IsSuccessStatusCode)
                {
                    HttpContent responseContent = response.Content;
                    var json = responseContent.ReadAsStringAsync().Result;
                    var rs = JsonConvert.DeserializeObject<List<int>>(json);

                    food.countNormal = rs.First();
                    food.countVegan = rs.Last();

                    if (food.timeFlying >= 360)
                    {
                        food.countNormal *= 2;
                        food.countVegan *= 2;
                    }

                    if (Car.Instance.GetCarStatus().Equals("0"))
                    {
                        string toStatus = "2" + " " + planeId.ToString() + " " + food.countNormal + " " + food.countVegan;
                        FileStorage.Instance.Set(toStatus, "../controllerStatus.txt", true);

                        return 0;
                    }
                    else
                    {
                        return 1;
                    }
                }
                else return 1;
            }
            else return 1;
        }

        // GET: Catering/status
        [HttpGet("status")]
        public string Get()
        {
            try
            {
                var status = Car.Instance.GetCarStatus();
                return status;
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
                FileStorage.Instance.Set("3", "../controllerStatus.txt", true);
                return 0;
            }
            catch
            {
                return 1;
            }
        }


    }
}
