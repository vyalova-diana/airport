using System;
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


namespace WebApiAirportNew.Controllers
{

        [Route("api/[controller]/[action]")]
        [ApiController]
        public class ScoreboardController : ControllerBase
        {
        private HttpClient Client { get; set; }
        IStorage Storage { get; } = new Storage();

        // GET Scoreboard/status
        //[HttpGet("status")]
        //public void PostStatus(int st, int id) => Storage.AddStatus(id, st);
        // GET Scoreboard/flights

        [HttpPost] //("flights")
        //[Route("Flights")]
        public ActionResult <List<Flight>> Flights([FromBody] List<Flight> fl)
        {
            return Ok(new List<Flight>());
            //HttpResponseMessage response = Client.GetAsync("api/scoreboard/Flights").Result;
            //if (response.IsSuccessStatusCode)
            //{
            //    HttpContent responseContent = response.Content;
            //    var json = responseContent.ReadAsStringAsync().Result;
            //    var flights = JsonConvert.DeserializeObject<List<Flight>>(json);

                
            //    foreach (var ms in flights)
            //    {
            //        Console.WriteLine(
            //            $"Id: {ms.Id}," +
            //            $"From: {ms.From}," +
            //            $"To: {ms.To}," +
            //            $"Time: {ms.Time}," +
            //            $"Status: {ms.Status}");
            //    }
            //    return flights;
            //}
            //else
            //    return null;

            //var client = new HttpClient();
            //client.BaseAddress = new Uri("http://localhost:6404");
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            ////var messages = JsonConvert.DeserializeObject<ValuesController[]>(body);
            //var result = client.GetAsync("scoreboard/flights").Result;
            //var body = result.Content.ReadAsStringAsync().Result;

            //var messages = JsonConvert.DeserializeObject<Message[]>(body);
            //foreach (var ms in messages)
            //{
            //    Console.WriteLine(
            //        $"Id: {ms.Id}," +
            //        $"From: {ms.From}," +
            //        $"To: {ms.To}," +
            //        $"Time: {ms.Time}," +
            //        $"Status: {ms.Status}");
            //}
        }

        private readonly Db _db;
        public ScoreboardController(Db db)
        {
            _db = db;
        }
        [HttpPost]
        public ActionResult FlightStatus([FromBody] (int idSt, int st) flightNum)
        {
            var flightSt = _db.flights.FindAll((fp) => fp.Id == flightNum.idSt);
            foreach (var bp in flightSt)
            {
                bp.Status = flightNum.st;
            }
            Console.WriteLine('1');
            return NoContent();
        }


    }
}