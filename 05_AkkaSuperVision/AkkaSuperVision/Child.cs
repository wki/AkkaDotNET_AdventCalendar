using System;
using Akka.Actor;

namespace AkkaSuperVision
{
    public class Child : ReceiveActor
	{
        private int nrMessagesHandled;

        public Child ()
        {
            nrMessagesHandled = 0;
            Receive<string>(HandleStringMessage);
        }

        protected override void PreStart()
        {
            Console.WriteLine("PreStart Actor '{0}'", Self.Path.Name);
        }

        protected override void PreRestart(Exception reason, object message)
        {
            Console.WriteLine("PreRestart Actor '{0}', reason: {1}, message: {2}",
                Self.Path.Name, reason.Message, message);
        }

        protected override void PostRestart(Exception reason)
        {
            Console.WriteLine("PostRestart Actor '{0}', reason: {1}", 
                Self.Path.Name, reason.Message);
        }

        protected override void PostStop()
        {
            Console.WriteLine("PostStop Actor '{0}'", Self.Path.Name);
        }

        private void HandleStringMessage(string message)
        {
            Console.WriteLine("Received: '{0}'", message);

            if (++nrMessagesHandled >= 3)
                throw new InvalidMessageException("haha");
        }
	}
}
