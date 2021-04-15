using System;

namespace GameOfLife.Core
{
    public class Cell
    {
        public Cell(int row, int column, bool alive)
        {
            Row = row;
            Column = column;
            IsAlive = alive;
        }

        public bool IsAlive { get; set; }

        public int Row { get; private set; }

        public int Column { get; private set; }
    }
}
