using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DBlib;
using System.Threading;


namespace MessageWebApi.Controllers
{

    


    [RoutePrefix("api/Schedule")]
    public class ScheduleController : ApiController
    {
        [HttpGet]
        public List<Reis> Get()
        {
            MyTime MT = MyTime.GetMyTime();
            return MT.reises;
        }
        [Route("{id}")]
        public int GetId(int id)
        {
            MyTime MT = MyTime.GetMyTime();
            foreach (var r in MT.reises)
            {
                if (r.reisNumber == id)
                {
                    if (r.count != null)
                    {
                        if (r.registrtionTime != null)
                        {
                            if (MT.curMin < r.registrtionTime)
                                return (Convert.ToInt16(r.count));
                            else
                                return -3;//регистрация уже началась
                        }
                        else
                            return -1;//самолет прилетает, на него нет смысла покупать билеты
                    }
                    return -2;//К рейсу почему-то не прикреплен самолет
                }

            }
            return -4;//рейса нет
        }
        [Route("GetTime/{id}")]
        public int GetTime(int id)
        {
            MyTime MT = MyTime.GetMyTime();
            foreach(var r in MT.reises)
            {
                if (r.reisNumber == id)
                {
                    if (r.registrtionTime != null)
                    {
                        return (r.timeStop - r.timeStart);
                    }
                    else return -1;
                }
            }
            return 0;
        }

    }

    class NameComparer : IComparer<Reis>
    {
        public int Compare(Reis o1, Reis o2)
        {
            if (o1.timeStart > o2.timeStart)
            {
                return -1;
            }
            else if (o1.timeStart < o2.timeStart)
            {
                return 1;
            }

            return 0;
        }
    }
    public class MyTime
    {
        public List<Reis> reises;
        List<Coordinate> towns = new List<Coordinate>();
        Coordinate curTown;
        public int curMin;
        private static MyTime MT;
        private MyTime()
        {
            curMin = 0;
            towns.Add(new Coordinate(56, 38, "Москва"));
            towns.Add(new Coordinate(60, 30, "Санкт-Питербург"));
            towns.Add(new Coordinate(41, -74, "Нью-Йорк"));
            towns.Add(new Coordinate(36, 140, "Токио"));
            towns.Add(new Coordinate(30, 31, "Каир"));
            towns.Add(new Coordinate(1, 104, "Сингапур"));
            towns.Add(new Coordinate(38, -122, "Сан-Франциско"));
            towns.Add(new Coordinate(49, 2, "Париж"));
            towns.Add(new Coordinate(51, 0, "Лондон"));
            towns.Add(new Coordinate(65, 178, "Анадырь"));
            towns.Add(new Coordinate(37, -8, "Фаро"));

            curTown = new Coordinate(56, 38, "Москва");

            Start();
        }

        public static MyTime GetMyTime()
        {
            if (MT == null)
            {
                MT = new MyTime();
            }
            return MT;
        }
        public void Start()
        {
            DB db = new DB();
            reises = db.ReadFile();
            if (reises.Count() == 0)
            {
                reises = GenerateReises();
                bool flag = false;
                while (!flag)
                {
                    try
                    {
                        foreach (var a in reises)
                        {
                            if (a.plain == null)
                            {
                                //Взять у Васи самолеты
                            }
                        }
                        flag = true;
                    }
                    catch { flag = false; }
                }
                try
                {
                    db.SaveAll(reises);
                }
                catch
                { }
            }
            Thread timer = new Thread(new ThreadStart(Timer));
            timer.Start();
            foreach (var r in reises)
            {
                Thread t = new Thread(delegate () { forTime(r); });
                t.Start();
            }
        }

        public List<Reis> GenerateReises()
        {
            NameComparer nc = new NameComparer();
            reises = new List<Reis>();
            for (int i = 1; i <= 10; i++)
                reises.Add(CreateReis(i));
            return reises;
        }
        Random random = new Random((int)DateTime.Now.Ticks);
        public int r(int a, int b) { return random.Next(a, b); }
        public Reis CreateReis(int i)
        {
            int f = r(0, 3);

            int k = r(0, towns.Count);
            while ((towns.ElementAt(k)).town == curTown.town)
                k = r(0, towns.Count);
            int time = (curTown.destenation((towns.ElementAt(k)).x, (towns.ElementAt(k)).y)) / 7;
            int timesart = r(240, 1440);
            if (f == 2)
                return new Reis(towns.ElementAt(k).town, curTown.town, timesart, timesart + time, null, i, null, null, null);
            else
                return new Reis(curTown.town, towns.ElementAt(k).town, timesart, timesart + time, null, i, null);
        }
        public void Timer()
        {
            while (true)
            {
                curMin++;
                Thread.Sleep(1000);
            }

        }
        public void forTime(Reis reis)
        {

            bool b = false, r = false, s = false;
            int t = curMin;
            if (reis.registrtionTime != null)
            {
                while (!b || !r || !s)
                {
                    if (t >= reis.registrtionTime && !r)
                    {
                        r = true;
                        try
                        {
                            //отправить Алине и Лере статус о начале регистрации
                        }
                        catch { }
                    }
                    if (t >= reis.boardingTime && !b)
                    {
                        try
                        {
                            b = true;
                        //отправить Алине и Лере статус о начале посадки, Васе  о подготовке самолета
                    }
                        catch { }
                }
                    if (t >= reis.timeStart && !s)
                    {
                        try
                        {
                            s = true;
                        //  Васе о том, что пора лететь
                    }
                        catch { }
                }
                    Thread.Sleep(1000);
                    t++;
                }

            }
            else
            {
                while (!s)
                {
                    if (t >= reis.timeStart && !s)
                    {
                        s = true;
                        //Васе о том, что пора лететь
                    }
                    Thread.Sleep(1000);
                    t++;
                }
            }
        }
        public void AddReis(string txt)
        {
            string[] s = txt.Split(';');
            bool f = false;
            int q = 0;
            try { q = Convert.ToInt16(s[2]); }
            catch { };
            if (s.Length == 3 && (s[0] == curTown.town || s[1] == curTown.town) && q > 100)
                try
                {
                    int time = 0;
                    //string i = towns.ElementAt(towns.Count() - 1).;
                    if (s[1] == curTown.town)
                    {
                        foreach (var t in towns)
                        {
                            if (t.town == s[0])
                            {

                                time = (curTown.destenation(t.x, t.y)) / 7;
                                reises.Add(new Reis(s[0], curTown.town, q, q + time, null, towns.Count + 1, null, null, null));
                                break;
                            }
                        }
                    }
                    else
                    {
                        foreach (var t in towns)
                        {
                            if (t.town == s[1])
                            {

                                time = (curTown.destenation(t.x, t.y)) / 7;
                                reises.Add(new Reis(curTown.town, s[0], q, q + time, null, towns.Count + 1, null));
                                break;
                            }
                        }
                    }
                    f = true;
                }
                catch { }
            if (f)
            {
                f = false;
                while (!f)
                {
                    try
                    {
                        //Взять у Васи самолет
                        f = true;
                    }
                    catch
                    { }
                }
                f = false;
                while (!f)
                {
                    try
                    {
                        //отправить Алине рейсы
                        f = true;
                    }
                    catch
                    { }
                }
                DB db = new DB();
                try
                {
                    db.SaveOne(reises.ElementAt(towns.Count() - 1));
                }
                catch { }

                Thread t = new Thread(delegate () { forTime(reises.ElementAt(towns.Count() - 1)); });
                t.Start();
            }
        }

    }
}
