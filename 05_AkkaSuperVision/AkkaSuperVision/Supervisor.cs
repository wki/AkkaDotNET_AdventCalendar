using System;
using Akka.Actor;

namespace AkkaSuperVision
{
	public class Supervisor : ReceiveActor
	{
        public IActorRef child;

        public Supervisor()
        {
            child = Context.ActorOf(Props.Create<Child>(),"Child");

            Context.Watch(child);

            Receive<string>(HandleStringMessage);
        }

        private void HandleStringMessage(string message)
        {
            child.Tell(message);
        }

        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(
                10,
                TimeSpan.FromSeconds(10),
                exception =>
                {
                    return Directive.Restart;
                }
            );
        }
	}
}
