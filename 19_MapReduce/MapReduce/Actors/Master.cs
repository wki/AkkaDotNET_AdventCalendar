using System;
using Akka.Actor;
using Akka.Routing;
using MapReduce.Actors;
using MapReduce.Messages;

namespace MapReduce.Actors
{
    public class Master : ReceiveActor
    {
        const int NrMappers = 4;
        const int NrReducers = 4;

        IActorRef mapper;
        IActorRef reducer;
        IActorRef aggregator;

        public Master()
        {
            mapper = Context.ActorOf(
                Props.Create<Mapper>()
                     
                .WithRouter(new RoundRobinPool(NrMappers))
            );
            reducer = Context.ActorOf(
                Props.Create<Reducer>()
                     .WithRouter(new RoundRobinPool(NrReducers))
            );
            aggregator = Context.ActorOf(Props.Create<Aggregator>());

            // 1. forward a string to mapper
            Receive<string>(s => mapper.Tell(s));

            // 2. forward map result to reducer
            Receive<MapResult>(m => reducer.Tell(m));

            // 3. forward reduce result to aggregator
            Receive<ReduceResult>(r => aggregator.Tell(r));

            // allow asking for aggregated result at any time
            Receive<GetResult>(g => aggregator.Forward(g));
        }
    }
}
