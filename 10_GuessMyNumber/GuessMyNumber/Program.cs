using System;
using Akka.Actor;
using GuessMyNumber.Actors;
using System.Threading;
using GuessMyNumber.Messages;

namespace GuessMyNumber
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Number Guess Starting");

            var system = ActorSystem.Create("Numb3rs");
            var game = system.ActorOf(Props.Create<GuessGame>(), "Game");
            game.Tell(new Start());

            Thread.Sleep(500);
            Console.WriteLine("Press [enter] to continue");
            Console.ReadLine();

            system.Shutdown();
            system.AwaitTermination();
        }
    }
}
