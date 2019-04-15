using System;
using TicketModel;

namespace TicketWindow.Models
{
    public class Passenger
    {
        public Guid guid { get; set; }
        public string Surname { get; set; }
        public string GivenNames { get; set; }
        public string Nationality { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Sex sex { get; set; }
    }
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
