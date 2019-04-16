using System;
using System.Collections.Generic;
using System.Text;

namespace FlightPassengerHttpClient
{
    public class Ticket
    {
        public Ticket(Guid ticketId, Guid passnegerId, int flightId, string cityArrive, string givenNames, string surname, Sex gender)
        {
            tID = ticketId;
            pID = passnegerId;
            fID = flightId;
            city = cityArrive;
            pName = givenNames;
            pSecondName = surname;
            sex = gender;
        }
        public Guid tID;
        public Guid pID; //passenger ID
        public int fID; //flight ID
        public string city;
        public string pName;
        public string pSecondName;
        public Sex sex;
    }
}
