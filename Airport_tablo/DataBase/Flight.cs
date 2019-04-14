using System;
using System.Collections.Generic;
using System.Text;

namespace DataBase
{
    public class Flight
    {
        private Guid id;
        private string from;
        private string to;
        private DateTime time;
        private int status;

        public Guid Id { get => id; set => id = value; }
        public string From { get => from; set => from = value; }
        public string To { get => to; set => to = value; }
        public DateTime Time { get => time; set => time = value; }
        public int Status { get => status; set => status = value; }
    }
}
