using System;
using Akka.Actor;
using XmasTree.Messages;
using System.Collections.Generic;

namespace XmasTree
{
    public class Controller : ReceiveActor
    {
        Random random;
        List<Switch> controlMessages;

        public Controller()
        {
            random = new Random();

            controlMessages = new List<Switch>();
            for (var c = (Color)0; c < Color.NrValues; c++)
            {
                controlMessages.Add(new Switch(c, false));
                controlMessages.Add(new Switch(c, true));
            }

            Receive<Tick>(t => HandleTick(t));
        }

        private void HandleTick(Tick _)
        {
            var index = random.Next(controlMessages.Count);

            Context.System.EventStream.Publish(controlMessages[index]);
        }
    }
}
