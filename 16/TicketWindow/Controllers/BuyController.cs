﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using TicketModel;
using TicketList;
using TicketWindow.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TicketWindow.Controllers
{
    [Route("[controller]")]
    public class BuyController : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        // GET request for buying a ticket and getting it back
        [HttpGet("{JsonData}")]
        public string Get(int id, string data)
        {
            var jdata = JsonConvert.DeserializeObject<ValueTuple<Flight, Passenger>>(data);
            Flight f = jdata.Item1;
            Passenger p = jdata.Item2;
            int passnumber = 0; //number of passengers in the plain
            Guid flightID = f.reisNumber;
            string ticketJson = null;
            string scheduleData = ScheduleHttpClient.ScheduleRequest(flightID);
            int scheduleInt = Convert.ToInt32(scheduleData);
            if (scheduleInt >= 0)
            {
                passnumber = scheduleInt;
                if (TicketStorage.getInstance().fingPassenger(p.guid))
                {
                    return "2"; //passenger with this id has already buy a ticket
                }
                else
                {
                    if (TicketStorage.getInstance().flightCount(flightID) < passnumber)
                    {
                        Ticket t = new Ticket(p.guid, flightID, f.to, p.Surname, p.GivenNames, p.sex);
                        TicketStorage.getInstance().Add(t);
                        ticketJson = JsonConvert.SerializeObject(t);
                        return ticketJson;
                    }
                    else
                    {
                        return "3"; //there are no more tickets for this flight
                    }
                }
            }
            else
            {
                if (scheduleInt == -3)
                {
                    return "4"; //flight registration
                }
                else if (scheduleInt == -4 || scheduleInt == -2 || scheduleInt == -1)
                {
                    return "1"; //flight to this city doesn't exist or plain has already landed or there is no plain for this flight - can not buy a ticket
                }
                else
                {
                    return "5";
                }
            }
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {

        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
