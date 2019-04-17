using System;
using System.Collections.Generic;
using System.Text;

namespace FollowMeBackend.HttpClients.GroundControlClient
{
    public class AirplanePositionResponse
    {
        public string result { get; set; }
        public string service { get; set; }
        public string identifier { get; set; }
        public string locationCode { get; set; }
        public string status { get; set; }
    }
}
