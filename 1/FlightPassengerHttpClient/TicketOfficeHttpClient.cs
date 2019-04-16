using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace FlightPassengerHttpClient
{
    class TicketOfficeHttpClient
    {
        private HttpClient Client { get; set; }

        public TicketOfficeHttpClient(HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri("http://localhost:44367/");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Client = httpClient;
        }
        public Ticket BuyTicket(FlightPassenger flightPassenger, Flight flight)
        {
            var fpFlight = (FlightPassenger: flightPassenger, Flight: flight);
            var stringContent = new StringContent(JsonConvert.SerializeObject(fpFlight), Encoding.UTF8, "application/json");
            HttpResponseMessage response = Client.PostAsync("api/values/3", stringContent).Result;
            if (response.IsSuccessStatusCode)
            {
                HttpContent responseContent = response.Content;
                var json = responseContent.ReadAsStringAsync().Result;
                var ticket = JsonConvert.DeserializeObject<Ticket>(json);
                return ticket;
            }
            else
                return null;
        }
    }
}
