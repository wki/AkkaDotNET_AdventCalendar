using System;
using Akka.Actor;
using System.Linq;

namespace PiCalculator
{
    /// <summary>
    /// The master controls calculation for Pi
    /// </summary>
    public class Master : ReceiveActor
    {
        /// <summary>
        /// Command Message to initiate Calculation process
        /// </summary>
        public class CalculatePi
        {
            public int NrWorkers { get; private set; }
            public int Series { get; private set; }

            public CalculatePi(int nrWorkers, int series)
            {
                NrWorkers = nrWorkers;
                Series = series;
            }
        }

        private double sum;
        private int nrMissing;
        private IActorRef creator;

        /// <summary>
        /// Master Actor for calculating Pi. delegates to workers.
        /// </summary>
        public Master()
        {
            Console.WriteLine("Starting Master {0}", Self.Path.Name);

            Receive<CalculatePi>(c => CalculatePiHandler(c));
            Receive<double>(d => SumHandler(d));
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
            creator = Sender;

            sum = 0;
            nrMissing = calculatePi.NrWorkers;

            int workersLeft = calculatePi.NrWorkers;
            int series = calculatePi.Series;

            int minWorkUnit = 0;
            while (workersLeft > 0)
            {
                int maxWorkUnit = minWorkUnit + (series - minWorkUnit) / workersLeft;
                var worker = Context.ActorOf<Worker>();
                worker.Tell(new Worker.CalculateRange(minWorkUnit, maxWorkUnit));

                minWorkUnit = maxWorkUnit + 1;
                workersLeft--;
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
                Console.WriteLine("All Workers terminated. Pi = {0}", sum);

                creator.Tell(sum);
            }
        }
//
//        /// <summary>
//        /// Event handler fired when a child has terminated
//        /// </summary>
//        /// <param name="terminated">Terminated.</param>
//        private void TerminatedHandler(Terminated terminated)
//        {
//            var nrChildren = Context.GetChildren().Count();
//            Console.WriteLine("Child terminated, remaining: {0}", nrChildren);
//
//            // terminate if no more children are active
//            if (nrChildren == 0)
//                Context.Stop(Self);
//        }
    }
}
