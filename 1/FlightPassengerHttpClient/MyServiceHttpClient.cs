using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace FlightPassengerHttpClient
{
    class MyServiceHttpClient
    {
        private HttpClient Client { get; set; }

        public MyServiceHttpClient(HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri("https://localhost:44367/");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Client = httpClient;
        }
        public bool DeleteFlightPassenger(Guid id)
        {
            var stringContent = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
            HttpResponseMessage response = Client.DeleteAsync("api/flightpassengers/delete/" +id).Result;
            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }
        public bool DeleteArrivedFlightPassenger(Guid id)
        {
            var stringContent = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
            HttpResponseMessage response = Client.DeleteAsync("api/values/deletearrived/" + id).Result;
            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }
    }
}
