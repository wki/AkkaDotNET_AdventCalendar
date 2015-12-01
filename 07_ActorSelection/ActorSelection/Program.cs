using System;
using Akka.Actor;
using System.Threading;

namespace ActorSelection
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var system = ActorSystem.Create("Selection");

            var writer1 = system.ActorOf(Props.Create<Writer>(), "writer1");
            var writer2 = system.ActorOf(Props.Create<Writer>(), "writer2");
            var writer3 = system.ActorOf(Props.Create<Writer>(), "writer3");

            var xxx1 = system.ActorOf(Props.Create<Writer>(), "xxx1");
            var xxx2 = system.ActorOf(Props.Create<Writer>(), "xxx2");

            var printer = system.ActorOf(Props.Create<Writer>(), "printer");


            // use ActorRef
            writer2.Tell("hello");
            xxx1.Tell("Ola");

            // use ActorSelection
            system.ActorSelection("/user/writer*").Tell("hi");

            Thread.Sleep(500);
            Console.WriteLine("press [enter] to continue");
            Console.ReadLine();

            system.Shutdown();
            system.AwaitTermination();
        }
    }
}
