using System;
using System.Collections.Generic;
using System.Text;

namespace FlightPassengerHttpClient
{
    public class Flight
    {
        public string frm { get; set; }
        public string to { get; set; }
        public int timeStart { get; set; }
        public int timeStop { get; set; }
        public int count { get; set; }
        public Guid reisNumber { get; set; }
        public Guid plain { get; set; }
        public int registrtionTime { get; set; }
        public int boardingTime { get; set; }
    }
}
