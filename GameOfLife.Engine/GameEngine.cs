using System.Collections.Generic;
using System.Linq;

namespace GameOfLife.Engine
{
    public class GameEngine : IGameEngine
    {
        private readonly Playground _playground;

        public GameEngine(Playground playground)
        {
            _playground = playground;
        }

        public int CurrentGenerationNumber { get; private set; }

        public bool GameEnded { get; private set; }

        public void MoveToNextGeneration()
        {
            const int underPopulationThreshold = 2;
            const int overPopulationThreshold = 3;
            const int reproductionThreshold = 3;

            var changedCellsList = new List<Cell>();

            for (var x = 0; x < _playground.Width; x++)
            {
                for (var y = 0; y < _playground.Height; y++)
                {
                    var cell = _playground.GetCell(x, y);

                    var numberOfAliveNeighbors = GetNumberOfAliveNeighbors(cell);

                    if (cell.IsAlive && (numberOfAliveNeighbors < underPopulationThreshold ||
                                         numberOfAliveNeighbors > overPopulationThreshold))
                    {
                        changedCellsList.Add(cell);
                    }
                    else if (!cell.IsAlive && numberOfAliveNeighbors == reproductionThreshold)
                    {
                        changedCellsList.Add(cell);
                    }
                }
            }

            GameEnded = !changedCellsList.Any();

            if (!GameEnded)
            {
                CurrentGenerationNumber++;
                changedCellsList.ForEach(ChangeUniverseState);
            }
        }

        public void ChangeUniverseState(Cell cell)
        {
            _playground.Update(cell);
        }

        private int GetNumberOfAliveNeighbors(Cell cell)
        {
            var numberOfAliveNeighbours = 0;

            var neighboringCells = new List<Cell>
            {
                _playground.GetCell(cell.X - 1, cell.Y - 1),
                _playground.GetCell(cell.X - 1, cell.Y + 1),
                _playground.GetCell(cell.X, cell.Y + 1),
                _playground.GetCell(cell.X + 1, cell.Y + 1),
                _playground.GetCell(cell.X + 1, cell.Y),
                _playground.GetCell(cell.X + 1, cell.Y - 1),
                _playground.GetCell(cell.X, cell.Y - 1),
                _playground.GetCell(cell.X - 1, cell.Y)
            };

            neighboringCells.ForEach(
                neighboringCell => numberOfAliveNeighbours +=
                    (neighboringCell != null && neighboringCell.IsAlive) ? 1 : 0
            );

            return numberOfAliveNeighbours;
        }
    }
}
