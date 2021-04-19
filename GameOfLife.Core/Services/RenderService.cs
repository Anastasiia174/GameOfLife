using System.Drawing;
using System.Drawing.Imaging;

namespace GameOfLife.Core.Services
{
    public class RenderService : IRenderService
    {
        public string CreatePlayground(int width, int height)
        {
            const int pixelsPerCell = 8;
            const int borderWidth = 1;

            var imageWidth = width * pixelsPerCell + borderWidth;
            var imageHeight = height * pixelsPerCell + borderWidth;

            var bitmap = new Bitmap(imageWidth, imageHeight);
            
            var graphics = Graphics.FromImage(bitmap);

            graphics.FillRectangle(Brushes.White, 0, 0, imageWidth, imageHeight);

            for (int y = 0; y < imageHeight; y += pixelsPerCell)
            {
                graphics.FillRectangle(Brushes.Gray, 0, y, imageWidth, 1);
            }

            for (int x = 0; x < imageWidth; x += pixelsPerCell)
            {
                graphics.FillRectangle(Brushes.Gray, x, 0, 1, imageHeight);
            }

            //var stream = new MemoryStream();
            //bitmap.Save(stream, ImageFormat.Bmp);
            bitmap.Save("grid.bmp", ImageFormat.Bmp);

            return @"grid.bmp";
        }
    }
}
