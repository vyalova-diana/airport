using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace CateringDB
{
    public sealed class Car
    {
        Car() { }
        private static int carState = 0;
        private static readonly object singleLock = new object();
        private static Car instance = null;
 
        public static Car Instance
        {
            get
            {
                lock (singleLock)
                {
                    if (instance == null)
                    {
                        instance = new Car();
                    }
                    return instance;
                }
            }
        }

        public int CountFillingTime(int foodNeeded)
        {
            return 1000 + (foodNeeded * 1000);
        }

        public void FillingAirplane(int foodNeeded)
        {
            Thread.Sleep(CountFillingTime(foodNeeded));
        }

        public void SetCarStatus(string status)
        {
            FileStorage.Instance.Set(status, "../CarStatus.txt", false);
        }

        public string GetCarStatus()
        {
            return FileStorage.Instance.Get(1, "../CarStatus.txt", false);
        }
    }
}
