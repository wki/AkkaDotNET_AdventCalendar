using Akka.Actor;
using Akka.Event;
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
            Context.System.EventStream.Subscribe<SetDigit>(Self);
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
