using System;
using Akka.Actor;
using GuessMyNumber.Messages;

namespace GuessMyNumber.Actors
{
    /// <summary>
    /// The selector picks a random number and waits for guesses
    /// </summary>
    public class Chooser : ReceiveActor
    {
        private Random generator;
        private int mySecretNumber;

        // will die only the first time.
        private static bool shouldCrash = true;

        public Chooser()
        {
            generator = new Random();

            Receive<Start>(s => ChooseNumber(s));
            Receive<TestTry>(t => HandleTestTry(t));
        }

        protected override void PostRestart(Exception reason)
        {
            Console.WriteLine("Chooser PreRestart");

            Self.Tell(new Start());

            shouldCrash = false;
        }

        private void ChooseNumber(Start _)
        {
            mySecretNumber = generator.Next(1, 101);

            Console.WriteLine("Chooser: Pssst -- I have something to guess: {0}", mySecretNumber);

            // indicate we are ready to wait for tries
            Context.Parent.Tell(new Started());
        }

        private void HandleTestTry(TestTry testTry)
        {
            if (shouldCrash)
                throw new InvalidOperationException("chooser died");

            var triedNumber = testTry.Number;

            Console.WriteLine("Chooser: Received Guess {0}", triedNumber);

            if (triedNumber < mySecretNumber)
            {
                Sender.Tell(new TryTooSmall(triedNumber));
            }
            else if (triedNumber > mySecretNumber)
            {
                Sender.Tell(new TryTooBig(triedNumber));
            }
            else
            {
                Sender.Tell(new Guessed(triedNumber));
            }
        }

//        protected override SupervisorStrategy SupervisorStrategy()
//        {
//            return new OneForOneStrategy(
//                _ => Directive.Stop
//            );
//        }
    }
}
