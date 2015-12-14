using System;
using System.Diagnostics;
using Akka.Actor;
using Akka.Routing;
using System.Collections.Generic;

namespace PiSequential
{
    public class Master : ReceiveActor
    {
        // modify these constants to change behavior
        const int NrParts = 8;
        const int NrWorkers = 4;
        const int RangeTo = 200 * 1000 * 1000;

        public class CalculatePi {}

        IActorRef worker;
        double sum;
        int nrAnswers;
        Stopwatch stopwatch;

        public Master()
        {
            // single router
            // worker = Context.ActorOf(Props.Create<Worker>());

            // home brewn router
            // worker = Context.ActorOf(Props.Create<RoutingWorker>());

            // akka.net built-in router
            worker = Context.ActorOf(
                Props.Create<Worker>()
                     .WithRouter(new RoundRobinPool(NrWorkers))
            );

            Receive<CalculatePi>(c => DoCalculation(c));
            Receive<double>(d => AddToSum(d));
        }

        private void DoCalculation(CalculatePi _)
        {
            sum = 0;
            nrAnswers = 0;
            stopwatch = Stopwatch.StartNew();

            foreach (var range in Distribute(RangeTo, NrParts))
                worker.Tell(new Worker.CalculateRange(range));
        }

        private void AddToSum(double d)
        {
            sum += d;
            if (++nrAnswers == NrParts)
            {
                stopwatch.Stop();
                Console.WriteLine("Calculation done. Pi = {0}, Elapsed: {1:F2}", sum, stopwatch.ElapsedMilliseconds / 1000.0);
            }
        }

        public static IEnumerable<Range> Distribute(int rangeMax, int nrParts)
        {
            int rangeStartAt = 0;
            int nrPartsRemaining = nrParts;

            while (nrPartsRemaining > 0)
            {
                int rangeEndAt = rangeStartAt + (rangeMax - rangeStartAt) / nrPartsRemaining;

                yield return new Range(rangeStartAt, rangeEndAt);

                rangeStartAt = rangeEndAt + 1;
                nrPartsRemaining--;
            }
        }

    }
}

