using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using RefuelBackend;

namespace TestingRequests
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.Sleep(2000);

            //var path = "../../../../something.txt";
            //FileManager fm = new FileManager(path, true);
            //fm.Set("dick heck beck1");
            //fm.Set("dick heck beck2");
            //fm.Set("dick heck beck3");
            //fm.Set("dick heck beck4");
            //fm.Set("dick heck beck5");
            //Thread.Sleep(1000);
            //Console.WriteLine(fm.Get(5));


            var host = "http://localhost:57741/Refueller/status";
            string str = null;
            var req = new StreamReader(HttpWebRequest.Create(host).GetResponse().GetResponseStream());
            str = req.ReadToEnd();
            Console.WriteLine("Status is: {0}", str);


            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:57741/Refueller");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"planeID\":\"1\"," +
                              "\"fuelNeeded\":\"100\"}";

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            Console.WriteLine("awaiting response");
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                Console.WriteLine("Response2: {0}", result);
            }
            Console.ReadKey();
        }
    }
}
