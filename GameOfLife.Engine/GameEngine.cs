using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace GameOfLife.Engine
{
    public class GameEngine : IGameEngine
    {
        private  UniverseConfiguration _configuration;

        private int _width;

        private int _height;

        private Playground _playground;

        public GameEngine(int width, int height, UniverseConfiguration configuration)
        {
            _width = width;
            _height = height;
            _configuration = configuration;

            _playground = new Playground(_width, _height, _configuration);
        }

        public int CurrentGenerationNumber { get; private set; }

        public bool GameEnded { get; private set; }

        public Bitmap Playground => _playground.Body;

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

        public GameSave SaveGame()
        {
            return new GameSave()
            {
                GenerationNumber = CurrentGenerationNumber,
                GameEnded = GameEnded,
                Playground = _playground.Body,
                UniverseConfiguration = _playground.Configuration,
                DateTime = DateTime.Now
            };
        }

        public void LoadGame(GameSave save)
        {
            CurrentGenerationNumber = save.GenerationNumber;
            GameEnded = save.GameEnded;
            _playground.LoadGameFromBitmap(save.Playground);
            _playground.Configuration = save.UniverseConfiguration;
        }

        public void RandomizeGame()
        {
            var randomCells = new Cell[_width, _height];

            var random = new Random((int)DateTime.Now.Ticks);
            for (int x = 0; x < randomCells.GetLength(0); x++)
            {
                for (int y = 0; y < randomCells.GetLength(1); y++)
                {
                    randomCells[x, y] = new Cell(x, y, random.Next(2) == 1);
                }
            }

            _playground.LoadGameFromCells(randomCells);
        }

        public void ResetGame(int width, int height, UniverseConfiguration configuration)
        {
            _width = width;
            _height = height;
            _configuration = configuration;
            _playground = new Playground(width, height, configuration);
            GameEnded = false;
            CurrentGenerationNumber = 0;
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
