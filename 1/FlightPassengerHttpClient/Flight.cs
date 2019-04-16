using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace FlightPassengerHttpClient
{
    public class Flight
    {
        public string frm { get; set; }
        public string to { get; set; }
        public int timeStart { get; set; }
        public int timeStop { get; set; }
        public int? count { get; set; }
        public int reisNumber { get; set; }
        public int? plain { get; set; }
        public int? registrtionTime { get; set; }
        public int? boardingTime { get; set; }
    }
}
