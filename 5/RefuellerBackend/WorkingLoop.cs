using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net;
using System.IO;

namespace RefuelBackend
{
    class WorkingLoop
    {
        private static int controllerCounter = 1;
        
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
                    Thread t = new Thread(()=>ExecuteRefuelling(fuelNeeded, planeID));
                    t.Start();
                    controllerCounter++;
                }
            }
        }

        public void ExecuteRefuelling(int fuelNeeded, string planeID)
        {
            Console.WriteLine("Thread started");
            Console.WriteLine("fuelNeeded is {0} planeid is {1}", fuelNeeded, planeID);
            var time = Vehicle.Instance.CountRefuelTime(fuelNeeded);

            //calls to other apis
        }
    }

}
