using System;
using Akka.Actor;
using System.Threading;

namespace AkkaHelloWorld
{
    public class MainClass
    {
        public static void Main(string[] args)
        {
            var system = ActorSystem.Create("World");

            // TODO: do something with actor system
            var hello = system.ActorOf<Hello>();
//            var hello = system.ActorOf(Props.Create<Hello>());
//            var hello = system.ActorOf(Props.Create(typeof(Hello));
//            var hello = system.ActorOf(Props.Create(() => new Hello()));
            hello.Tell("Hello World");


            var time = system.ActorOf(Props.Create<TimeReader>());
            time.Tell("");
            time.Tell("");
            time.Tell("");
            time.Tell("");
            time.Tell("");
            time.Tell("");

            Thread.Sleep(TimeSpan.FromSeconds(0.5));

            Console.WriteLine("\n\nPress [enter] to continue");
            Console.ReadLine();

            system.Shutdown();
            system.AwaitTermination();
        }
    }
}
