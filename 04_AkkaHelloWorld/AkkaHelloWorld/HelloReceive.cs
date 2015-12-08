using System;
using Akka.Actor;

namespace AkkaHelloWorld
{
    public class HelloReceive : ReceiveActor
    {
        public HelloReceive()
        {
            Receive<string>(s => HandleStringMessage(s));
        }

        public void HandleStringMessage(string message)
        {
            Console.WriteLine("Received: '{0}'", message);
        }
    }
}
