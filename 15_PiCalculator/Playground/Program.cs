using System;
using System.Collections.Generic;

namespace Playground
{
    class MainClass
    {
        public class Range
        {
            public int StartAt { get; set; }
            public int EndAt { get; set; }

            public Range (int startAt, int endAt)
            {
                StartAt = startAt;
                EndAt = endAt;
            }

            public override string ToString()
            {
                return string.Format("[Range: {0}...{1}]", StartAt, EndAt);
            }
        }

        public static void Main(string[] args)
        {
            Console.WriteLine("20 - 3 parts: " + String.Join(", ", Distribute(20, 3)));
            Console.WriteLine("1000 - 6 parts: " + String.Join(", ", Distribute(1000, 6)));
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
