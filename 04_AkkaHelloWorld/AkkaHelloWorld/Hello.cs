using System;
using Akka.Actor;

namespace AkkaHelloWorld
{
    public class Hello : UntypedActor
    {
        protected override void OnReceive(object message)
        {
            Console.WriteLine("Received: '{0}'", message);
        }
    }
}
