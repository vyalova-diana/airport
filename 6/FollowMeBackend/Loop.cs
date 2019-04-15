using System;
using System.Threading;
using NLog;
using FollowMeBackend.HttpClients;
using FollowMeBackend.HttpClients.GroundControlClient;

namespace FollowMeBackend
{
    internal class Loop
    {
        private static int controllerCounter = 1;
        public Logger logger;
        public void StartLoop(Logger log)
        {

            logger = log;
            logger.Info("Init loop file manager");
            logger.Info(Vehicle.Instance.GetVehicleStatus().ToString());

            logger.Info("Starting loop");

            while (true)
            {
                logger.Info(controllerCounter);
                string s = FileManager.Instance.Get(controllerCounter, "../../../../controllerStatus.txt");
                logger.Info("Got controller input:"+s);
                

                if (s.Equals("0"))
                {
                    logger.Info("=> no input");
                    controllerCounter++;
                    Thread.Sleep(5000);

                }
                else if (s.Equals("empty line"))
                {
                    logger.Info("=> stay 0");
                    FileManager.Instance.Set("0", "../../../../controllerStatus.txt", true);
                   
                    Thread.Sleep(5000);
                }
                else
                {
                    logger.Info("=> got input");
                    string planeID = s;
                    logger.Info("starting thread");

                    ThreadPool.QueueUserWorkItem(delegate { ExecuteEscort(planeID); });
                    controllerCounter++;
                }
                
            }

        }

        public void ExecuteEscort(string planeID)
        {
            logger.Info("Thread started");
            logger.Info("Airplane ID is {0}", planeID);

            //начальный статус "в гараже" idle, потом гейт idle
            var flag = true;
            while (flag)
            {

                var resp = GroundControlClient.StatusUpdate(Vehicle.Instance.stat);

                if (resp.success == "false")
                {
                    logger.Error("StatusUpdate: " + resp.description);
                    Thread.Sleep(5000);
                }
                else
                {
                    
                    logger.Info("StatusUpdate: " + resp.success + (Vehicle.Instance.stat).ToString());
                    flag = false;
                }
            }


            //найти где самолёт getTFInformation
            
            flag = true;
            var loc = "";
            var req_loc = new LocateAirplaneRequest();
            req_loc.identifier = planeID;
            while (flag)
            {
                
                var resp = GroundControlClient.FindAirplane(req_loc);

                if (resp.result == "Not found")
                {
                    logger.Error("Airplane "+planeID+" location");
                    Thread.Sleep(5000);
                }
                else
                {
                    logger.Info("Airplane: " + resp.identifier +" at "+ resp.locationCode);
                    loc = resp.locationCode;
                    flag = false;
                }
            }

            //запросить разрешение на передвижение к самолёту
            flag = true;
            var req_perm = new PermissionRequest
            {
                from = Vehicle.Instance.GetVehicleStatus().locationCode,
                to = loc,
                service = Vehicle.Instance.GetVehicleStatus().service,
                identifier = Vehicle.Instance.GetVehicleStatus().identifier
            };
            while (flag)
            {

                var resp = GroundControlClient.AskPermission(req_perm);

                if (resp.permission == "Denied")
                {
                    logger.Error("Permission from "+req_perm.from+"to"+req_perm.to+"DENIED" );
                    Thread.Sleep(5000);
                }
                else
                {
                    logger.Info("Permission from: " + req_perm.from + "to" + req_perm.to +resp.permission);
                    
                    flag = false;
                }
            }

            //поменять статус на "в дороге" к самолёту
            flag = true;
            var stat= new Status
            {
                identifier=Vehicle.Instance.stat.identifier,
                locationCode=req_perm.from+"-"+req_perm.to,
                status= "On road"
            };
            while (flag)
            {

                var resp = GroundControlClient.StatusUpdate(stat);

                if (resp.success == "false")
                {
                    logger.Error("StatusUpdate: " + resp.description);
                    Thread.Sleep(5000);
                }
                else
                {
                    Vehicle.Instance.SetVehicleStatus(stat);
                    logger.Info("StatusUpdate: " + resp.success + (Vehicle.Instance.stat).ToString());
                    flag = false;
                }
            }
            //таймаут(еду до самолёта)
            logger.Info("On road....");
            Thread.Sleep(10000);
            //контакт с самолётом


            /////////////////////////////////////////////////////


            //запросить разрешение на движение к гейту
            flag = true;
            var req_perm2 = new PermissionRequest
            {
                from = req_perm.to,
                to = "Gate",
                service = Vehicle.Instance.GetVehicleStatus().service,
                identifier = Vehicle.Instance.GetVehicleStatus().identifier
            };
            while (flag)
            {

                var resp = GroundControlClient.AskPermission(req_perm2);

                if (resp.permission == "Denied")
                {
                    logger.Error("Permission from " + req_perm2.from + "to" + req_perm2.to + "DENIED");
                    Thread.Sleep(5000);
                }
                else
                {
                    logger.Info("Permission from: " + req_perm2.from + "to" + req_perm2.to + resp.permission);

                    flag = false;
                }
            }

            //поменять статус на "в дороге" к гейту
            flag = true;
            var stat2 = new Status
            {
                identifier = Vehicle.Instance.stat.identifier,
                locationCode = req_perm2.from + "-" + req_perm2.to,
                status = "On road"
            };
            while (flag)
            {

                var resp = GroundControlClient.StatusUpdate(stat2);

                if (resp.success == "false")
                {
                    logger.Error("StatusUpdate: " + resp.description);
                    Thread.Sleep(5000);
                }
                else
                {
                    Vehicle.Instance.SetVehicleStatus(stat2);
                    logger.Info("StatusUpdate: " + resp.success + (Vehicle.Instance.stat).ToString());
                    flag = false;
                }
            }
            //через таймаут поддерживать связь с самолётом
            //сообщить самолёту, что приехали
            //сообщить 8, что выполнено
            //поменять статус на "idle" у гейта
            flag = true;
            var stat3 = new Status
            {
                identifier = Vehicle.Instance.stat.identifier,
                locationCode = req_perm2.to,
                status = "Idle"
            };
            while (flag)
            {

                var resp = GroundControlClient.StatusUpdate(stat3);

                if (resp.success == "false")
                {
                    logger.Error("StatusUpdate: " + resp.description);
                    Thread.Sleep(5000);
                }
                else
                {
                    Vehicle.Instance.SetVehicleStatus(stat3);
                    logger.Info("StatusUpdate: " + resp.success + (Vehicle.Instance.stat).ToString());
                    flag = false;
                }
            }




        }
    }
}