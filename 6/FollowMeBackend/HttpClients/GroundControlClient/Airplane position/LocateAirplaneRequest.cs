using System;
using System.Collections.Generic;
using System.Text;

namespace FollowMeBackend.HttpClients.GroundControlClient
{ 
    public class LocateAirplaneRequest
    {
        public static string service= "Air Facilities";
        public string identifier { get; set; }
    }
}
