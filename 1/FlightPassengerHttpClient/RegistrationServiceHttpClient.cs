using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace FlightPassengerHttpClient
{
    class RegistrationServiceHttpClient
    {
        private HttpClient Client { get; set; }

        public RegistrationServiceHttpClient(HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri("http://localhost:44367/");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Client = httpClient;
        }
        public bool Register(FlightPassenger flightPassenger)
        {
            var stringContent = new StringContent(JsonConvert.SerializeObject(flightPassenger), Encoding.UTF8, "application/json");
            HttpResponseMessage response = Client.PostAsync("api/values/3", stringContent).Result;
            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }
    }
}
