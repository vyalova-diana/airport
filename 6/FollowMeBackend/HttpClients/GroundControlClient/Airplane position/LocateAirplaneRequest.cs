using System;
using System.Collections.Generic;
using System.Text;

namespace FollowMeBackend.HttpClients.GroundControlClient
{ 
    public class LocateAirplaneRequest
    {
        public string service= "Air Facility";
        public string identifier { get; set; }
    }
}
