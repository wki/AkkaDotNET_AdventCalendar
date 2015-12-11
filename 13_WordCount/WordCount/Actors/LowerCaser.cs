using System;
using Akka.Actor;
using WordCount.Messages;

namespace WordCount
{
    /// <summary>
    /// Convert a string stream into lower case
    /// </summary>
    public class LowerCaser : ReceiveActor
    {
        public LowerCaser(IActorRef next)
        {
            Receive<string>(s => next.Tell(s.ToLower()));
            Receive<End>(next.Tell);
        }
    }
}
