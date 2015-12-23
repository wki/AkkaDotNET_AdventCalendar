namespace XmasTree.Messages
{
    public class ShowLeaf
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public ShowLeaf(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
