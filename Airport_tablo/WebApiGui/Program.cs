using WebApiAirportNew;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using WebApiAirportNew.Controllers;
using DataBase;
using System.Text;

namespace WebApiGui
{
    class Program
    {
        static void Main(string[] args)
        {
    //        var client = new HttpClient();
    //        client.BaseAddress = new Uri("http://localhost:44336");//6404
    //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    //        //var messages = JsonConvert.DeserializeObject<ValuesController[]>(body);
    //        var result = client.GetAsync("api/scoreboard/Flights").Result;


    //        var msg = new Flight
    //        {
    //            Id = 1,
    //            From = "A",
    //            To = "B",
    //            Time = DateTime.Now,
    //            Status = 1


    //        };

    //        Console.WriteLine(result.Content.ReadAsStringAsync().Result);
    //        var content = new StringContent(
    //JsonConvert.SerializeObject(msg),
    //Encoding.UTF8, "application/json");

    //        var response = client.PostAsync("api/scoreboard/Flights", content).Result;

    //        response.EnsureSuccessStatusCode();

    //        var body = result.Content.ReadAsStringAsync().Result;

    //        var messages = JsonConvert.DeserializeObject<Flight[]>(body);
    //        foreach (var ms in messages)
    //        {
    //            Console.WriteLine(
    //                $"Id: {ms.Id}," +
    //                $"From: {ms.From}," +
    //                $"To: {ms.To}," +
    //                $"Time: {ms.Time}," +
    //                $"Status: {ms.Status}");
    //        }
    //        Console.ReadLine();
        }
    }
}
