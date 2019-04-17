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
    [Route("api/[controller]")]
    [ApiController]
    public class CheckInController : ControllerBase
    {
        public List<Food> FoodList { get; set; }
        public List<Baggage> BaggageList { get; set; }

        private HttpClient Client { get; set; }
        [HttpPost]
        public string Post(Passenger passenger)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:44304");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var stringContent = new StringContent(JsonConvert.SerializeObject(passenger.Ticket.pID), Encoding.UTF8, "application/json");
            var result = client.PostAsync("api/<controller>/5", stringContent).Result;
            // ?
            int f = 0;
            if (result.ToString() != "0")
            {
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
                if (f == 0 )
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

                Baggage baggage = new Baggage();
                baggage.FlightNumber = passenger.Ticket.fID;
                baggage.PassengerId = passenger.Ticket.pID;
                baggage.BaggageWeight = passenger.BaggageWeight;
                BaggageList.Add(baggage);
                return "1";
            }
            else
            {
                return "0";
            }

        }

        //public List<Flight> GetFlights([FromBody] (Guid flId, Guid flFrom, Guid flTo, Guid flTime, Guid flStatus) fl)
        //{
        //    HttpResponseMessage response = Client.GetAsync("api/scoreboard/flights").Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        HttpContent responseContent = response.Content;
        //        var json = responseContent.ReadAsStringAsync().Result;
        //        var flights = JsonConvert.DeserializeObject<List<Flight>>(json);
        //        return flights;
        //    }
        //    else
        //        return null;
        //}

        [HttpGet("{number}")]
       // [Route("get/food/{number}")] //номер рейса 
        public string Get(int number) // возвращаем 2 числа количество ( Normal - 0 , Vegan - 1 )
        {
            List<int> FoodCount = null;
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
                return json;
            }
            else
                return "0";

        }

        [HttpGet("{number}")]
       // [Route("get/baggage/{number}")] //номер рейса 
        public string GetBaggage(int number) // возвращаем список багажа
        {
            List<Baggage> baggage = null;
            foreach (var b in BaggageList)
            {
                if (b.FlightNumber == number)
                    baggage.Add(b);
            }
            if (baggage != null)
            {
                var json = JsonConvert.SerializeObject(baggage);
                return json;
                
            }
            else
                return "0";

        }


    }
}