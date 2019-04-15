﻿using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using DataBase;
using System.Text;

namespace WebApiAirportNew.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BoardController : ControllerBase
    {
        private HttpClient Client { get; set; }


        public Db db;

        [HttpPost("Fligts")] 

        public void Flights([FromBody] List<Flight> fl)
        {
            Console.WriteLine("{0,10}   |{1,10}   |{2,10}   |{3,10}   |{4,10}   |{5,10}", "Рейс", "Откуда", "Куда", "Время вылета", "Время прилета", "Статус");
            Console.WriteLine("---------------------------------------------------------------------------");
            foreach (var item in fl)
            {
                db.AddFlight(item);
                Console.WriteLine("{0,10}   |{1,10}   |{2,10}   |{3,10}   |{4,10}   |{5,10}", item.Id, item.From, item.To, item.TimeStart, item.TimeStop, item.TimeStop);

            }

            Console.WriteLine("1");

            //return Ok(new List<Flight>());
            //HttpResponseMessage response = Client.GetAsync("api/board/Flights").Result;
            //if (response.IsSuccessStatusCode)
            //{
            //    HttpContent responseContent = response.Content;
            //    var json = responseContent.ReadAsStringAsync().Result;
            //    var flights = JsonConvert.DeserializeObject<List<Flight>>(json);
            //    db.flights = fl;

            //    foreach (var ms in flights)
            //    {
            //        Console.WriteLine(
            //            $"Id: {ms.Id}," +
            //            $"From: {ms.From}," +
            //            $"To: {ms.To}," +
            //            $"Time: {ms.Time}," +
            //            $"Status: {ms.Status}");
            //    }
            //    return NoContent();
            //}
            //else
            //    return null;
        }
        //api/board/SendStatus
        [HttpPost("FlightStatus")]
        public void FlightStatus([FromBody] (int idSt, int st) flightNum)
        {

            var flightSt = db.flights.Find((fp) => fp.Id == flightNum.idSt);
            if (flightSt != null)
            {
                flightSt.Status = flightNum.st;
            }

        }
        //api/board/SendStatus/{flightId}
        [HttpGet("{flightId}")] //("SendStatus/{flightId}")
        public ActionResult<int> SendStatus(int flightId)
        {
            
            var flightSt = db.flights.Find((fp) => fp.Id == flightId);
            
            if (flightSt != null)
            {
                return Ok(flightSt.Status);
            }
            else
                return NotFound(-1);
   
        }
    }
}
//var client = new HttpClient();
//client.BaseAddress = new Uri("http://localhost:/");//Данин
//client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
//var stringContent = new StringContent(JsonConvert.SerializeObject(st), Encoding.UTF8, "application/json");
//HttpResponseMessage response = Client.PostAsync("api/values/3", stringContent).Result;