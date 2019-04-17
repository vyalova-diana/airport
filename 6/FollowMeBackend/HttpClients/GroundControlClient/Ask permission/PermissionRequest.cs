using System;
using System.Collections.Generic;
using System.Text;

namespace FollowMeBackend.HttpClients.GroundControlClient
{
    public class PermissionRequest
    {
      
        public string from { get; set; }
        public string to { get; set; }

        public string service { get; set; }
        public string identifier { get; set; }
    }
}
