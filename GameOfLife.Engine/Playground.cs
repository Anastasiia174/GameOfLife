using System;
using System.Drawing;

namespace GameOfLife.Engine
{
    internal class Playground : IDisposable
    {
        private const int PixelsPerCell = 1;

        private Cell[,] _cells;

        public Playground(int width, int height, UniverseConfiguration configuration = UniverseConfiguration.Limited)
        {
            Width = width;
            Height = height;
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
                    PixelsPerCell,
                    PixelsPerCell);
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

        public void LoadGameFromBitmap(Bitmap playgroundBitmap)
        {
            Width = playgroundBitmap.Width;
            Height = playgroundBitmap.Height;

            InitializeCells();

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    var pixelColor = playgroundBitmap.GetPixel(x, y);
                    if (pixelColor.ToArgb() == Color.DarkMagenta.ToArgb())
                    {
                        _cells[x, y].IsAlive = true;
                    }
                }
            }

            Body = playgroundBitmap;
        }

        public void LoadGameFromCells(Cell[,] cells)
        {
            if (cells.Rank != 2 
                || cells.GetLength(0) != Width
                || cells.GetLength(1) != Height)
            {
                throw new ArgumentException($"Parameter {nameof(cells)} has invalid rank or length.");
            }

            InitializeCells();
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
            var imageWidth = Width * PixelsPerCell;
            var imageHeight = Height * PixelsPerCell;

            var bitmap = new Bitmap(imageWidth, imageHeight);

            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.FillRectangle(Brushes.WhiteSmoke, 0, 0, imageWidth, imageHeight);
            }

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

        public void Dispose()
        {
            Body?.Dispose();
            _cells = null;
        }
    }
}
