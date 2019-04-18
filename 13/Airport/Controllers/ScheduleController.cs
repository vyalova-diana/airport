using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Runtime.Serialization.Json;
using System.IO;
using Newtonsoft.Json;


namespace Airport.Controllers

{
    [Produces("application/json")]
    [Route("api/Schedule")]
    public class ScheduleController : Controller
    {
        // GET: api/Schedule
        [HttpGet]
        public List<Reis> Get()
        {
            MyTime MT = MyTime.GetMyTime();
            return MT.reises;
        }

        // GET: api/Schedule/5
        [HttpGet("{id}", Name = "Get")]
        public int Get(int id)
        {
            MyTime MT = MyTime.GetMyTime();
            Console.WriteLine("Запрошена информация по рейсу " + id.ToString());
            foreach (var r in MT.reises)
            {
                if (r.reisNumber == id)
                {
                    if (r.count != null)
                    {
                        if (r.registrtionTime != null)
                        {
                            if (MT.curMin < r.registrtionTime)
                            {
                                Console.WriteLine("Количество возможных пассажиров на рейс " + id.ToString() + " --> " + r.count.ToString());

                                return (Convert.ToInt16(r.count));
                            }
                            else
                            {
                                Console.WriteLine("Регистрация на рейс " + id.ToString() + "уже началась");

                                return -3;//регистрация уже началась
                            }
                        }
                        else
                        {
                            Console.WriteLine("Рейс " + id.ToString() + " прибывает в наш аэропорт");

                            return -1;//самолет прилетает, на него нет смысла покупать билеты
                        }
                    }
                    Console.WriteLine("К Рейсу " + id.ToString() + " не прикреплен самолет");
                    return -2;//К рейсу почему-то не прикреплен самолет
                }

            }
            Console.WriteLine("Рейса " + id.ToString() + " не существует");

            return -4;//рейса нет
        }
        [Route("GetTime/{id}")]
        public string GetTime(int id)
        {
            Console.WriteLine("Запрошена информация по Самолету " + id.ToString());
            string txt = "";
            MyTime MT = MyTime.GetMyTime();
            foreach (var r in MT.reises)
            {
                if (r.plain != null)
                {
                    if (r.plain == id)
                    {
                        if (r.registrtionTime != null)
                        {
                            Console.WriteLine("Самолет " + id.ToString() + " будет лететь " + (r.timeStop - r.timeStart).ToString() + " минут");
                            txt = (r.timeStop - r.timeStart).ToString() + ";" + r.reisNumber.ToString();
                            return txt;
                        }
                        else
                        {
                            Console.WriteLine("Самолет " + id.ToString() + " прилетает. Еду везти не надо.");
                            txt = "-1;" + r.reisNumber.ToString();
                            return txt;
                        }
                    }
                }
            }
            Console.WriteLine("Самолета " + id.ToString() + " несуществует.");
            txt = "0;0";
            return txt;
        }
    }

   
}
