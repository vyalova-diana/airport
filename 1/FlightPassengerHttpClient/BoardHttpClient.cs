using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace FlightPassengerHttpClient
{
    /*
    public enum RegistrationStatus
    {
        NotStarted = 1,
        InProgress,
        IsOver
    }
    */
    class BoardHttpClient
    {
        private HttpClient Client { get; set; }

        public BoardHttpClient(HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri("http://localhost:44367/");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Client = httpClient;
        }

        public List<Flight> GetFlights()
        {
            HttpResponseMessage response = Client.GetAsync("api/flightpassengers").Result;
            if (response.IsSuccessStatusCode)
            {
                HttpContent responseContent = response.Content;
                var json = responseContent.ReadAsStringAsync().Result;
                var flights = JsonConvert.DeserializeObject<List<Flight>>(json);
                return flights;
            }
            else
                return null;
        }
        public int GetRegistrationStatus(int flightId)
        {
            var stringContent = new StringContent(JsonConvert.SerializeObject(flightId), Encoding.UTF8, "application/json");
            HttpResponseMessage response = Client.GetAsync("api/scoreboard/flights/" + flightId).Result;
            if (response.IsSuccessStatusCode)
            {

                HttpContent responseContent = response.Content;
                var json = responseContent.ReadAsStringAsync().Result;
                var rs = JsonConvert.DeserializeObject<int>(json);
                return rs;
            }
            else
                return -1;
        }
    }
}
