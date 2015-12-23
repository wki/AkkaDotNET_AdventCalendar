using System;

namespace XmasTree.Messages
{
    public class Switch
    {
        public Color Color { get; set; }
        public bool SwitchOn { get; set; }

        public Switch(Color color, bool switchOn)
        {
            Color = color;
            SwitchOn = switchOn;
        }
    }
}
