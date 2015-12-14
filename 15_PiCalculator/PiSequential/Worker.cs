using System;
using Akka.Actor;
using System.Threading;

namespace PiSequential
{
    /// <summary>
    /// The worker does the actual calculation for a given range
    /// </summary>
    public class Worker : ReceiveActor
    {
        /// <summary>
        /// Command Message to initiate Calculation for a given range
        /// </summary>
        public class CalculateRange : Range
        {
            public CalculateRange(Range range) : base(range) {}
            
            public CalculateRange(int startAt, int endAt) : base(startAt, endAt) {}
        }

        public Worker()
        {
            Console.WriteLine("  Starting Worker {0}", Self.Path.Name);
            Receive<CalculateRange>(c => CalculateRangeHandler(c));
        }

        /// <summary>
        /// Command handler for CalculateRange
        /// </summary>
        /// <param name="range">Range.</param>
        private void CalculateRangeHandler(CalculateRange range)
        {
            Console.WriteLine("  {0}: Calculating {1} Thread {2}", 
                Self.Path.Name, range, Thread.CurrentThread.ManagedThreadId
            );

            // calculate
            double sum = 0;
            for (int n = range.StartAt; n <= range.EndAt; n++)
                sum += Math.Pow(-1, n) / (2 * n + 1);

            // send back result. If we multiply by 4 here, the sender will
            // automatically contain pi.
            Sender.Tell(sum * 4);
        }
    }
}
