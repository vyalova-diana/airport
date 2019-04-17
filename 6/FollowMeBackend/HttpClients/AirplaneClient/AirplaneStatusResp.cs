using System;
using System.Collections.Generic;
using System.Text;

namespace FollowMeBackend.HttpClients
{
    public class AirplaneStatusResp
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public string Passengers { get; set; }
		public string PassengersMax { get; set; }
        public AirplaneRoute Route { get; set; }

        public string Baggage { get; set; }
        public string Fuel { get; set; }
        public string FuelMax { get; set; }

        public string Defrosted { get; set; }
        public string Food { get; set; }
    }
    public class AirplaneRoute
    {
        public string frm { get; set; }
        public string to { get; set; }
        public int timeStart { get; set; }
        public int timeStop { get; set; }
        public int count { get; set; }
        public int reisNumber { get; set; }
        public int plain { get; set; }
        public int registrtionTime { get; set; }
        public int boardingTime { get; set; }
    }
}