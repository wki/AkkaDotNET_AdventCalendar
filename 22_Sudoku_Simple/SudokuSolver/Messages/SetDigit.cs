using System;

namespace SudokuSolver
{
    /// <summary>
    /// Command message to set a digit at a row + column
    /// </summary>
    public class SetDigit
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public int Block { get { return Row / 3 * 3 + Col / 3; } }

        public int Digit { get; set; }

        public SetDigit(int row, int col, int digit)
        {
            Row = row;
            Col = col;
            Digit = digit;
        }
    }
}
