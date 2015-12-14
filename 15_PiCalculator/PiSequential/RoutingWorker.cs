using System;
using Akka.Actor;

namespace PiSequential
{
    public class RoutingWorker : ReceiveActor
    {
        const int NrWorkers = 4;

        private IActorRef[] workers;
        private int nextWorker;

        public RoutingWorker()
        {
            workers = new IActorRef[NrWorkers];

            for (var i = 0; i < NrWorkers; i++)
                workers[i] = Context.ActorOf(Props.Create<Worker>());

            nextWorker = 0;

            Receive<Worker.CalculateRange>(r => ForwardToWorker(r));
        }

        private void ForwardToWorker(Worker.CalculateRange message)
        {
            workers[nextWorker].Forward(message);

            nextWorker = (nextWorker + 1) % NrWorkers;
        }
    }
}

