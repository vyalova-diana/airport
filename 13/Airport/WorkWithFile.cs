using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;
using System.IO;
using System.Threading;
using System.Runtime.Serialization.Json;

using System.Text;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Airport
{
    public class MyTime
    {
        public List<Reis> reises;
        List<Coordinate> towns = new List<Coordinate>();
        Coordinate curTown;
        public int curMin;
        private static MyTime MT;
        int speed = 1;
        private MyTime()
        {
            reises = new List<Reis>();
            curMin = 0;
            towns.Add(new Coordinate(56, 38, "Moscow"));
            towns.Add(new Coordinate(60, 30, "Saint Petersburg"));
            towns.Add(new Coordinate(41, -74, "New York"));
            towns.Add(new Coordinate(36, 140, "Tokyo"));
            towns.Add(new Coordinate(30, 31, "Cairo"));
            towns.Add(new Coordinate(1, 104, "Singapore"));
            towns.Add(new Coordinate(38, -122, "San Francisco"));
            towns.Add(new Coordinate(49, 2, "Paris"));
            towns.Add(new Coordinate(51, 0, "London"));
            towns.Add(new Coordinate(37, -8, "Faro"));
            towns.Add(new Coordinate(60, 150, "Magadan"));
            towns.Add(new Coordinate(67, 64, "Vorkuta"));
            towns.Add(new Coordinate(69, 33, "Murmansk"));
            int q = r(0, towns.Count);
            curTown = towns.ElementAt(q);
            bool flag = false;
            while (!flag)
            {
                try
                {
                    flag = true;
                    var client = new HttpClient();
                    client.BaseAddress = new Uri("http://localhost:7011/");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    string req = "api//deicing//GetCity//" + curTown.town;
                    var result = client.GetAsync(req).Result;
                }
                catch
                {
                    Console.WriteLine("Сервис погоды не доступен.");
                    flag = false;
                }
            }
            Console.WriteLine("Текущий город:" + curTown.town);
            Start(1);
        }

        public static MyTime GetMyTime()
        {
            if (MT == null)
            {
                MT = new MyTime();
            }
            return MT;
        }
        public void Start(int b)
        {
            try
            {
                reises.Clear();
            }
            catch { };
            bool flag = false;
            reises = GenerateReises();
            while (!flag)
            {
                try
                {
                    flag = true;

                    foreach (var a in reises)
                    {
                        if (a.plain == null && a.registrtionTime != null)
                        {
                            easyPlane eP = new easyPlane(a, 0);
                            var client = new HttpClient();
                            client.BaseAddress = new Uri("http://localhost:7014/");
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            string req = "planes//create";
                            var serializer = new DataContractJsonSerializer(typeof(easyPlane));
                            var stringPayload = JsonConvert.SerializeObject(eP);
                            var content = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                            var result = client.PostAsync(req, content).Result;
                            var body = result.Content.ReadAsStringAsync().Result;
                            //Взять у Васи самолеты
                            JsonSerializerSettings settings = new JsonSerializerSettings();
                            settings.Converters.Add(new PlaneConvertor());
                            var plane = JsonConvert.DeserializeObject<Plane>(body, settings);
                            a.plain = plane.Id;
                            a.count = plane.PassengersMax;
                        }


                    }
                    Console.WriteLine("Привязка самолетов закончена");
                }
                catch
                {
                    Console.WriteLine("Не удалось прикрепить самолет");
                    flag = false;
                }
            }
            string txt = "\n";
            foreach (Reis r in reises)
                txt += r.reisToString() + "\n";
            Console.WriteLine("Расписание:" + txt);
            flag = false;
            while (!flag)
            {
                try
                {
                    flag = true;
                    var client = new HttpClient();
                    client.BaseAddress = new Uri("http://localhost:7015/");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    string req = "api//board//Flights";
                    var serializer = new DataContractJsonSerializer(typeof(List<Reis>));
                    var stringPayload = JsonConvert.SerializeObject(reises);
                    var content = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                    var result = client.PostAsync(req, content).Result;
                    var body = result.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("Cвязь с табло установлена");
                    flag = true;
                }
                catch
                {
                    Console.WriteLine("Не удалось связаться с табло");

                    flag = false;
                }
            }

            if (b == 1)
            {
                Thread timer = new Thread(delegate () { Timer(b); });
                timer.Start();
            }
            foreach (var r in reises)
            {

                    Thread t = new Thread(delegate () { ForTime(r); });
                    t.Start();
            }
            
        }

        public List<Reis> GenerateReises()
        {
            for (int i = 1; i <= 10; i++)
                reises.Add(CreateReis());
            return reises;
        }
        Random random = new Random((int)DateTime.Now.Ticks);
        public int r(int a, int b) { return random.Next(a, b); }
        public Reis CreateReis()
        {
            int i;
            if (reises.Count == 0)
                i = 1;
            else
            {
                i = reises.ElementAt(((reises.Count) - 1)).reisNumber + 1;
            }
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
        public void Timer(int b)
        {
            Console.WriteLine("День 1 Время: " + (curMin / 60).ToString() + "ч. " + (curMin % 60).ToString() + "мин.");
            int i = 0;
            while (true)
            {
                curMin += speed;
                i++;
                Thread.Sleep(1000);
                if (curMin >= 1440 * b)
                {
                    b++;
                    Start(b);
                    Console.WriteLine("День " + b.ToString());
                }
                if (i >= 60 / speed)
                {
                    i = 0;
                    Console.WriteLine("День " + b.ToString() + " Время: " + ((curMin - 24 * (b - 1) * 60) / 60).ToString() + "ч. " + (curMin % 60).ToString() + "мин.");
                }
            }

        }

        public int ChangeSt(string id, string st, string txt, Uri u)
        {
            var client = new HttpClient();
            client.BaseAddress = u;
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string req = txt + "api//Schedule//FlightStatus//" + id + "//" + st;
            var result = client.GetAsync(req).Result;
            if (txt == "api/board/FlightStatus//" || txt == "CheckIn//CheckInStatus//")
                return 0;
            var body = result.Content.ReadAsStringAsync().Result;
            int messages = JsonConvert.DeserializeObject<int>(body);
            return messages;
        }

        public int ForTime(Reis reis)
        {
            Uri A = new Uri("http://localhost:7015/");
            Uri L = new Uri("http://localhost:44304/");
            Uri V = new Uri("http://localhost:7014/");
            string txtA = "api/board/FlightStatus//";
            string txtL = "CheckIn//CheckInStatus//";
            string txtV = "plane//update_status//";

            bool b = false, r = false, s = false, pp = false, pb = false, pf = false;
            if (reis.registrtionTime != null)
            {
                while (!s)
                {
                    if (curMin >= reis.registrtionTime && !r)
                    {
                        r = true;
                        try
                        {
                            ChangeSt(reis.reisNumber.ToString(), "//1", txtA, A);
                            ChangeSt(reis.reisNumber.ToString(), "//1", txtL, L);
                            //отправить Алине и Лере статус о начале регистрации
                            Console.WriteLine("На рейс " + reis.reisNumber.ToString() + " открыта регистрация");
                            reis.registrtionTime = curMin;
                        }
                        catch
                        {
                            Console.WriteLine("Служба регистрации и/или табло не доступны-> регистрация не началась->пробуем еще раз. Рейс " + reis.reisNumber.ToString());
                            r = false;
                        }
                        if (!pp)
                        {
                            pp = true;
                            try
                            {
                                int message = ChangeSt(reis.plain.ToString(), "//1", txtV, V);
                                if (message == 0)
                                {
                                    pp = false;
                                    Console.WriteLine("Cамолет, связанный с рейсом " + reis.reisNumber.ToString() + ", не может начать подготовку(Не готов к этому).");
                                }
                                else
                                    Console.WriteLine("Cамолет, связанный с рейсом " + reis.reisNumber.ToString() + ", начал подготовку.");
                            }
                            catch
                            {
                                Console.WriteLine("Cамолет, связанный с рейсом " + reis.reisNumber.ToString() + ", не доступен.-> не начал подготовку->пробуем еще раз.");
                                pp = false;
                            }
                        }

                    }
                    if (curMin >= reis.boardingTime && !b && pp && r)
                    {
                        if (!pb)
                        {
                            pb = true;
                            try
                            {
                                int message = ChangeSt(reis.plain.ToString(), "//6", txtV, V);
                                if (message == 0)
                                {
                                    pb = false;
                                    Console.WriteLine("Cамолет, связанный с рейсом " + reis.reisNumber.ToString() + ", не может начать посадку пассажиров.");
                                }
                                else
                                {
                                    Console.WriteLine("Cамолет, связанный с рейсом " + reis.reisNumber.ToString() + ", начал посадку пассажиров.");
                                }
                            }
                            catch
                            {
                                Console.WriteLine("Cамолет, связанный с рейсом " + reis.reisNumber.ToString() + ", не доступен.-> не начал посадку пассажиров.->пробуем еще раз.");
                                pb = false;
                            }
                        }
                        if (pb)
                        {
                            try
                            {
                                b = true;
                                ChangeSt(reis.reisNumber.ToString(), "//2", txtA, A);
                                ChangeSt(reis.reisNumber.ToString(), "//2", txtL, L);
                                Console.WriteLine("На рейс " + reis.reisNumber.ToString() + " открыта посадка");
                            }
                            catch
                            {
                                Console.WriteLine("Служба регистрации и/или табло не доступны->открытm посадку не удалось на рейс " + reis.reisNumber.ToString() + " ->пробуем еще раз.");
                                b = false;
                            }
                        }
                    }
                    if (curMin >= reis.timeStart && !s && pb && b)
                    {
                        if (!pf)
                        {
                            s = true;
                            pf = true;
                            try
                            {
                                int message = ChangeSt(reis.plain.ToString(), "//8", txtV, V);
                                if (message == 0)
                                {
                                    pf = false;
                                    s = false;
                                    Console.WriteLine("Cамолет, связанный с рейсом " + reis.reisNumber.ToString() + ", не может уйти на взлет.");
                                }
                                else
                                {
                                    Console.WriteLine("Cамолет, связанный с рейсом " + reis.reisNumber.ToString() + ", ушел на взлет.");
                                }
                            }
                            catch
                            {
                                pf = false;
                                s = false;
                                Console.WriteLine("Cамолет, связанный с рейсом " + reis.reisNumber.ToString() + ", не доступен.->Не улетел->пробуем еще раз.");

                            }
                        }
                        if (pf)
                        {
                            try
                            {
                                ChangeSt(reis.reisNumber.ToString(), "//3", txtA, A);
                                //  Алине, что посадка закончена
                                Console.WriteLine("Информация, что самолет, связанный с рейсом " + reis.reisNumber.ToString() + ", ушел на взлет, передана табло");
                            }
                            catch
                            {
                                Console.WriteLine("Табло недоступно.");
                            }
                        }

                    }
                    Thread.Sleep(1000);

                }
                reises.Remove(reis);
                
                return 0;
            }

            else
            {
                while (!s)
                {
                    if (curMin >= reis.timeStop - 10 && !s)
                    {
                        try
                        {
                            easyPlane eP = new easyPlane(reis, 10);
                            var client = new HttpClient();
                            client.BaseAddress = new Uri("http://localhost:7014/");
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            string req = "planes//create";
                            var serializer = new DataContractJsonSerializer(typeof(easyPlane));
                            var stringPayload = JsonConvert.SerializeObject(eP);
                            var content = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                            var result = client.PostAsync(req, content).Result;
                            var body = result.Content.ReadAsStringAsync().Result;
                            s = true;
                            Console.WriteLine("Cамолет, связанный с рейсом " + reis.reisNumber.ToString() + ", создан и скоро прилетит в аэропорт.");

                            //генерация самолета
                        }
                        catch
                        {
                            s = false;
                            Console.WriteLine("Cамолет, связанный с рейсом " + reis.reisNumber.ToString() + ", создать не удалось.->Не прилетит");

                        }
                    }
                    Thread.Sleep(1000);
                    curMin += speed;
                }
                reises.Remove(reis);
                return 0;
            }
        }


    }

    public class Coordinate
    {
        public int x { get; set; }
        public int y { get; set; }
        public string town { get; set; }
        public Coordinate(int x2, int y2, string town2) { x = x2; y = y2; town = town2; }
        public int destenation(int x2, int y2)
        { return Convert.ToInt16((Math.Acos(Math.Sin(x) * Math.Sin(x2) + Math.Cos(x) * Math.Cos(x2) * Math.Cos(y - y2))) * 6371); }
    }

    public class ReisConvertor : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(Reis));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            string frm = (string)jo["frm"];
            string to = (string)jo["to"];
            int timeStart = (int)jo["timeStart"];
            int timeStop = (int)jo["timeStop"];
            int? count = (int?)jo["count"];
            int reisNumber = (int)jo["reisNumber"];
            int? plain = (int?)jo["plain"];
            int? registrtionTime = (int?)jo["registrtionTime"];
            int? boardingTime = (int?)jo["boardingTime"];
            Reis result = new Reis(frm, to, timeStart, timeStop, count, reisNumber, plain, registrtionTime, boardingTime);
            return result;
        }
        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

    }




    public class PlaneConvertor : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(Plane));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            int Id = (int)jo["Id"];
            int PassengersMax = (int)jo["PassengersMax"];
            Plane ep = new Plane(Id, PassengersMax);
            return ep;
        }
        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

    }



    public class Plane
    {
        public int Id;
        public int PassengersMax;
        public Plane(int id, int Count) { Id = id; PassengersMax = Count; }
    }
    [DataContract]
    public class easyPlane
    {
        [DataMember]
        Reis Route;
        [DataMember]
        int Status;
        public easyPlane(Reis r, int s) { Route = r; Status = s; }
    }
    [DataContract]
    public class Reis
    {
        [DataMember]
        public string frm { get; set; }
        [DataMember]
        public string to { get; set; }
        [DataMember]
        public int timeStart { get; set; }
        [DataMember]
        public int timeStop { get; set; }
        [DataMember]
        public int? count { get; set; }
        [DataMember]
        public int reisNumber { get; set; }
        [DataMember]
        public int? plain { get; set; }
        [DataMember]
        public int? registrtionTime { get; set; }
        [DataMember]
        public int? boardingTime { get; set; }


        public Reis(string From, string To, int TimeStart, int TimeStop, int? Count,
            int ReisNumber, int? Plain)
        {
            frm = From;
            to = To;
            timeStart = TimeStart;
            timeStop = TimeStop;
            count = Count;
            reisNumber = ReisNumber;
            plain = Plain;
            registrtionTime = timeStart - 240;
            boardingTime = timeStart - 40;

        }
        //[JsonConstructor]
        public Reis(string From, string To, int TimeStart, int TimeStop, int? Count,
           int ReisNumber, int? Plain, int? RegistrtionTime, int? BoardingTime)
        {
            frm = From;
            to = To;
            timeStart = TimeStart;
            timeStop = TimeStop;
            count = Count;
            reisNumber = ReisNumber;
            plain = Plain;
            registrtionTime = RegistrtionTime;
            boardingTime = BoardingTime;

        }

        public string reisToString()
        {

            return (frm + ";" + to + ";" + timeStart.ToString() + ";" + timeStop.ToString()
                + ";" + count.ToString() + ";" + reisNumber.ToString() + ";" + plain.ToString()
                + ";" + registrtionTime.ToString() + ";" + boardingTime.ToString() + ";");
        }

    }

    

}
