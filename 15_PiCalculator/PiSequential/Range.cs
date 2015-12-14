using System;

namespace PiSequential
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

        public Range(Range other)
        {
            StartAt = other.StartAt;
            EndAt = other.EndAt;
        }

        public override string ToString()
        {
            return string.Format("[Range: {0}...{1}]", StartAt, EndAt);
        }
    }
}
