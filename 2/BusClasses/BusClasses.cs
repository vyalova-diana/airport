using System;
using System.Threading;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Text;

namespace BusClasses
{
    public class Bus
    {
        public int busId;
        public int isFree;
        public int flightId;
        public int planeId;
        public int cntpas;
        public int dest;
        public string planelocationcode;

        public BusLog bl = new BusLog();

        public void AllowMoving(string from, string to)
        {
            bl.WriteToLog("Для автобуса " + this.busId + ", движущегося к самолету " + this.planeId
                + " запрошено передвижение от " + from + " до " + to + ".");
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://groundcontrol.v2.vapor.cloud/askForPermission");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"from\":\"" + from + "\"," +
                              "\"to\":\"" + to + "\"," +
                              "\"service\":\"Buss Service\"," +
                              "\"identifier\":\"" + this.busId + "\"}";

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            string response;
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                response = streamReader.ReadToEnd();
            }

            if (response != "{\"permission\" : \"Obtained\"}")
            {
                bl.WriteToLog("Для автобуса " + this.busId + ", движущегося к самолету " + this.planeId
                + " запрос на передвижение от " + from + " до " + to + " поставлен в очередь.");
                bool res = false;
                while (res != true)
                {
                    var file = new FileCSV();
                    var cnt = 0;
                    while (cnt == 0)
                        cnt = file.ExistsInCSVMoving(this.busId);
                    res = true;
                }
            }
            bl.WriteToLog("Для автобуса " + this.busId + ", движущегося к самолету " + this.planeId
                + " разрешено передвижение от " + from + " до " + to + ".");
        }

        public void FindPlaneLocationCode()
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://groundcontrol.v2.vapor.cloud/getTFInformation");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"service\":\"Air Facility\"," + //ну или как там сервис называется
                              "\"identifier\":\"" + this.planeId + "\"}";

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            string response;
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                response = streamReader.ReadToEnd();
            }

            int startlocationcode = response.IndexOf("\"locationCode\"")+16;
            int endlocationcode = response.IndexOf("\",\"status\"");
            this.planelocationcode = response.Substring(startlocationcode, endlocationcode - startlocationcode);
        }

        public void SendLocation (string from, string to, string status)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://groundcontrol.v2.vapor.cloud/updateTFStatus");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"service\":\"Buss Service\"," +
                              "\"identifier\":\"" + this.busId + "\"," +
                              "\"locationCode\":\""+ from + "-" + to + "\"," +
                              "\"status\":\"" + status + "\"}";

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            string response;
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                response = streamReader.ReadToEnd();
            }
        }

        public void BusArivedToAirplane ()
        {
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(this.busId).CopyTo(bytes, 0);

            Guid busId = new Guid(bytes);
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:7001");
            var stringContent = new StringContent(JsonConvert.SerializeObject(busId), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync("api/flightpassengers/busarrivedtoairplane", stringContent).Result;
        }

        public void BusArivedToGate()
        {
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(this.busId).CopyTo(bytes, 0);

            Guid busId = new Guid(bytes);
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:7001");
            var stringContent = new StringContent(JsonConvert.SerializeObject(busId), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync("api/flightpassengers/busarrivedtolandedpassengerstorage", stringContent).Result;
        }

        public void EndMoving()
        {
            var file = new FileCSV();
            file.RemoveFromCSVMoving(this.busId);
        }

        public void Execute()
        {
            var file = new FileCSV();
            var cntformoving = 0;
            while (cntformoving==0)
            {
                cntformoving = file.ReadFromCSVСnts(this.busId);
                Thread.Sleep(500);
            }
            file.RemoveFromCSVCnts(this.busId);
            bl.WriteToLog("Автобусу " + this.busId + ", движущемуся к самолету " + this.planeId
                + " необходимо перевезти " + cntformoving + " пассажиров.");

            Thread.Sleep(10000); //еду
            if (this.dest == 0)
            {
                bl.WriteToLog("Автобус " + this.busId + ", движущийся к самолету " + this.planeId
                  + " прибыл к GT1");
                SendLocation("BGR", "GT1", "Busy");
            }
            else
            {
                bl.WriteToLog("Автобус " + this.busId + ", движущийся к самолету " + this.planeId
                  + " прибыл к " + this.planelocationcode);
                SendLocation("BGR", this.planelocationcode, "Busy");
            }


            //удостоверилась, что сели в автобус все пассажиры
            var checkcntpas = -1;
            while (checkcntpas != cntpas)
            {
                checkcntpas = file.ReadFromCSVPassanger(this.busId);
                Thread.Sleep(500);
            }
            bl.WriteToLog("Автобус " + this.busId + ", движущийся к самолету " + this.planeId
                + " принял на борт всех пассажиров.");

            if (this.dest == 0)
                SendLocation("BGR", "GT1", "Idle");
            else
                SendLocation("BGR", this.planelocationcode, "Idle");
            //запрос на передвижение'
            if (this.dest == 0)
            {
                FindPlaneLocationCode();
                AllowMoving("GT1", this.planelocationcode);
                SendLocation("GT1", this.planelocationcode, "Moving");
            }
            else
            {
                AllowMoving(this.planelocationcode, "GT1");
                SendLocation(this.planelocationcode, "GT1", "Moving");
            }
            Thread.Sleep(10000);//еду
            if (this.dest == 0)
            {
                bl.WriteToLog("Автобус " + this.busId + ", движущийся к самолету " + this.planeId
                    + " прибыл к " + this.planelocationcode + ".");
                SendLocation("GT1", this.planelocationcode, "Busy");
            }
            else
            {
                bl.WriteToLog("Автобус " + this.busId + ", движущийся к самолету " + this.planeId
                    + " прибыл к " + "GT1" + ".");
                SendLocation(this.planelocationcode, "GT1", "Busy");
            }
            //окончание движения
            EndMoving();

            //оповещаю пассажиров в автобусе, что пора сваливать.
            if (dest == 0)
                BusArivedToAirplane();
            else
                BusArivedToGate();
            Thread.Sleep(1000);
            file.RemoveFromCSVPassangerInBus(this.busId);
            bl.WriteToLog("Автобус " + this.busId + ", движущийся к самолету " + this.planeId
                + " пуст. Все пассажиры вышли из автобуса");

            if (dest == 0)
            {
                SendLocation("GT1", this.planelocationcode, "Idle");
                //запрос на передвижение
                AllowMoving(this.planelocationcode, "BGR");
                SendLocation(this.planelocationcode, "BGR", "Moving");
                Thread.Sleep(1000);
                bl.WriteToLog("Автобус " + this.busId + " прибыл на стоянку");
                SendLocation(this.planelocationcode, "BGR", "Busy");
                SendLocation(this.planelocationcode, "BGR", "Idle");
            }
            else
            {
                SendLocation(this.planelocationcode, "GT1", "Idle");
                //запрос на передвижение
                AllowMoving("GT1", "BGR");
                SendLocation("GT1", "BGR", "Moving");
                Thread.Sleep(1000);
                bl.WriteToLog("Автобус " + this.busId + " прибыл на стоянку");
                SendLocation("GT1", "BGR", "Busy");
                SendLocation("GT1", "BGR", "Idle");
            }
            //окончание движения
            EndMoving();
            //освободила автобус
            file.ChangeBusStatusCSV(this.busId);
            bl.WriteToLog("Автобус " + this.busId + " свободен.");

        }

    }



    public class Task
    {
        public int busId;
        public int planeId;
    }
    public class Passanger
    {
        public int passangerId;
        public int busId;
    }
    public class PermissonForMoving
    {
        public int identifier;
        public string from;
        public string to;
    }
    public class PasCnts
    {
        public int busId;
        public int cnt;
    }

    public class BusLog
    {
        public string filelog = "../BusLog.txt";
        public void WriteToLog (string text)
        {
            bool done = false;
            while (done == false)
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(text);
                    File.AppendAllText(filelog, sb.ToString());
                    done = true;
                }
                catch (Exception e) { }
            }
        }
        public void ClearAll()
        {
            File.WriteAllText(filelog, String.Empty);
        }
    }

    public class FileCSV
    {


        public string delimiter = ";";
        public string filepathpassengers = "../BusPassengers.csv";
        public string filepathbusmoving = "../BusMoving.csv";
        public string filepathbus = "../Bus.csv";
        public string filepathcnts = "../BusPasCnts.csv";

        public void WriteToCSVBus(int busId, int free)
        {
            bool done = false;
            while (done == false)
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    //1-free, 0-busy
                    sb.AppendLine(busId + delimiter + free + delimiter);
                    File.AppendAllText(filepathbus, sb.ToString());
                    done = true;
                }
                catch (Exception e) { }
            }
        } //вписать автобус
        public void WriteToCSVPassanger(Passanger pas)
        {
            bool done = false;
            while (done == false)
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(pas.passangerId + delimiter + pas.busId + delimiter);
                    File.AppendAllText(filepathpassengers, sb.ToString());
                    done = true;
                }
                catch (Exception e) { }
            }
        } //вписать пассажира
        public void WriteToCSVMoving(int busId)
        {
            bool done = false;
            while (done == false)
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(busId + delimiter);
                    File.AppendAllText(filepathbusmoving, sb.ToString());
                    done = true;
                }
                catch (Exception e) { }
            }
        } //вписать разрешение на передвижение
        public void WriteToCSVCnts(int busId, int cnt)
        {
            bool done = false;
            while (done == false)
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(busId + delimiter + cnt + delimiter);
                    File.AppendAllText(filepathcnts, sb.ToString());
                    done = true;
                }
                catch (Exception e) { }
            }
        } //вписать перевозимое количество для автобуса

        public void ClearAll()
        {
            File.WriteAllText(filepathpassengers, String.Empty);
            File.WriteAllText(filepathbusmoving, String.Empty);
            File.WriteAllText(filepathbus, String.Empty);
            File.WriteAllText(filepathcnts, String.Empty);
        }

        public void RemoveFromCSVPassanger()
        {
            bool done = false;
            while (done == false)
            {
                try
                {
                    var Passangers = ReadFromCSVPassanger();
                    Passangers.RemoveAt(0);
                    File.WriteAllText(filepathpassengers, String.Empty);
                    foreach (Passanger pas in Passangers)
                    {
                        WriteToCSVPassanger(pas);
                    }
                    done = true;
                }
                catch (Exception e) { }
            }
        } //убираю самого старого пассажира
        public void RemoveFromCSVPassanger(int pasId)
        {
            var Passangers = ReadFromCSVPassanger();
            for (int i = 0; i < Passangers.Count; i++)
            {
                if (Passangers[i].passangerId == pasId)
                {
                    Passangers.RemoveAt(i);
                }
            }
            bool done = false;
            while (done == false)
            {
                try
                {
                    File.WriteAllText(filepathpassengers, String.Empty);
                    foreach (Passanger pas in Passangers)
                    {
                        WriteToCSVPassanger(pas);
                    }
                    done = true;
                }
                catch (Exception e) { }
            }

        } //убираю пасссажира по id
        public void RemoveFromCSVPassangerInBus(int busId)
        {
            var Passangers = ReadFromCSVPassanger();
            for (int i = 0; i < Passangers.Count; i++)
            {
                if (Passangers[i].busId == busId)
                {
                    Passangers.RemoveAt(i);
                }
            }
            bool done = false;
            while (done == false)
            {
                try
                {
                    File.WriteAllText(filepathpassengers, String.Empty);
                    foreach (Passanger pas in Passangers)
                    {
                        WriteToCSVPassanger(pas);
                    }
                    done = true;
                }
                catch (Exception e) { }
            }

        } //убираю пасссажира по id
        public void RemoveFromCSVMoving(int busId)
        {
            var Moving = ReadFromCSVMoving();
            for (int i = 0; i < Moving.Count; i++)
            {
                if (Moving[i] == busId)
                {
                    Moving.RemoveAt(i);
                }
            }

            bool done = false;
            while (done == false)
            {
                try
                {
                    File.WriteAllText(filepathbusmoving, String.Empty);
                    foreach (int id in Moving)
                    {
                        WriteToCSVMoving(id);
                    }
                    done = true;
                }
                catch (Exception e) { }
            }
        } //убираю задание на передвижение по id автобуса
        public void RemoveFromCSVCnts(int busId)
        {
            var PasCnts = ReadFromCSVСnts();
            for (int i = 0; i < PasCnts.Count; i++)
            {
                if (PasCnts[i].busId == busId)
                {
                    PasCnts.RemoveAt(i);
                }
            }

            bool done = false;
            while (done == false)
            {
                try
                {
                    File.WriteAllText(filepathcnts, String.Empty);
                    foreach (PasCnts pcnt in PasCnts)
                    {
                        WriteToCSVCnts(pcnt.busId, pcnt.cnt);
                    }
                    done = true;
                }
                catch (Exception e) { }
            }
        } //убираю количество перевозимых по id автобуса

        public void ChangeBusStatusCSV(int busId)
        {
            var Busses = ReadFromCSVBus();
            for (int i = 0; i < Busses.Count; i++)
            {
                if (Busses[i].busId == busId)
                {
                    if (Busses[i].isFree == 1)
                        Busses[i].isFree = 0;
                    else
                        Busses[i].isFree = 1;
                }
            }

            bool done = false;
            while (done == false)
            {
                try
                {
                    File.WriteAllText(filepathbus, String.Empty);
                    foreach (Bus b in Busses)
                    {
                        WriteToCSVBus(b.busId, b.isFree);
                    }
                    done = true;
                }
                catch (Exception e) { }
            }
        } //изменяю статус автобуса (свободен/ занят)

        public List<Passanger> ReadFromCSVPassanger()
        {
            bool done = false;
            List<Passanger> allPassangers = new List<Passanger>();
            while (done == false)
            {
                try
                {
                    var reader = new StreamReader(filepathpassengers);
                    int counter = 0;
                    while (reader.ReadLine() != null)
                    {
                        counter++;
                    }
                    reader.Close();
                    reader = new StreamReader(filepathpassengers);
                    int k = 0;
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(';');
                        var temp = new Passanger();
                        temp.passangerId = Convert.ToInt32(values[0]);
                        temp.busId = Convert.ToInt32(values[1]);
                        allPassangers.Add(temp);
                        k++;
                    }
                    reader.Close();
                    done = true;
                }
                catch (Exception e) { }
            }
            return allPassangers;
        } //прочитать всех пассажиров
        public int ReadFromCSVPassanger(int busId)
        {
            var passengers = ReadFromCSVPassanger();
            int cnt = 0;
            foreach (Passanger pas in passengers)
                if (pas.busId == busId)
                    cnt++;
            return cnt;
        } //прочитать количество пассажиров в автобусе
        public List<int> ReadFromCSVMoving()
        {
            bool done = false;
            List<int> allMoving = new List<int>();
            while (done == false)
            {
                try
                {
                    var reader = new StreamReader(filepathbusmoving);
                    int counter = 0;
                    while (reader.ReadLine() != null)
                    {
                        counter++;
                    }
                    reader.Close();
                    reader = new StreamReader(filepathbusmoving);

                    int k = 0;
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(';');
                        int busId = Convert.ToInt32(values[0]);
                        allMoving.Add(busId);
                        k++;
                    }
                    reader.Close();
                    done = true;
                }
                catch (Exception e) { }
            }
            return allMoving;
        } //прочитать все втобусы,которым разрешено ехать
        public List<Bus> ReadFromCSVBus()
        {
            bool done = false;
            List<Bus> allBusses = new List<Bus>();
            while (done == false)
            {
                try
                {
                    var reader = new StreamReader(filepathbus);
                    int counter = 0;
                    while (reader.ReadLine() != null)
                    {
                        counter++;
                    }
                    reader.Close();
                    reader = new StreamReader(filepathbus);
                    int k = 0;
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(';');
                        var temp = new Bus();
                        temp.busId = Convert.ToInt32(values[0]);
                        temp.isFree = Convert.ToInt32(values[1]);
                        allBusses.Add(temp);
                        k++;
                    }
                    reader.Close();
                    done = true;
                }
                catch (Exception e) { }
            }
            return allBusses;
        } //прочитать все автобусы
        public List<PasCnts> ReadFromCSVСnts()
        {
            bool done = false;
            List<PasCnts> allCnts = new List<PasCnts>();
            while (done == false)
            {
                try
                {
                    var reader = new StreamReader(filepathcnts);
                    int counter = 0;
                    while (reader.ReadLine() != null)
                    {
                        counter++;
                    }
                    reader.Close();
                    reader = new StreamReader(filepathcnts);
                    int k = 0;
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(';');
                        var temp = new PasCnts();
                        temp.busId = Convert.ToInt32(values[0]);
                        temp.cnt = Convert.ToInt32(values[1]);
                        allCnts.Add(temp);
                        k++;
                    }
                    reader.Close();
                    done = true;
                }
                catch (Exception e) { }
            }
            return allCnts;
        } //прочитать все количества

        public int ReadFromCSVСnts(int busId)
        {
            var cnts = ReadFromCSVСnts();
            int cnt = 0;
            foreach (PasCnts c in cnts)
                if (c.busId == busId)
                    cnt = c.cnt;
            return cnt;
        } //прочитать количество для конкретного автобуса

        public int ExistsInCSVMoving(int busId)
        {
            var listMoving = ReadFromCSVMoving();
            var cnt = 0;
            foreach (int i in listMoving)
                if (i == busId)
                    cnt++;
            return cnt;
        } //проверка, разрешено ли автобусу ехать
    }
}
