﻿using System;
using Akka.Actor;
using GuessMyNumber.Messages;

namespace GuessMyNumber.Actors
{
    public class GuessGame : ReceiveActor
    {
        private IActorRef chooser;
        private IActorRef enquirer;

        public GuessGame()
        {
            chooser = Context.ActorOf(
                Props.Create<Chooser>(),
                "Chooser");
            
            enquirer = Context.ActorOf(
                Props.Create<Enquirer>(chooser), 
                "Enquirer");

            Receive<Start>(s => StartGame(s));
            Receive<Started>(s => StartGuessing(s));
            Receive<Guessed>(g => EndGame(g));
        }

        protected override void PreRestart(Exception reason, object message)
        {
            Console.WriteLine("GAME PreRestert e:{0}", reason.Message);
        }

        private void StartGame(Start start)
        {
            Console.WriteLine("Game: We are told to start the game");
            chooser.Tell(new Start());
        }

        private void StartGuessing(Started _)
        {
            Console.WriteLine("Game: Now we can start guessing");
            enquirer.Tell(new Start());
        }

        private void EndGame(Guessed _)
        {
            Console.WriteLine("Game: Number is guessed, game is over");
        }
    }
}
