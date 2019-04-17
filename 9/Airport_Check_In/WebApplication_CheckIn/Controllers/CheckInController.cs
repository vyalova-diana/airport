using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CheckInDataBase;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;

namespace WebApplication_CheckIn.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CheckInController : ControllerBase
    {
        public List<Food> FoodList = new List<Food>();
        public List<Baggage> BaggageList = new List<Baggage>();

        private HttpClient Client { get; set; }

        [HttpGet("CheckInStatus/{idSt}/{st}")]
        public void CheckInStatusStatus(int idSt, int st)
        {
          
                CheckInStatus newStatus = new CheckInStatus();
                newStatus.FlightNumber = idSt;
                if(st == 2)
                {
                
                newStatus.Status = CheckIn_Status.Finish;
                Console.WriteLine("9: Check in on flight number {0} is over", idSt);
            }
                else
                {
                    newStatus.Status = CheckIn_Status.Start;
                Console.WriteLine("9: Check in on flight number {0}  has begun", idSt);
            }
                CheckInSchedule.getInstance().ChangeStatus(newStatus);
            Console.WriteLine("9:  flight status changeg");

        }

        [HttpPost]
        public ActionResult PostPassenger([FromBody] Passenger passenger)
        {
            int r = CheckInSchedule.getInstance().find(passenger.Ticket.fID);
            if (r == 0)
            {
                Console.WriteLine("9:  flight number {0} not found", passenger.Ticket.fID);
                return NotFound();
            }
            else
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri("https://localhost:44304/");
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = httpClient.GetAsync("get/" + passenger.Ticket.pID).Result;

                if (response.IsSuccessStatusCode)
                {
                    HttpContent responseContent = response.Content;
                    var json = responseContent.ReadAsStringAsync().Result;
                    var rs = JsonConvert.DeserializeObject<string>(json);

                    int f = 0;
                    if (rs != "0")
                    {
                        if (passenger.BaggageWeight <= 25)
                        {
                            Baggage baggage = new Baggage();
                            baggage.FlightNumber = passenger.Ticket.fID;
                            baggage.PassengerId = passenger.Ticket.pID;
                            baggage.BaggageWeight = passenger.BaggageWeight;
                            BaggageList.Add(baggage);

                            foreach (var i in FoodList)
                            {
                                if (passenger.Ticket.fID == i.FlightNumber)
                                {
                                    f = 1;
                                    if (passenger.TypeOfFood == TypeOfFood.Normal)
                                    {
                                        i.Normal++;
                                    }
                                    else i.Vegan++;
                                }
                            }
                            if (f == 0)
                            {
                                Food food = new Food();
                                food.FlightNumber = passenger.Ticket.fID;
                                if (passenger.TypeOfFood == TypeOfFood.Normal)
                                {
                                    food.Normal++;
                                }
                                else food.Vegan++;
                                FoodList.Add(food);

                            }

                            Console.WriteLine("9: passenger number {0} is registered for flight number {1} ",passenger.Ticket.pID, passenger.Ticket.fID);
                            return NoContent();
                        }
                        else
                        {
                            Console.WriteLine("9: baggage overweight");
                            return StatusCode(403); //перевес багажа
                        }
                    }
                    else
                    {
                        Console.WriteLine("9: passenger {0} : ticket not found ", passenger.Ticket.pID);
                        return NotFound();
                    }

                }
                else
                { 
                Console.WriteLine("9: passenger {0} : ticket not found ", passenger.Ticket.pID);
                return NotFound();
                }
            }
        }

      

        [HttpGet("GetFood/{number}")]
       public string GetFood(int number) // возвращаем 2 числа количество ( Normal - 0 , Vegan - 1 )
        {
            List<int> FoodCount = new List<int>();
            int Normal = 0;
            int Vegan = 0;
            var flight = FoodList.Where(x => x.FlightNumber == number).First();
            if (flight != null)
            {
                Normal = flight.Normal;
                Vegan = flight.Vegan;
                FoodCount.Add(Normal);
                FoodCount.Add(Vegan);
                var json = JsonConvert.SerializeObject(FoodCount);
                Console.WriteLine("9: food list for flight number {0} sent", number);
                return json;
            }
            else
            {
                Console.WriteLine("9: food list for flight number {0} not sent", number);
                return "0";
            }

        }

        [HttpGet("GetBaggage/{number}")]
       // [Route("get/baggage/{number}")] //номер рейса 
        public string GetBaggage(int number) // возвращаем список багажа
        {
            List<Baggage> baggage = new List<Baggage>();
            foreach (var b in BaggageList)
            {
                if (b.FlightNumber == number)
                    baggage.Add(b);
            }
            if (baggage.Count() != 0)
            {
                var json = JsonConvert.SerializeObject(baggage);
                Console.WriteLine("9: baggage list for flight number {0} sent", number);
                return json;
                
            }
            else
            { 
                Console.WriteLine("9: baggage list for flight number {0} not sent", number);
                return "0";
            }

        }


    }
}