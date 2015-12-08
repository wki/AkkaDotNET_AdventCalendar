using System;
using Akka.Actor;
using GuessMyNumber.Messages;

namespace GuessMyNumber.Actors
{
    /// <summary>
    /// The enquirer tries to guess a number unsing binary search
    /// </summary>
    public class Enquirer : ReceiveActor
    {
        private readonly IActorRef selector;

        // possible range of numbers including boundaries
        private int rangeFrom;
        private int rangeTo;

        public Enquirer(IActorRef selector)
        {
            this.selector = selector;

            rangeFrom = 1;
            rangeTo = 100;

            Receive<TryTooSmall>(s => HandleTooSmallTry(s));
            Receive<TryTooBig>(b => HandleTooBigTry(b));
            Receive<Guessed>(g => HandleGuessed(g));

            MakeATry();
        }

        private void MakeATry()
        {
            var triedNumber = rangeFrom + (rangeTo - rangeFrom) / 2;

            Console.WriteLine("Range: {0} - {1}, trying: {2}", rangeFrom, rangeTo, triedNumber);

            selector.Tell(new TestTry(triedNumber));
        }

        private void HandleTooSmallTry(TryTooSmall guessTooSmall)
        {
            rangeFrom = guessTooSmall.Number + 1;
            MakeATry();
        }

        private void HandleTooBigTry(TryTooBig guessTooBig)
        {
            rangeTo = guessTooBig.Number - 1;
            MakeATry();
        }

        private void HandleGuessed(Guessed guessed)
        {
            Console.WriteLine("Guessed: {0}", guessed.Number);
        }
    }
}
