using System;
using Akka.Actor;

namespace ActorSelection
{
    public class Writer : ReceiveActor
    {
        public Writer()
        {
            Receive<string>(s => 
                Console.WriteLine("{0} received {1}", Self.Path.Name, s));
        }
    }
}
