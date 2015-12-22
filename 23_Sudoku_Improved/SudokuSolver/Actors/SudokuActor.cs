using System;
using Akka.Actor;
using SudokuSolver.Messages;

namespace SudokuSolver.Actors
{
    /// <summary>
    /// Base class for all Sudoko actors
    /// </summary>
    public class SudokuActor : ReceiveActor
    {
        private readonly IActorRef printActor;

        public SudokuActor(IActorRef printActor)
        {
            this.printActor = printActor;
        }

        protected override void PreStart()
        {
            base.PreStart();

            Context.System.EventStream.Subscribe(Self, typeof(SetDigit));
            Context.System.EventStream.Subscribe(Self, typeof(StrikeDigit));

            Context.System.EventStream.Subscribe(Self, typeof(FindRowDigit));
            Context.System.EventStream.Subscribe(Self, typeof(FindColDigit));
            Context.System.EventStream.Subscribe(Self, typeof(FindBlockDigit));
        }

        protected void Publish(object message)
        {
            Context.System.EventStream.Publish(message);
        }

        protected override void Unhandled(object message)
        {
            // we do nothing, just ignore unhandled messages
        }

        protected void PrintLine(int row, int col, string line)
        {
            printActor.Tell(new PrintLine(row, col, line));
        }
    }
}
