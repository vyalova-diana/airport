using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace RefuelBackend
{
    public sealed class Vehicle
    {
        Vehicle() { }

        private static readonly object singleLock = new object();

        private static Vehicle instance = null;

        public static Vehicle Instance
        {
            get
            {
                lock (singleLock)
                {
                    if (instance == null)
                    {
                        instance = new Vehicle();
                    }
                    return instance;
                }
            }
        }

        public int CountRefuelTime(int fuelNeeded)
        {
            return 1000 + (fuelNeeded * 1000);
        }

        public void RefuelAirplane(int fuelNeeded)
        {
            Thread.Sleep(CountRefuelTime(fuelNeeded));
        }

        public void SetVehicleStatus(string status)
        {
            FileManager.Instance.Set(status, "../vehicleStatus.txt", false);
        }

        public string GetVehicleStatus()
        {
            return FileManager.Instance.Get(1, "../vehicleStatus.txt", false);
        }
    }
}
