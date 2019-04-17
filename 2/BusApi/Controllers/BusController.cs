using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BusClasses;
using System.Threading;

namespace BusApi.Controllers
{
    [Produces("application/json")]
    [Route("bus")]
    public class MessagesController : Controller
    {
        [Route("newtask/{plane}/{dest}")] //ВНЕШКА  этот пост используется сторонними системами, для постановки задания
        public string Post(int plane, int dest)
        {   
            var bl = new BusLog();
            var Busses = new List<Bus>();
            var filebusses = new FileCSV();
            Busses = filebusses.ReadFromCSVBus();
            if (Busses.Count == 0)
            {
                bl.WriteToLog("Шесть автобусов на стоянке. ID автобусов от 1 до 6");
                for (var j = 1; j <= 6; j++)
                {
                    var bus = new Bus() { busId = j, isFree = 1 };
                    Busses.Add(bus);
                    filebusses.WriteToCSVBus(j, 1);
                }
            }
            bl.WriteToLog("Получено задание. ID самолета - " + plane);
            int i = 0;
            while (i < Busses.Count() && Busses[i].isFree != 1)
                i++;
            if (Busses[i].isFree == 1)
            {
                bl.WriteToLog("Для самолета " + plane + " назначен автобус. ID автобуса - " + Busses[i].busId + ". Вместимость - 30 пассажиров.");
                var filetasks = new FileCSV();
                Busses[i].planeId = plane;
                Busses[i].dest = dest;
                filetasks.ChangeBusStatusCSV(Busses[i].busId);
                if (dest == 0)
                {
                    Busses[i].AllowMoving("BGR", "GT1");
                    Busses[i].SendLocation("BGR", "GT1", "Moving");
                }
                else
                {
                    Busses[i].FindPlaneLocationCode();
                    Busses[i].AllowMoving("BGR", Busses[i].planelocationcode);
                    Busses[i].SendLocation("BGR", Busses[i].planelocationcode, "Moving");
                }
                Thread busthread;
                busthread = new Thread(new ThreadStart(Busses[i].Execute));
                busthread.Start();
                return "busID=" + Busses[i].busId + ";amountPassengers=30;flagMoving=true";
            }
            else
            {
                bl.WriteToLog("Нет свободных автобусов для самолета " + plane);
                return "1";
            }
        }

        [Route("setpassenger")] //ВНЕШКА  этот пост используется системой пассажир для посадки в автобус
        public void Post([FromBody]ValueTuple<Guid, Guid> pastuple)
        {
            var filepassangers = new FileCSV();
            var bl = new BusLog();
            var pas = new Passanger();

            byte[] p = pastuple.Item1.ToByteArray();
            int pint = BitConverter.ToInt32(p, 0);
            byte[] b = pastuple.Item2.ToByteArray();
            int bint = BitConverter.ToInt32(b, 0);


            pas.passangerId = pint;
            pas.busId = bint;
            bl.WriteToLog("Автобус " + bint + " принялна борт пассажира " + pint + ".");
            filepassangers.WriteToCSVPassanger(pas);
        }

        [Route("setcountpas/{busId}/{cnt}")] //ВНЕШКА  сколько пассажиров хотят посадить в автобус
        public void Post(int cnt, int busId)
        {
            var filepassangers = new FileCSV();
            filepassangers.WriteToCSVCnts(busId, cnt);
        }

        [Route("movebus")] //ВНЕШКА  этот пост используется для указания, что автобус может двигаться
        public void Post([FromBody]PermissonForMoving pfm)
        {
            var filemoving = new FileCSV();
            filemoving.WriteToCSVMoving(pfm.identifier);
        }

    }
}