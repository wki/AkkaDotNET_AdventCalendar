using System;

namespace XmasTree.Messages
{
    public class HideLight
    {
        public int X { get; set; }
        public int Y { get; set; }

        public HideLight(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
