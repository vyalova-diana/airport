using System.Collections.Generic;
using FlightPassengerHttpClient;

namespace FlightPassengerApi
{
    public class DataBase
    {
        public DataBase()
        {
            flightPassengers = new List<FlightPassenger>();
            arriveFlightPassengers = new List<FlightPassenger>();
        }
        public List<FlightPassenger> flightPassengers;
        public List<FlightPassenger> arriveFlightPassengers;
    }
}
