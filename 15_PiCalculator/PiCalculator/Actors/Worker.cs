using System;
using System.Linq;
using Akka.Actor;
using System.Threading;

namespace PiCalculator
{
    /// <summary>
    /// The worker does the actual calculation for a given range
    /// </summary>
    public class Worker : ReceiveActor
    {
        /// <summary>
        /// Command Message to initiate Calculation for a given range
        /// </summary>
        public class CalculateRange
        {
            public int From { get; private set; }
            public int To { get; private set; }

            public CalculateRange(int from, int to)
            {
                From = from;
                To = to;
            }
        }

        public Worker()
        {
            Console.WriteLine("  Starting Worker {0}", Self.Path.Name);
            Receive<CalculateRange>(r => CalculateRangeHandler(r));
        }

        /// <summary>
        /// Command handler for CalculateRange
        /// </summary>
        /// <param name="range">Range.</param>
        private void CalculateRangeHandler(CalculateRange range)
        {
            Console.WriteLine("  {0}: Calculating Range from {1} to {2} Thread {3}", 
                Self.Path.Name, range.From, range.To, Thread.CurrentThread.ManagedThreadId
            );

            // calculate
            double sum = 0;
            for (int k = range.From; k <= range.To; k++)
                sum += Math.Pow(-1, k) / (2 * k + 1);

            // send back result
            Sender.Tell(sum * 4);
        }
    }
}
