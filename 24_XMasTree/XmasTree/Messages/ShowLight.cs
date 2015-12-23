using System;

namespace XmasTree.Messages
{
    public class ShowLight
    {
        public int X { get; set; }
        public int Y { get; set; }
        public ConsoleColor Color { get; set; }

        public ShowLight(int x, int y, ConsoleColor color)
        {
            X = x;
            Y = y;
            Color = color;
        }
    }
}
