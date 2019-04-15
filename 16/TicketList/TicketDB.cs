using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketModel;

namespace TicketList
{
    public class TicketStorage
    {
        public static List<Ticket> boughtTickets = new List<Ticket>();

        private static TicketStorage instance;
        private TicketStorage()
        {

        }
        public static TicketStorage getInstance()
        {
            if (instance == null)
            {
                instance = new TicketStorage();
            }
            return instance;
        }

        public void Add(Ticket t)
        {
            boughtTickets.Add(t);
        }

        public Ticket takeTicket(Guid tID)
        {
            Ticket tmp = null;
            for (int i = 0; i < boughtTickets.Count; i++)
            {
                if (boughtTickets[i].tID == tID)
                {
                    tmp = boughtTickets[i];
                }
            }
            return tmp;
        }

        public List<Ticket> takeAllTickets()
        {
            return boughtTickets;
        }

        public void ptintTicket(Guid tID)
        {
            Ticket tmp = null;
            for (int i = 0; i < boughtTickets.Count; i++)
            {
                if (boughtTickets[i].tID == tID)
                {
                    tmp = boughtTickets[i];
                }
            }
            Ticket.Output(tmp);
        }

        public void printAllTickets()
        {
            foreach (Ticket t in boughtTickets)
            {
                Ticket.Output(t);
            }
        }

        public int getTicketsCount()
        {
            return boughtTickets.Count();
        }

        public Ticket getPassengerTicket(Guid pID)
        {
            Ticket tmp = null;
            for (int i = 0; i < boughtTickets.Count; i++)
            {
                if (boughtTickets[i].pID == pID)
                {
                    tmp = boughtTickets[i];
                }
            }
            return tmp;
        }

        public bool fingPassenger(Guid pID)
        {
            bool found = false;
            for (int i = 0; i < boughtTickets.Count; i++)
            {
                if (boughtTickets[i].pID == pID)
                {
                    found = true;
                    break;
                }
            }
            return found;
        }

        //number of passangers in particular flight
        public int flightCount(Guid fID)
        {
            int count = 0;
            for (int i = 0; i < boughtTickets.Count; i++)
            {
                if (boughtTickets[i].fID == fID)
                    count++;
            }
            return count;
        }
    }
}

