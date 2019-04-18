using System;
using System.Collections.Generic;
using System.Text;

namespace DataBase
{
    public class Db
    {
        public Db()
        {
            flights = new List<Flight>();

        }
        public List<Flight> flights;
        public void AddFlight(Flight f)
        {
            flights.Add(f);
        }

        public void DelFlight(Flight fl)
        {
            flights.Remove(fl);
        }
    }
}
