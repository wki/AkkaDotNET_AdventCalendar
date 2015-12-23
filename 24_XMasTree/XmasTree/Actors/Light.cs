using System;
using Akka.Actor;
using Akka.Event;
using XmasTree.Messages;

namespace XmasTree.Actors
{
    public class Light : ReceiveActor
    {
        private int x;
        private int y;
        private Color color;
        private IActorRef writer;

        public Light(int x, int y, Color color, IActorRef writer)
        {
            this.x = x;
            this.y = y;
            this.color = color;
            this.writer = writer;

            Receive<Switch>(SwitchLight, s => s.Color == color);
        }

        protected override void PreStart()
        {
            Context.System.EventStream.Subscribe<Switch>(Self);
        }

        private void SwitchLight(Switch s)
        {
            ConsoleColor consoleColor;
            Enum.TryParse<ConsoleColor>(color.ToString(), out consoleColor);

            if (s.SwitchOn)
                writer.Tell(new ShowLight(x, y, consoleColor));
            else
                writer.Tell(new HideLight(x, y));
        }
    }
}
