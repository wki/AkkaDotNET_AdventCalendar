using System;
// using System.Linq;
using Akka.Actor;
using MapReduce.Messages;

namespace MapReduce.Actors
{
    public class Aggregator : ReceiveActor
    {
        private ReduceResult reduceResult;

        public Aggregator()
        {
            reduceResult = new ReduceResult();

            Receive<ReduceResult>(r => Aggregate(r));
            Receive<GetResult>(_ => Sender.Tell(reduceResult));
        }

        private void Aggregate(ReduceResult result)
        {
            foreach (var language in result.Result.Keys)
            {
                if (reduceResult.Result.ContainsKey(language))
                {
                    reduceResult.Result[language] += result.Result[language];
                }
                else
                {
                    reduceResult.Result[language] = result.Result[language];
                }
            }

            Console.WriteLine("Aggregator: {0}", reduceResult);
        }
    }
}
