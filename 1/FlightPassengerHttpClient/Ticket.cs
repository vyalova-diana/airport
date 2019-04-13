using System;
using System.Collections.Generic;
using System.Text;

namespace FlightPassengerHttpClient
{
    public class Ticket
    {
        public Ticket(Guid ticketId, Guid passnegerId, Guid flightId, string cityArrive, string givenNames, string surname, Sex gender)
        {
            tID = ticketId;
            pID = passnegerId;
            fID = flightId;
            city = cityArrive;
            pName = givenNames;
            pSecondName = surname;
            sex = gender;
        }
        public Guid tID; //id билета
        public static int count;
        public Guid pID; //id пассажира
        public Guid fID; //id рейса
        public string city; //место назначения
        public string pName; //имя
        public string pSecondName; //фамилия
        public Sex sex; //пол
    }
}
