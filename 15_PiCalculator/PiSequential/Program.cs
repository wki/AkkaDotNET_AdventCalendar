using System;
using System.Collections.Generic;
using Akka.Actor;

namespace PiSequential
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var system = ActorSystem.Create("Pi");
            var master = system.ActorOf(Props.Create<Master>());

            master.Tell(new Master.CalculatePi());

            Console.WriteLine("Calculating Pi -- please be patient...");

            Console.WriteLine("Press [enter] to continue");
            Console.ReadLine();

            system.Shutdown();
            system.AwaitTermination();
        }
    }
}
