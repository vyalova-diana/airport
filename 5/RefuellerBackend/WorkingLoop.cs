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

            string planePos = MakeGetGateRequestCall(null, planeID);    //get plane position (gate)

            MoveRequest mReqv = new MoveRequest("Garage", planePos, "Refueller Serivce", "Refuel1");
            string jsonmReqv = JsonConvert.SerializeObject(mReqv);
            string moveToGatePermission = MakeMoveRequestCall(null, planePos);  //request permission to move to gate

            if (moveToGatePermission.Equals("Obtained"))
            {
                Vehicle.Instance.SetVehicleStatus("2");
                Thread.Sleep(10000);
                Vehicle.Instance.SetVehicleStatus("3");
                Thread.Sleep(time);
                string moveToGaragePermission = MakeMoveRequestCall(null, "Garage");

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

        private string MakeMoveRequestCall(string host, string planePos)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(host);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());

            MoveRequest mReqv = new MoveRequest("Garage", planePos, "Refueller Serivce", "Refuel1");
            string jsonmReqv = JsonConvert.SerializeObject(mReqv);

            streamWriter.Write(jsonmReqv);
            streamWriter.Flush();
            streamWriter.Close();

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            Console.WriteLine("awaiting response");

            var streamReader = new StreamReader(httpResponse.GetResponseStream());
            var result = JsonConvert.DeserializeObject<MoveRequestResult>(streamReader.ReadToEnd());
                
            Console.WriteLine("Post call (move request) to {0} returns {1}", host, result.permission);
            return result.permission;
        }

        private string MakeGetGateRequestCall(string host, string planeID)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(host);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());

            GetGateRequest ggr = new GetGateRequest("Air Facility", planeID);
            string jsonMessage = JsonConvert.SerializeObject(ggr);
            streamWriter.Write(jsonMessage);
            streamWriter.Flush();
            streamWriter.Close();

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            Console.WriteLine("awaiting response");

            var streamReader = new StreamReader(httpResponse.GetResponseStream());
            var result = JsonConvert.DeserializeObject<GetGateRequsetResult>(streamReader.ReadToEnd());

            Console.WriteLine("Post call (get gate request) to {0} returns {1}", host, result.locationCode);
            return result.locationCode;
        }

        

    }

}
