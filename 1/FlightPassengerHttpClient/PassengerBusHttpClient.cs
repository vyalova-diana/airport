using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace FlightPassengerHttpClient
{
    class PassengerBusHttpClient
    {
        private HttpClient Client { get; set; }

        public PassengerBusHttpClient(HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri("http://localhost:44367/");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Client = httpClient;
        }
        public bool EnterTheBus(Guid passengerId, Guid busId)
        {
            var pasbus = (PassengerId: passengerId, BusId: busId);
            var stringContent = new StringContent(JsonConvert.SerializeObject(pasbus), Encoding.UTF8, "application/json");
            HttpResponseMessage response = Client.PostAsync("api/values/3", stringContent).Result;
            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }
        public bool EnterTheLandBus(Guid passengerId, Guid busId)
        {
            var pasbus = (PassengerId: passengerId, BusId: busId);
            var stringContent = new StringContent(JsonConvert.SerializeObject(pasbus), Encoding.UTF8, "application/json");
            HttpResponseMessage response = Client.PostAsync("api/values/3", stringContent).Result;
            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }
    }
}
