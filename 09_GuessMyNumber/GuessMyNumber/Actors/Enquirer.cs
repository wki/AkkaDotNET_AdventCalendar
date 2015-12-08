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
            Console.WriteLine("Enquirer: Constructor");

            this.selector = selector;

            Receive<Start>(s => StartGuessing(s));
            Receive<TryTooSmall>(t => HandleTooSmallTry(t));
            Receive<TryTooBig>(t => HandleTooBigTry(t));
            Receive<Guessed>(g => HandleGuessed(g));
        }

        protected override void PreStart()
        {
            Console.WriteLine("Enquirer: PreStart");
        }

        protected override void PreRestart(Exception reason, object message)
        {
            Console.WriteLine("Enquirer: PreRestart, message: {0}", message);
        }

        protected override void PostRestart(Exception reason)
        {
            Console.WriteLine("Enquirer: PostRestart");

            // we crashed during processing. So it is wise to Start again
            Self.Tell(new Start());
        }

        private void MakeATry()
        {
            var triedNumber = rangeFrom + (rangeTo - rangeFrom) / 2;

            Console.WriteLine("Enquirer: Range {0} - {1}, trying: {2}", rangeFrom, rangeTo, triedNumber);

            selector.Tell(new TestTry(triedNumber));
        }

        private void StartGuessing(Start _)
        {
            Console.WriteLine("Enquirer: Starting to guess");
            rangeFrom = 1;
            rangeTo = 100;

            MakeATry();
        }

        private void HandleTooSmallTry(TryTooSmall guessTooSmall)
        {
            Console.WriteLine("Enquirer: {0} is too small", guessTooSmall.Number);
            rangeFrom = guessTooSmall.Number + 1;
            MakeATry();
        }

        private void HandleTooBigTry(TryTooBig guessTooBig)
        {
            Console.WriteLine("Enquirer: {0} is too big", guessTooBig.Number);
            rangeTo = guessTooBig.Number - 1;
            MakeATry();
        }

        private void HandleGuessed(Guessed guessed)
        {
            Console.WriteLine("Enquirer: Guessed {0}", guessed.Number);
        }

        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(ex => Directive.Restart);
        }
    }
}
