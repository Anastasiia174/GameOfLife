using System.Drawing;

namespace GameOfLife.Engine.Services
{
    internal class PlaygroundService : IPlaygroundService
    {
        private const int PixelsPerCell = 1;

        private const int BorderWidth = 0;

        public Bitmap CreatePlayground(int width, int height)
        {
            var imageWidth = width * PixelsPerCell + BorderWidth;
            var imageHeight = height * PixelsPerCell + BorderWidth;

            var bitmap = new Bitmap(imageWidth, imageHeight);
            
            var graphics = Graphics.FromImage(bitmap);

            graphics.FillRectangle(Brushes.WhiteSmoke, 0, 0, imageWidth, imageHeight);

            //for (int y = 0; y < imageHeight; y += PixelsPerCell)
            //{
            //    graphics.FillRectangle(Brushes.Gray, 0, y, imageWidth, 1);
            //}

            //for (int x = 0; x < imageWidth; x += PixelsPerCell)
            //{
            //    graphics.FillRectangle(Brushes.Gray, x, 0, 1, imageHeight);
            //}

            return bitmap;
        }

        public Bitmap ToggleCellState(int x, int y, Bitmap playground)
        {
            //var cellWidth = playgroundWidth / playground.Width;
            //var cellHeight = playgroundHeight / playground.Height;

            //var pixelX = (int) (x / cellWidth);
            //var pixelY = (int) (y / cellHeight);

            var graphics = Graphics.FromImage(playground);

            var currentColor = playground.GetPixel(x, y);

            graphics.FillRectangle(
                currentColor.ToArgb() == Color.DarkMagenta.ToArgb() ? Brushes.WhiteSmoke : Brushes.DarkMagenta,
                x,
                y, 
                PixelsPerCell, 
                PixelsPerCell);

            return playground;
        }
    }
}
