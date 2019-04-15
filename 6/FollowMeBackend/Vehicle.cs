using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading;

namespace FollowMeBackend
{
    public sealed class Vehicle
    {
        Vehicle() {
            stat = new Status();
            stat.identifier = "fo8ll8ow";
            stat.locationCode = "Garage";
            stat.status = "Idle";

            var str_stat = stat.ToString();
            FileManager.Instance.Set(str_stat, "../../../../vehicleStatus.txt", false); //перезапись
        }

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

        public Status stat;
        

        public void SetVehicleStatus(Status st)
        {
            //string[] splitResult = status.Split(' ');
            //string serv = splitResult[0];
            //string ID = splitResult[1];
            //string locCode = splitResult[2];
            //string state = splitResult[3];

            
            
            stat.locationCode = st.locationCode;
            stat.status= st.status;

            var str_stat = stat.ToString();
            FileManager.Instance.Set(str_stat, "../../../../vehicleStatus.txt", false); //перезапись
        }

        public Status GetVehicleStatus() 
        {
            return stat;
            //return FileManager.Instance.Get(1, "../../../../vehicleStatus.txt");   //всегда одна строка
        }


    }
}