using System;
using System.Diagnostics;
using Akka.Actor;
using System.Threading;
using System.Collections.Generic;

namespace PiCalculator
{
    public class MainClass
    {
        // the amount of values to sum up to get Pi
        private const int Million = 1000 * 1000;
        private const int Series = 100 * Million;

        // the number of units to split the Series up
        private static readonly int[] NrUnitsToIterate = new [] { 1, 2, 3, 4, 8, 20, 50, 250 };

        // the number of worker actors to start for doing calculations
        private const int NrWorkersToStart = 10;

        // remember runtimes for iterations
        private class Statistics
        {
            public int NrUnits { get; set; }
            public long ElapsedMilliseconds { get; set; }

            public Statistics(int nrUnits, long elapsedMilliseconds)
            {
                NrUnits = nrUnits;
                ElapsedMilliseconds = elapsedMilliseconds;
            }
        }

        public static void Main(string[] args)
        {
            var actorSystem = ActorSystem.Create("Calculator");
            var statistics = new List<Statistics>();

            foreach (var nrUnits in NrUnitsToIterate)
            {
                var stopWatch = Stopwatch.StartNew();

                actorSystem.ActorOf(Props.Create<RoutingMaster>(NrWorkersToStart), "Master" + nrUnits)
                    .Ask<double>(new RoutingMaster.CalculatePi(nrUnits, Series))
                    .Wait();

                stopWatch.Stop();
                statistics.Add(new Statistics(nrUnits, stopWatch.ElapsedMilliseconds));

                Console.WriteLine("----------");
            }

            Thread.Sleep(TimeSpan.FromSeconds(1));

            actorSystem.Shutdown();
            actorSystem.AwaitTermination();

            Console.WriteLine();
            statistics.ForEach(s => 
                Console.WriteLine ("nrUnits: {0,3}, Elapsed: {1,5}ms", s.NrUnits, s.ElapsedMilliseconds)
            );

            Console.WriteLine("Press [enter] to continue");
            Console.ReadLine();
        }
    }
}
