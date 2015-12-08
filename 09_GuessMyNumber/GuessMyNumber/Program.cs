using System;
using Akka.Actor;
using GuessMyNumber.Actors;
using System.Threading;

namespace GuessMyNumber
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Number Guess Starting");

            var system = ActorSystem.Create("Numb3rs");
            var chooser = system.ActorOf(Props.Create<Chooser>(), "Chooser");
            var enquirer = system.ActorOf(Props.Create<Enquirer>(chooser), "Enquirer");

            Thread.Sleep(500);
            Console.WriteLine("Press [enter] to continue");
            Console.ReadLine();

            system.Shutdown();
            system.AwaitTermination();
        }
    }
}
