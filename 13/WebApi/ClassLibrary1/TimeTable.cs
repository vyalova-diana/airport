using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization;

namespace DBlib
{
   


    public class Coordinate
    {
        public int x { get; set; }
        public int y { get; set; }
        public string town { get; set; }
        public Coordinate(int x2, int y2, string town2) { x = x2;y = y2; town = town2; }
        public int destenation(int x2, int y2)
        {return Convert.ToInt16((Math.Acos(Math.Sin(x)*Math.Sin(x2)  + Math.Cos(x) * Math.Cos(x2)* Math.Cos(y - y2)))*6371); }
    }
    [DataContract]
    public class Reis
    {
        [DataMember]
        public string   frm { get; set; }
        [DataMember]
        public string   to { get; set; }
        [DataMember]
        public int      timeStart { get; set; }
        [DataMember]
        public int      timeStop { get; set; }
        [DataMember]
        public int?     count { get; set; }
        [DataMember]
        public int      reisNumber { get; set; }
        [DataMember]
        public int?     plain { get; set; }
        [DataMember]
        public int?     registrtionTime { get; set; }
        [DataMember]
        public int?     boardingTime { get; set; }


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
            
            return (frm +";" + to +";" + timeStart.ToString() + ";" + timeStop.ToString()
                + ";" + count.ToString() + ";" + reisNumber.ToString() + ";" + plain.ToString()
                + ";" + registrtionTime.ToString() + ";" + boardingTime.ToString() + ";");
        }

    }
    public class DB
    {
        public string path = ".\\TimeTable.txt";

        public void DeleteAll()
        {
 
            try
            {
                using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default))
                {
                    sw.WriteLine("");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
 
        }

        public void DeleteOne(int ID)
        {
            List<Reis> Reises = ReadFile();
            foreach (Reis r in Reises)
            {
                if (r.reisNumber != ID)
                {
                    try
                    {
                        using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default))
                        {
                            sw.WriteLine(r.ToString());
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }

        public void SaveAll(List<Reis> someReises)
        {
            string txt = "";
            foreach (Reis r in someReises)
                txt += r.reisToString() + "\n";
            try
            {
                using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default))
                {
                        sw.Write(txt);
                    }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


        public void SaveOne(Reis someReis)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.Default))
                {
                    sw.WriteLine(someReis.reisToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }



        public List<Reis> ReadFile()
        {
            string text = "";
            try
            {
                text = System.IO.File.ReadAllText(path);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            if (text != "")
                return Parse(text);
            return new List<Reis>();

        }


        public List<Reis> Parse(string txt)
        {
            List<Reis> reises = new List<Reis>();
            string[] msg = txt.Split('\n');
            for (int i = 0; i < msg.Length -1; i++)
            {
                string[] concrR = msg[i].Split(';');
                int? a, b, c, d;
                if (concrR[4] == "")
                    a = null;
                else a = Convert.ToInt16(concrR[4]);

                if (concrR[6] == "")
                    b = null;
                else b = Convert.ToInt16(concrR[6]);

                if (concrR[7] == "")
                    c = null;
                else c = Convert.ToInt16(concrR[7]);

                if (concrR[8] == "")
                    d = null;
                else d = Convert.ToInt16(concrR[8]);
                Reis r = new Reis(concrR[0], concrR[1], Convert.ToInt16(concrR[2]), 
                    Convert.ToInt16(concrR[3]), a, Convert.ToInt16(concrR[5]), b, c,d);
                reises.Add(r);
            }
            return reises;
        }
    }
}
