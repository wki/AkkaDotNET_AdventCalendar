using System;
using Akka.Actor;
using XmasTree.Messages;

namespace XmasTree
{
    public class Writer : ReceiveActor
    {
        const int NrRows = 7;
        const int RootRows = 2;
        const int MinX = 10;
        const int MinY = 2;

        public Writer()
        {
            Console.SetCursorPosition(0, 0);
            Console.Clear();

            Receive<HideLight>(h => HideLight(h));
            Receive<ShowLight>(s => ShowLight(s));
            Receive<ShowLeaf>(s => ShowLeaf(s));
            Receive<ShowRoot>(s => ShowRoot(s));
        }

        private void HideLight(HideLight hideLight)
        {
            Console.SetCursorPosition(hideLight.X + MinX, hideLight.Y + MinY);
            Console.Write(' ');
            Console.SetCursorPosition(0, 0);
        }

        private void ShowLight(ShowLight showLight)
        {
            Console.ForegroundColor = showLight.Color;
            Console.SetCursorPosition(showLight.X + MinX, showLight.Y + MinY);
            Console.Write('*');
            Console.SetCursorPosition(0, 0);
        }

        private void ShowLeaf(ShowLeaf showLeaf)
        {
            Console.ResetColor();
            Console.SetCursorPosition(showLeaf.X + MinX, showLeaf.Y + MinY);
            Console.Write('x');
        }

        private void ShowRoot(ShowRoot _)
        {
            Console.ResetColor();

            for (var row = 0; row < RootRows; row++)
            {
                Console.SetCursorPosition(NrRows + MinX, row + NrRows * 2 + MinY - 1);
                Console.Write('X');
            }
        }
    }
}
