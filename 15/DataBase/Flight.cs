using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;

namespace DataBase
{
    public class Flight
    {
        private int id;
        private string from;
        private string to;
        private int timeStart;
        private int timeStop;
        private int status;

        public int Id { get => id; set => id = value; }
        public string From { get => from; set => from = value; }
        public string To { get => to; set => to = value; }
        public int TimeStart { get => timeStart; set => timeStart = value; }
        public int TimeStop { get => timeStop; set => timeStop = value; }
        public int Status { get => status; set => status = value; }

        public Flight(Reis r)
        {
            from = r.frm;
            to = r.to;
            timeStart = r.timeStart;
            timeStop = r.timeStop;
            id = r.reisNumber;
            status = 0;
        }

        public Flight()
        {

        }
    }

    public class ReisConvertor : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(Reis));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Load the JSON for the Result into a JObject 
            JObject jo = JObject.Load(reader);

            // Read the properties which will be used as constructor parameters 

            string frm = (string)jo["frm"];
            string to = (string)jo["to"];
            int timeStart = (int)jo["timeStart"];
            int timeStop = (int)jo["timeStop"];
            int? count = (int?)jo["count"];
            int reisNumber = (int)jo["reisNumber"];
            int? plain = (int?)jo["plain"];
            int? registrtionTime = (int?)jo["registrtionTime"];
            int? boardingTime = (int?)jo["boardingTime"];
            // Construct the Result object using the non-default constructor 
            Reis result = new Reis(frm, to, timeStart, timeStop, count, reisNumber, plain, registrtionTime, boardingTime);

            // (If anything else needs to be populated on the result object, do that here) 

            // Return the result 
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

    [DataContract]
    public class Reis
    {
        [DataMember]
        // [JsonProperty ("From")] 
        public string frm { get; set; }
        [DataMember]
        //[JsonProperty ("To")] 
        public string to { get; set; }
        [DataMember]
        //[JsonProperty ("TimeStart")] 
        public int timeStart { get; set; }
        [DataMember]
        //[JsonProperty ("TimeStop")] 
        public int timeStop { get; set; }
        [DataMember]
        //[JsonProperty ("Count")] 
        public int? count { get; set; }
        [DataMember]
        //[JsonProperty ("ReisNumber")] 
        public int reisNumber { get; set; }
        [DataMember]
        // [JsonProperty ("Plain")] 
        public int? plain { get; set; }
        [DataMember]
        // [JsonProperty ("RegistrtionTime")] 
        public int? registrtionTime { get; set; }
        [DataMember]
        //[JsonProperty ("BoardingTime")] 
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
