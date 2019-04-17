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
using NLog;


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
            logger.Info("Запрошена информация по рейсу " + id.ToString());
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
                                logger.Info("Количество возможных пассажиров на рейс " + id.ToString() + " --> " + r.count.ToString());

                                return (Convert.ToInt16(r.count));
                            }
                            else
                            {
                                logger.Info("Регистрация на рейс " + id.ToString() + "уже началась");

                                return -3;//регистрация уже началась
                            }
                        }
                        else
                        {
                            logger.Info("Рейс " + id.ToString() + " прибывает в наш аэропорт");

                            return -1;//самолет прилетает, на него нет смысла покупать билеты
                        }
                    }
                    logger.Info("К Рейсу " + id.ToString() + " не прикреплен самолет");
                    return -2;//К рейсу почему-то не прикреплен самолет
                }

            }
            logger.Info("Рейса " + id.ToString() + " не существует");

            return -4;//рейса нет
        }
        public static Logger logger = LogManager.GetCurrentClassLogger();
        [Route("GetTime/{id}")]
        public string GetTime(int id)
        {
            logger.Info("Запрошена информация по Самолету " + id.ToString());
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
                            logger.Info("Самолет " + id.ToString() + " будет лететь " + (r.timeStop - r.timeStart).ToString() + " минут");
                            txt = (r.timeStop - r.timeStart).ToString() + ";" + r.reisNumber.ToString();
                            return txt;
                        }
                        else
                        {
                            logger.Info("Самолет " + id.ToString() + " прилетает. Еду везти не надо.");
                            txt = "-1;" + r.reisNumber.ToString();
                            return txt;
                        }
                    }
                }
            }
            logger.Info("Самолета " + id.ToString() + " несуществует.");
            txt = "0;0";
            return txt;
        }
    }

   
}
