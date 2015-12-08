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
        private int mySecretNumber;

        public Chooser()
        {
            var generator = new Random();
            mySecretNumber = generator.Next(1, 101);

            Console.WriteLine("Pssst: I have something to guess: {0}", mySecretNumber);

            Receive<TestTry>(t => HandleTestTry(t));
        }

        private void HandleTestTry(TestTry testTry)
        {
            var triedNumber = testTry.Number;

            Console.WriteLine("Received Guess: {0}", triedNumber);

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
    }
}
