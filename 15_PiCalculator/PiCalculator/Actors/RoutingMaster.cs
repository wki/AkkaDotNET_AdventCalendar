using System;
using Akka.Actor;
using System.Linq;
using Akka.Routing;
using System.Collections.Generic;

namespace PiCalculator
{
    /// <summary>
    /// The master controls calculation for Pi using a router
    /// </summary>
    public class RoutingMaster : ReceiveActor
    {
        /// <summary>
        /// Command Message to initiate Calculation process
        /// </summary>
        public class CalculatePi
        {
            public int NrUnits { get; private set; }
            public int Series { get; private set; }

            public CalculatePi(int nrUnits, int series)
            {
                NrUnits = nrUnits;
                Series = series;
            }
        }

        private int nrWorkers;
        private double sum;
        private int nrMissing;
        private IActorRef worker;
        private IActorRef creator;

        /// <summary>
        /// Master Actor for calculating Pi. delegates to workers.
        /// </summary>
        public RoutingMaster(int nrWorkers)
        {
            Console.WriteLine("Starting Master {0}", Self.Path.Name);

            this.nrWorkers = nrWorkers;

            Receive<CalculatePi>(c => CalculatePiHandler(c));
            Receive<double>(d => SumHandler(d));
        }

        // will automatically start before going to regular opertion
        protected override void PreStart()
        {
            base.PreStart();

            worker = Context.ActorOf(
                Props.Create<Worker>()
                     .WithRouter(new RoundRobinPool(nrWorkers))
            );
        }

        /// <summary>
        /// Command Handler for CalculatePi.
        /// </summary>
        /// <param name="calculatePi">Calculate Pi command</param>
        /// <description>
        /// The calculation consists of these steps:
        ///  * initialize a sum with 0
        ///  * create the requested amount of workers
        ///  * spread the range from 0 to series among the workers
        ///  * sum up the workers' sum values
        /// </description>
        private void CalculatePiHandler(CalculatePi calculatePi)
        {
            // remember who created this message to send back answer
            creator = Sender;

            // initial value for sum to be calculated
            sum = 0;

            // nr of missing result values for completed sum
            nrMissing = calculatePi.NrUnits;

            // split up to approximately same-sized units
            int unitsLeft = calculatePi.NrUnits;
            int minWorkUnit = 0;
            while (unitsLeft > 0)
            {
                int maxWorkUnit = minWorkUnit + (calculatePi.Series - minWorkUnit) / unitsLeft;
                worker.Tell(new Worker.CalculateRange(minWorkUnit, maxWorkUnit));

                minWorkUnit = maxWorkUnit + 1;
                unitsLeft--;
            }
        }

        /// <summary>
        /// Document handler for receiving a worker's sum
        /// </summary>
        /// <param name="s">Sum value</param>
        private void SumHandler(double s)
        {
            Console.WriteLine("Received sum from worker {0}", Sender.Path.Name);
            sum += s;

            if (--nrMissing == 0)
            {
                Console.WriteLine("All Workers finished. Pi = {0}", sum);

                creator.Tell(sum);
            }
        }
    }
}
