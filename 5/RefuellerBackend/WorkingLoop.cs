using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace RefuelBackend
{
    class WorkingLoop
    {
        private static int controllerCounter = 1;
        bool invoke = false;
        public void StartLoop()
        {
            Console.WriteLine("Init loop file manager");


            Console.WriteLine(Vehicle.Instance.GetVehicleStatus());
            Console.WriteLine("Starting working loop");

            while (true)
            {
                string s = FileManager.Instance.Get(controllerCounter, "../../../../controllerStatus.txt", false);
                Console.WriteLine("Got controller input");
                Console.WriteLine(s);

                if (s.Equals("0"))
                {
                    Console.WriteLine("no input");
                    Thread.Sleep(5000);
                }
                else if (s.Equals("error"))
                {
                    Console.WriteLine("bad input");
                    Thread.Sleep(5000);
                }
                else
                {
                    Console.WriteLine("got input");
                    string[] splitResult = s.Split(' ');
                    int fuelNeeded = Convert.ToInt32(splitResult[2]);
                    string planeID = splitResult[1];
                    Console.WriteLine("starting thread");

                    ThreadPool.QueueUserWorkItem(delegate { ExecuteRefuelling(fuelNeeded, planeID); });

                    controllerCounter++;
                }
            }
        }

        public void ExecuteRefuelling(int fuelNeeded, string planeID)
        {
            Console.WriteLine("Thread started");
            Console.WriteLine("fuelNeeded is {0} planeid is {1}", fuelNeeded, planeID);
            var time = Vehicle.Instance.CountRefuelTime(fuelNeeded);

            GetGateRequest ggReqv = new GetGateRequest("Air Facility", planeID);
            string jsonggReqv = JsonConvert.SerializeObject(ggReqv);
            string planePos = MakeApiCall(null, jsonggReqv, null);    //get plane position (gate)

            MoveRequest mReqv = new MoveRequest("Garage", planePos, "Refueller Serivce", "Refuel1");
            string jsonmReqv = JsonConvert.SerializeObject(mReqv);
            string moveToGatePermission = MakeApiCall(null, jsonmReqv, null);  //request permission to move to gate

            if (moveToGatePermission.Equals("Obtained"))
            {
                Vehicle.Instance.SetVehicleStatus("2");
                Thread.Sleep(10000);
                Vehicle.Instance.SetVehicleStatus("3");
                Thread.Sleep(time);
                string moveToGaragePermission = MakeApiCall(null, null, null);

                if (moveToGaragePermission.Equals("Obtained"))
                {
                    Vehicle.Instance.SetVehicleStatus("2");
                    Thread.Sleep(10000);
                    Vehicle.Instance.SetVehicleStatus("0");
                }
                else if (moveToGatePermission.Equals("Queued"))
                {
                    Vehicle.Instance.SetVehicleStatus("4");
                    while (!invoke)
                    {
                        Thread.Sleep(1000);
                    }
                    invoke = false;
                }
                else if (moveToGatePermission.Equals("Denied"))
                {

                }
                else
                {
                    //something
                }
            }
            else if (moveToGatePermission.Equals("Queued"))
            {
                Vehicle.Instance.SetVehicleStatus("1");
                while (!invoke)
                {
                    Thread.Sleep(1000);
                }
                invoke = false;

            }
            else if (moveToGatePermission.Equals("Denied"))
            {

            }
            else
            {
                //something
            }
            //calls to other apis
        }

        private string MakeMoveRequestCall(string host, string jsonMessage)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(host);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());


            streamWriter.Write(jsonMessage);
            streamWriter.Flush();
            streamWriter.Close();

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            Console.WriteLine("awaiting response");

            var streamReader = new StreamReader(httpResponse.GetResponseStream());
            var result = JsonConvert.DeserializeObject<MoveRequestResult>(streamReader.ReadToEnd());
                
            Console.WriteLine("Post call to {0} returns {1}", host, result.permission);
            return result.permission;
        }



        private string MakeApiCall(string host, string jsonMessage, string requestMethod)
        {
            switch (requestMethod)
            {
                case "POST":
                    {
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(host);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";

                        var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());


                        streamWriter.Write(jsonMessage);
                        streamWriter.Flush();
                        streamWriter.Close();

                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        Console.WriteLine("awaiting response");

                        var streamReader = new StreamReader(httpResponse.GetResponseStream());
                        var result = streamReader.ReadToEnd();

                        Console.WriteLine("Post call to {0} returns {1}", host, result);
                        return result;
                    }
                case "GET":
                    {
                        string str = null;
                        var req = new StreamReader(HttpWebRequest.Create(host).GetResponse().GetResponseStream());
                        str = req.ReadToEnd();
                        Console.WriteLine("Get call to {0} returns {1}", host, str);
                        return str;
                    }
                default:
                    return "should not get this";
            }
        }

    }

}
