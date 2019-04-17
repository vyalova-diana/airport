using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using DataBase;

namespace WebApiTablo.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BoardController : ControllerBase
    {
        private HttpClient Client { get; set; }
        public Db db = new Db();
        

        [HttpPost("Fligts")]

        public void Flights([FromBody] List<Flight> fl)
        {
            Console.WriteLine("{0,6}   |{1,6}   |{2,6}   |{3,6}   |{4,6}   |{5,10}   |", "Flight", "From", "To", "Departure", "Arrival", "Status");
            Console.WriteLine("-----------------------------------------------------------------------------");
            foreach (var item in fl)
            {
                db.AddFlight(item);

                Console.WriteLine("{0,10}   |{1,10}   |{2,10}   |{3,10}   |{4,10}   |{5,10}", item.Id, item.From, item.To, item.TimeStart, item.TimeStop, item.TimeStop);
            }
            Console.WriteLine("Get schedule.");
        }

        //api/board/FlightStatus принимаю статус
        [HttpPost("FlightStatus/{idSt}/{st}")]
        public void FlightStatus(int idSt, int st)
        {

            var flightSt = db.flights.Find((fp) => fp.Id == idSt);
            if (flightSt != null)
            {
                var oldSt = flightSt.Status;
                flightSt.Status = st;
                if (st == 4)
                {
                    System.Threading.Thread.Sleep(5000);
                    var delreis = flightSt.Id;
                    db.DelFlight(flightSt);
                    Console.WriteLine("Flight " + delreis + "removed.");
                }
                Console.WriteLine("Flight status:" + idSt + "changed from " + oldSt + "to " + st);
            }

        }

        //api/board/GetFlights
        [HttpPost]
        public ActionResult<List<Flight>> GetFlights()
        {
            if (db.flights.Count != 0)
            {
                Console.WriteLine("List of flights sent.");
                return Ok(db.flights);
            }
            else
            {
                Console.WriteLine("There is no flight list");
                return NotFound();
            }

        }

        //api/board/SendStatus/{flightId}
        [HttpGet("{flightId}")] //("SendStatus/{flightId}")
        public ActionResult<int> SendStatus(int flightId)
        {

            var flightSt = db.flights.Find((fp) => fp.Id == flightId);

            if (flightSt != null)
            {
                Console.WriteLine("Flight: " + flightSt.Id + "Status: " + flightSt.Status + "sent.");
                return Ok(flightSt.Status);
            }
            else
            {
                Console.WriteLine("There is no flight " + flightId);
                return NotFound(-1);
            }

        }

        [Produces("application/json")]
        //api/board/ShowSchedule
        [HttpGet] //("ShowSchedule")
        public ActionResult<List<Flight>> ShowSchedule()
        {
            //var msg = new Flight()
            //{

            //    Id = 1,
            //    From = "Moscow",
            //    To = "Berlin",
            //    TimeStart = 10,
            //    TimeStop = 20,
            //    Status = 1
            //};

            //var msg2 = new Flight()
            //{
            //    Id = 2,
            //    From = "Moscow",
            //    To = "Ufa",
            //    TimeStart = 50,
            //    TimeStop = 2000,
            //    Status = 3
            //};
            //db.AddFlight(msg);
            //db.AddFlight(msg2);


            if (db.flights.Count != 0)
            {

                var htmlCode = "<table><tr><td>Reis    </td><td>From    </td><td>To         </td><td>TimeStart       </td><td>TimeStop       </td><td>Status    </td></tr>";
                foreach (var item in db.flights)
                {
                    int daysFrom = 0;
                    int hoursFrom = 0;
                    int minFrom = 0;
                    daysFrom = item.TimeStart / 1440 + 1;
                    hoursFrom = (item.TimeStart - 1440 * (daysFrom - 1)) / 60;
                    minFrom = item.TimeStart - 1440 * (daysFrom - 1) - 60 * hoursFrom;
                    int daysTo = 0;
                    int hoursTo = 0;
                    int minTo = 0;
                    daysTo = item.TimeStop / 1440 + 1;
                    hoursTo = (item.TimeStop - 1440 * (daysTo - 1)) / 60;
                    minTo = item.TimeStop - 1440 * (daysTo - 1) - 60 * hoursTo;
                    var convStatus = "";
                    if (item.Status == -1)
                    {
                        convStatus = "Flight not found.";
                    }

                    if (item.Status == 0)
                    {
                        convStatus = "Check in is pending.";
                    }

                    if (item.Status == 1)
                    {
                        convStatus = "Check in now.";
                    }
                    if (item.Status == 2)
                    {
                        convStatus = "Boarding.";
                    }
                    if (item.Status == 3)
                    {
                        convStatus = "Boarding is over.";
                    }
                    if (item.Status == 4)
                    {
                        convStatus = "Plane departured.";
                    }
                    if (item.Status == 5)
                    {
                        convStatus = "Plane arrived.";
                    }
                    htmlCode += "<tr><td>" + item.Id + "</td><td>" + item.From + "</td><td>" + item.To + "</td><td>" + "day " + daysFrom + " time " + hoursFrom + "." + minFrom + " </td><td>" + "day " + daysTo + " time " + hoursTo + "." + minTo + "</td><td>" + convStatus + " </td><td>";

                }
                htmlCode += "</table>";

                return new ContentResult
                {
                    ContentType = "text/html",
                    StatusCode = (int)HttpStatusCode.OK,
                    Content = htmlCode
                };
            }
            else
            {
                Console.WriteLine("There is no flight list!");
                return Ok();
            }
        }

    }
}