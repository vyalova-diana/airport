using System;

namespace RefuelBackend
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Backend Started");
            WorkingLoop wl = new WorkingLoop();
            wl.StartLoop();
        }
    }
}
