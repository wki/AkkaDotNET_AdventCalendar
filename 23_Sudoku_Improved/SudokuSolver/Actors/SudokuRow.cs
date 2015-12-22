using System;
using Akka.Actor;
using SudokuSolver.Messages;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver.Actors
{
    /// <summary>
    /// Keep digit counts for a row
    /// </summary>
    public class SudokuRow : SudokuActor
    {
        private readonly int row;

        private List<int> statistics;

        public SudokuRow(IActorRef printer, int row)
            : base(printer)
        {
            this.row = row;

            statistics = Enumerable.Range(1, 9).Select(_ => 9).ToList();

            Receive<StrikeDigit>(UpdateStatistics, s => s.Row == row);
        }

        private void UpdateStatistics(StrikeDigit strikeDigit)
        {
            var digit = strikeDigit.Digit;

            if (--statistics[digit - 1] == 1)
                Publish(new FindRowDigit(row, digit));

            Draw();
        }

        private void Draw()
        {
            PrintLine(20 + row, 44, String.Format("{0}: {1}",
                Self.Path.Name,
                String.Join(", ", statistics))
            );
        }
    }
}
