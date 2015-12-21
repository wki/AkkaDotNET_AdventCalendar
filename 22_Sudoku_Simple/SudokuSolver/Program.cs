using System;
using Akka.Actor;
using SudokuSolver.Actors;
using System.Threading;
using SudokuSolver.Messages;

namespace SudokuSolver
{
    // TODO:
    //  - create SodukuSolver actor (grid array is initial arg)
    //     - create all actor* actors
    //     - wait until all are ready
    //     - start solving
    //     - also register for SetDigit Messages
    //     - timeout: cannot solve
    //     - count SetDigit Messages (81 = ready)
    //  - discover solved sudoku
    //  - discover unsolvable sudoku
    //  - automatically print statistics at end
    class MainClass
    {
        private static IActorRef printer;

        public static void Main(string[] args)
        {
            var system = ActorSystem.Create("Sudoku");
            printer = system.ActorOf<Printer>();
            var statistics = system.ActorOf(Props.Create<StatisticsCollector>(printer));

            EnsureScreenIsBigEnough();

            ClearScreen();
            CreateSudokuActors(system);
            PopulateSudoku(system);

            PrintLine(44,0, "Press [enter] to Quit");
            Console.ReadLine();

            system.Shutdown();
            system.AwaitTermination();
        }

        static void EnsureScreenIsBigEnough()
        {
            const int MinWidth = 80;
            const int MinHeight = 48;

            while (Console.BufferWidth < MinWidth || Console.BufferHeight < MinHeight)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("*** Your Terminal is too small. Should be {0}x{1}, is {2}x{3}", 
                    MinWidth, MinHeight,
                    Console.BufferWidth, Console.BufferHeight);
                Console.WriteLine("\nPlease increase your screen's size or press Ctrl+C to abort");

                Thread.Sleep(TimeSpan.FromSeconds(0.5));
            }

            Console.ResetColor();
        }

        private static void PrintLine(int row, int col, string line)
        {
            printer.Tell(new PrintLine(row, col, line));
        }

        private static void CreateSudokuActors(ActorSystem system)
        {

            for (int row = 0; row < 9; row++)
                for (int col = 0; col < 9; col++)
                    system.ActorOf(Props.Create<SudokuCell>(printer, row, col), String.Format("{0}-{1}", row, col));
        }

        private static void ClearScreen()
        {
            Console.Clear();

            for (var row = 0; row < 44; row++)
            {
                Console.SetCursorPosition(0, row);
                if (row == 11 || row == 24)
                    Console.WriteLine("_________________________________________");
                else if (row < 37)
                    Console.WriteLine("             |              |");
            }
        }

        private static void PopulateSudoku(ActorSystem system)
        {
            // TODO: find better was. possibly wait untl all report "ready"
            Thread.Sleep(TimeSpan.FromSeconds(1));

            var sudoku = EasySudoku(); // HardSudoku();
            for (var i = 0; i < sudoku.Length; i++)
            {
                int digit = sudoku[i];

                int row = i / 9;
                int col = i % 9;

                if (digit != 0)
                {
                    system.EventStream.Publish(new SetDigit(row, col, digit));
                    Thread.Sleep(TimeSpan.FromMilliseconds(200));
                }
            }
        }

        private static int[] HardSudoku()
        {
            return new []
                {
                    0,0,4,   0,8,0,   0,1,0,
                    0,0,0,   9,0,2,   0,0,0,
                    2,6,0,   0,0,7,   5,0,0,

                    0,7,3,   0,0,0,   0,0,0,
                    6,0,0,   0,0,0,   4,9,3,
                    0,5,0,   0,1,0,   0,0,7,

                    0,0,1,   7,3,0,   0,2,0,
                    0,0,0,   0,0,0,   0,0,0,
                    0,0,0,   8,0,0,   6,0,0
                };
        }

        private static int[] EasySudoku()
        {
            return new []
                {
                    7,0,1,   9,6,8,   5,3,0,
                    0,9,0,   0,0,1,   8,7,6,
                    2,8,6,   7,0,0,   4,1,9,

                    0,0,0,   2,5,0,   7,8,0,
                    6,7,5,   0,8,0,   2,9,1,
                    0,0,4,   0,9,7,   3,0,0,

                    4,0,2,   0,7,9,   6,0,3,
                    0,6,8,   0,0,3,   0,2,7,
                    0,0,7,   0,1,2,   9,0,8
                };
        }
    }
}
