using System;
using System.Drawing;

namespace GameOfLife.Engine
{
    public class Playground
    {
        private readonly int _pixelsPerCell;

        private readonly int _borderWidth;

        private Cell[,] _cells;

        public Playground(int width, int height)
        : this (width, height, 1, 0, UniverseConfiguration.Limited)
        {
        }

        public Playground(int width, int height, int pixelsPerCell, int borderWidth, UniverseConfiguration configuration)
        {
            Width = width;
            Height = height;
            _pixelsPerCell = pixelsPerCell;
            _borderWidth = borderWidth;
            Configuration = configuration;

            CreatePlayground();
            InitializeCells();
        }

        internal int Width { get; private set; }

        internal int Height { get; private set; }

        public Bitmap Body { get; private set; }

        public UniverseConfiguration Configuration { get; set; }

        public void Update(Cell cell)
        {
            using (var graphics = Graphics.FromImage(Body))
            {

                var currentColor = Body.GetPixel(cell.X, cell.Y);

                graphics.FillRectangle(
                    currentColor.ToArgb() == Color.DarkMagenta.ToArgb() ? Brushes.WhiteSmoke : Brushes.DarkMagenta,
                    cell.X,
                    cell.Y,
                    _pixelsPerCell,
                    _pixelsPerCell);
            }

            var cellToChange = GetCell(cell.X, cell.Y);
            cellToChange.IsAlive = !cellToChange.IsAlive;
        }

        internal Cell GetCell(int x, int y)
        {
            // TODO implement logic for closed universe

            if (((x < 0 || x >= Width) || (y < 0 || y >= Height)) 
                && Configuration == UniverseConfiguration.Limited)
            {
                return null;
            }

            return _cells[x < 0 ? Width + x : x >= Width ? x % Width : x, 
                y < 0 ? Height + y : y >= Height ? y % Height : y];
        }

        private void LoadGameFromBitmap(Bitmap playgroundBitmap)
        {
            // TODO 
        }

        public void LoadGameFromCells(Cell[,] cells)
        {
            if (cells.Rank != 2 
                || cells.GetLength(0) != Width
                || cells.GetLength(1) != Height)
            {
                throw new ArgumentException($"Parameter {nameof(cells)} has invalid rank or length.");
            }

            CreatePlayground();

            for (int x = 0; x < cells.GetLength(0); x++)
            {
                for (int y = 0; y < cells.GetLength(1); y++)
                {
                    var newCell = cells[x, y];
                    
                    if (newCell.IsAlive)
                    {
                        Update(newCell);
                    }
                }
            }
        }

        private void CreatePlayground()
        {
            var imageWidth = Width * _pixelsPerCell + _borderWidth;
            var imageHeight = Height * _pixelsPerCell + _borderWidth;

            var bitmap = new Bitmap(imageWidth, imageHeight);

            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.FillRectangle(Brushes.WhiteSmoke, 0, 0, imageWidth, imageHeight);
            }

            //for (int y = 0; y < imageHeight; y += _pixelsPerCell)
            //{
            //    graphics.FillRectangle(Brushes.Gray, 0, y, imageWidth, _borderWidth);
            //}

            //for (int x = 0; x < imageWidth; x += _pixelsPerCell)
            //{
            //    graphics.FillRectangle(Brushes.Gray, x, 0, _borderWidth, imageHeight);
            //}

            Body = bitmap;
        }

        private void InitializeCells()
        {
            _cells = new Cell[Width, Height];

            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    _cells[x, y] = new Cell(x, y, false);
                }
            }
        }
    }
}
