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
            logger.Info("Location: "+Vehicle.Instance.GetVehicleStatus().locationCode+" Status:"+ Vehicle.Instance.GetVehicleStatus().status);

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
                    FileManager.Instance.Set("0", "../../../../controllerStatus.txt", true);

                    logger.Info("=> stay 0");
                    
                    Thread.Sleep(5000);
                }
                else
                {
                    logger.Info("=> got input");
                    string planeID = s;
                    logger.Info("starting thread");

                    ThreadPool.QueueUserWorkItem(delegate { ExecuteEscort(planeID); });
                    controllerCounter++;
                    Thread.Sleep(5000);
                }
                
            }

        }

        public void ExecuteEscort(string planeID)
        {
            logger.Info("Thread started");
            logger.Info("Airplane ID is {0}", planeID);

            //7 начальный статус "в гараже" Busy
            var flag = true;
            while (flag)
            {

                var resp = GroundControlClient.StatusUpdate(Vehicle.Instance.stat);

                if (resp.error == "true")
                {
                    logger.Error("StatusUpdate: " + resp.description);
                    Thread.Sleep(5000);
                }
                else
                {
                    
                    logger.Info("StatusUpdate: OK" + "Location: " + Vehicle.Instance.GetVehicleStatus().locationCode + " Status:" + Vehicle.Instance.GetVehicleStatus().status);
                    flag = false;
                }
            }


            //7 найти где самолёт getTFInformation
            
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

            //7 запросить разрешение на передвижение к самолёту
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

            //7 поменять статус на "в дороге" к самолёту
            flag = true;
            var stat= new Status
            {
                identifier=Vehicle.Instance.stat.identifier,
                locationCode=req_perm.from+"-"+req_perm.to,
                status= "Moving"
            };
            while (flag)
            {

                var resp = GroundControlClient.StatusUpdate(stat);

                if (resp.error == "true")
                {
                    logger.Error("StatusUpdate: " + resp.description);
                    Thread.Sleep(5000);
                }
                else
                {
                    Vehicle.Instance.SetVehicleStatus(stat);
                    
                    logger.Info("StatusUpdate: OK"  + "Location: " + Vehicle.Instance.GetVehicleStatus().locationCode + " Status:" + Vehicle.Instance.GetVehicleStatus().status);
                    flag = false;
                }
            }
            //таймаут(еду до самолёта)
            logger.Info("Moving....");
            Thread.Sleep(7000);
            //7 поменять статус на "Busy" на ВПП
            flag = true;
            var stat1 = stat;
            stat1.status = "Busy";
            stat1.locationCode = req_perm.to;
            
            while (flag)
            {

                var resp = GroundControlClient.StatusUpdate(stat1);

                if (resp.error == "true")
                {
                    logger.Error("StatusUpdate: " + resp.description);
                    Thread.Sleep(5000);
                }
                else
                {
                    Vehicle.Instance.SetVehicleStatus(stat1);

                    logger.Info("StatusUpdate: OK"  + "Location: " + stat1.locationCode + " Status:" + stat1.status);
                    flag = false;
                }
            }
            //14 контакт с самолётом /plane/ready_followme/*id*
            flag = true;
            while (flag)
            {

                var resp = AirplaneClient.GetStatus(planeID);

                if (resp != "1")
                {
                    logger.Error("Airplane is not ready to go");
                    Thread.Sleep(5000);
                }
                else
                {
                    //обновить статус самолёта на 2(прикреплён)
                    var podflag = true;
                    while (podflag)
                    {

                        var resp2 = AirplaneClient.StatusUpdate(planeID, "2");

                        if (resp2 == "0")
                        {
                            logger.Error("Direct airplane StatusUpdate");
                            Thread.Sleep(5000);
                        }
                        else
                        {

                            logger.Info("Direct airplane StatusUpdate: OK 2");
                            podflag = false;
                        }
                    }

                    logger.Info("Airplane ready to go");
                    flag = false;
                }
            }

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
                service= Vehicle.Instance.GetVehicleStatus().service,
                identifier = Vehicle.Instance.GetVehicleStatus().identifier,
                locationCode = req_perm2.from + "-" + req_perm2.to,
                status = "Moving"
            };
            while (flag)
            {

                var resp = GroundControlClient.StatusUpdate(stat2);

                if (resp.error == "true")
                {
                    logger.Error("StatusUpdate: " + resp.description);
                    Thread.Sleep(5000);
                }
                else
                {
                    Vehicle.Instance.SetVehicleStatus(stat2);
                    logger.Info("StatusUpdate: OK"  + "Location: " + stat2.locationCode + " Status:" + stat2.status);
                   
                    flag = false;
                }
            }
            //поменять статус самолёта  на "в дороге" к гейту
            flag = true;
            var stat_pl1 = new Status
            {
                service= "Air Facility",
                identifier = planeID,
                locationCode = req_perm2.from + "-" + req_perm2.to,
                status = "Moving"
            };
            while (flag)
            {

                var resp = GroundControlClient.StatusUpdate(stat_pl1);

                if (resp.error == "true")
                {
                    logger.Error("StatusUpdate: " + "Airplane:" + planeID);
                    Thread.Sleep(5000);
                }
                else
                {
                    
                    logger.Info("StatusUpdate: OK" +"Airplane:" + planeID + "Location: " + stat_pl1.locationCode + " Status:" + stat_pl1.status);

                    flag = false;
                }
            }
            //через таймаут поддерживать связь с самолётом(Move)
            for (int i = 0; i < 5; i++)
            {
                flag = true;
                while (flag)
                {

                    var resp = AirplaneClient.IsFollowing(planeID);

                    if (resp == "0")
                    {
                        logger.Error("Airplane " + planeID + "is not following"+"step#" + i.ToString());
                        Thread.Sleep(5000);
                    }
                    else
                    {

                        logger.Info("Airplane " + planeID + "is following"+ "step#"+ i.ToString());
                        flag = false;
                    }
                }
            }
            

            //7 поменять статус на "idle" у гейта
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

                if (resp.error == "true")
                {
                    logger.Error("StatusUpdate: " + resp.description);
                    Thread.Sleep(5000);
                }
                else
                {
                    Vehicle.Instance.SetVehicleStatus(stat3);
                    logger.Info("StatusUpdate: OK"  + "Location: " + stat3.locationCode + " Status:" + stat3.status);
                    flag = false;
                }
            }
            //14 Сообщить самолёту что приехали (статус 3)
            flag = true;
            while (flag)
            {

                var resp = AirplaneClient.StatusUpdate(planeID, "3");

                if (resp == "0")
                {
                    logger.Error("Direct airplane StatusUpdate");
                    Thread.Sleep(5000);
                }
                else
                {
                    
                    logger.Info("Direct airplane StatusUpdate: OK 3");
                    flag = false;
                }
            }
            //7 поменять статус самолёта на "Busy" у гейта
            flag = true;
            var stat_pl2 = stat3;
            stat_pl2.service = "Air Facility";
            stat_pl2.identifier = planeID;
            stat_pl2.status = "Busy";
            while (flag)
            {

                var resp = GroundControlClient.StatusUpdate(stat_pl2);

                if (resp.error == "true")
                {
                    logger.Error("StatusUpdate: " + resp.description);
                    Thread.Sleep(5000);
                }
                else
                {
                    
                    logger.Info("StatusUpdate: OK"+ "Airplane:" + planeID + "Location: " + stat_pl2.locationCode + " Status:" + stat_pl2.status);
                    flag = false;
                }
            }

            //сообщить 8, что выполнено
            //
            //
            //
            //
            //

            //7 запросить разрешение уехать в гараж
            flag = true;
            var req_perm3 = req_perm2;
            req_perm3.from = stat3.locationCode;
            req_perm3.to = "Garage";

            while (flag)
            {

                var resp = GroundControlClient.AskPermission(req_perm3);

                if (resp.permission == "Denied")
                {
                    logger.Error("Permission from " + req_perm3.from + " to " + req_perm3.to + " DENIED");
                    Thread.Sleep(5000);
                }
                else
                {
                    logger.Info("Permission from: " + req_perm3.from + " to " + req_perm3.to +" "+ resp.permission);

                    flag = false;
                }
            }
            //обновить статус на Moving в гараж
            flag = true;
            var stat4 = stat2;
            stat4.locationCode = stat3.locationCode + "-" + req_perm3.to;
            
            while (flag)
            {

                var resp = GroundControlClient.StatusUpdate(stat4);

                if (resp.error == "true")
                {
                    logger.Error("StatusUpdate: " + resp.description);
                    Thread.Sleep(5000);
                }
                else
                {
                    Vehicle.Instance.SetVehicleStatus(stat4);
                    logger.Info("StatusUpdate: OK"+ "Location: " + stat4.locationCode + " Status:" + stat4.status);
                    flag = false;
                }
            }
            //таймаут(еду в гараж)
            logger.Info("Moving....");
            Thread.Sleep(7000);
            //обновить статус Idle  в гараже
            flag = true;
            var stat5 = stat3;
            stat5.locationCode = req_perm3.to;
            
            while (flag)
            {

                var resp = GroundControlClient.StatusUpdate(stat5);

                if (resp.error == "true")
                {
                    logger.Error("StatusUpdate: " + resp.description);
                    Thread.Sleep(5000);
                }
                else
                {
                    Vehicle.Instance.SetVehicleStatus(stat5);
                    logger.Info("StatusUpdate: OK" + "Location: " + stat5.locationCode + " Status:" + stat5.status);
                    flag = false;
                }
            }
        }
    }
}