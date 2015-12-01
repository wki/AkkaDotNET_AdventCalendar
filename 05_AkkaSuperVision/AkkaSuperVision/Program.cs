using System;
using Akka.Actor;
using System.Threading;

namespace AkkaSuperVision
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var system = ActorSystem.Create("Hooks");

            var supervisor = system.ActorOf(
                Props.Create<Supervisor>(),
                // Props.Create<Supervisor>().WithSupervisorStrategy( ... ),
                "Supervisor");

            for (var i=0; i<500; i++)
                supervisor.Tell("message " + i);


            Thread.Sleep(500);
            Console.WriteLine("Press [enter] to continue...");
            Console.ReadLine();

            system.Shutdown();
            system.AwaitTermination();
        }
    }
}
