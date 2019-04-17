using System;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading;
using TicketModel;

namespace TicketWindow.Models
{
    public enum TypeOfFood : int
    {
        Normal,
        Vegan
    }
    public class FlightPassenger
    {
        public FlightPassenger()
        {

        }
        public FlightPassenger(Passport passport, uint baggageWeight, TypeOfFood typeOfFood)
        {
            Passport = passport;
            BaggageWeight = baggageWeight;
            TypeOfFood = typeOfFood;
        }
        public Passport Passport { get; set; }
        public uint BaggageWeight { get; set; }
        public TypeOfFood TypeOfFood { get; set; }
        public Ticket Ticket { get; set; }
        public List<Flight> Flights { get; set; }
        public Guid BusId { get; set; }
    }
    public class Passport
    {
        public Guid Guid { get; set; }
        public string Surname { get; set; }
        public string GivenNames { get; set; }
        public string Nationality { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Sex Sex { get; set; }
    }
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
