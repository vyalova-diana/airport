using System;
using NLog;
namespace FollowMeBackend
{
    class Program
    {
        static void Main()
        {
            var logger = LogManager.GetCurrentClassLogger();
            logger.Info("Backend Started!!!");
            
            Loop wl = new Loop();
            wl.StartLoop(logger);
        }
    }
}
