using System;
using System.Collections.Generic;
using System.Text;

namespace GameOfLife.UI.Infrastructure
{
    internal class CellCoordinate
    {
        public CellCoordinate(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public int Row { get; private set; }

        public int Column { get; private set; }
    }
}
