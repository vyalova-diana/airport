using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketModel
{
    public enum Sex
    {
        Female,
        Male
    }
    public class Ticket
    {
        //public readonly int tID; //ticket ID
        //public static int count; //for ticket ID
        public Guid tID;
        public Guid pID; //passenger ID
        public Guid fID; //flight ID
        public string city;
        public string pName;
        public string pSecondName;
        public Sex sex;

        private Guid ticketID
        {
            get
            {
                return tID;
            }
            set
            {
                tID = value;
            }
        }
        private string City
        {
            get
            {
                return city;
            }
            set
            {
                city = value;
            }
        }
        private string PNAme
        {
            get
            {
                return pName;
            }
            set
            {
                pName = value;
            }
        }
        private string PSecondName
        {
            get
            {
                return pSecondName;
            }
            set
            {
                pSecondName = value;
            }
        }
        private Sex Gender
        {
            get
            {
                return sex;
            }
            set
            {
                sex = value;
            }
        }
        private Guid PID
        {
            get
            {
                return pID;
            }
            set
            {
                pID = value;
            }
        }
        private Guid FID
        {
            get
            {
                return fID;
            }
            set
            {
                fID = value;
            }
        }
        public Ticket(Guid pID, Guid fID, string city, string pName, string pSecondName, Sex sex)
        {
            tID = Guid.NewGuid();
            PID = pID;
            FID = fID;
            City = city;
            PNAme = pName;
            PSecondName = pSecondName;
            Gender = sex;
            //this.tID = ++count;
        }

        public static void Output(Ticket t)
        {
            Console.WriteLine("Ticket id: {0}, passenger id: {1}, flight id: {2}, city: {3}, name: {4}, second name: {5}, sex: {6}.", t.tID, t.pID, t.fID, t.city, t.pName, t.pSecondName, t.sex);
        }
    }
}

