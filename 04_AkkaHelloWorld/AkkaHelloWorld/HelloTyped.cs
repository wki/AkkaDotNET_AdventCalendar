using System;
using Akka.Actor;

namespace AkkaHelloWorld
{
    public class HelloTyped : TypedActor, IHandle<string>
    {
        public void Handle(string message)
        {
            Console.WriteLine("Received: '{0}'", message);
        }
    }
}
