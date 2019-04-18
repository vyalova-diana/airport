using System;
using System.Collections.Generic;
using System.Text;

namespace CateringDB
{
    class GetGateRequestResult
    {
        public string service { get; set; }
        public string identifier { get; set; }
        public string locationCode { get; set; }
        public string status { get; set; }
    }
}
