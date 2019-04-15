using System;
using System.Collections.Generic;
using System.Text;

namespace DataBase
{
    public class Flight
    {
        private int id;
        private string from;
        private string to;
        private int timeStart;
        private int timeStop;
        private int status;

        public int Id { get => id; set => id = value; }
        public string From { get => from; set => from = value; }
        public string To { get => to; set => to = value; }
        public int TimeStart { get => timeStart; set => timeStart = value; }
        public int TimeStop { get => timeStop; set => timeStop = value; }
        public int Status { get => status; set => status = value; }
    }
}
