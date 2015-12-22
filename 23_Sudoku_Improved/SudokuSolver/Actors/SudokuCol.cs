﻿using System;
using Akka.Actor;
using SudokuSolver.Messages;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver.Actors
{
    /// <summary>
    /// Keep digit counts for a column
    /// </summary>
    public class SudokuCol : SudokuActor
    {
        private readonly int col;

        private List<int> statistics;

        public SudokuCol(IActorRef printer, int col)
            : base(printer)
        {
            this.col = col;

            statistics = Enumerable.Range(1, 9).Select(_ => 9).ToList();

            Receive<StrikeDigit>(UpdateStatistics, s => s.Col == col);
        }

        private void UpdateStatistics(StrikeDigit strikeDigit)
        {
            var digit = strikeDigit.Digit;

            if (--statistics[digit - 1] == 1)
                Publish(new FindColDigit(col, digit));

            Draw();
        }

        private void Draw()
        {
            PrintLine(10 + col, 44, String.Format("{0}: {1}",
                Self.Path.Name,
                String.Join(", ", statistics))
            );
        }
    }
}
