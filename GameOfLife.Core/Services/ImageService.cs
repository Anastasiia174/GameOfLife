using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GameOfLife.Core.Extensions;

namespace GameOfLife.Core.Services
{
    public class ImageService
    {
        private const byte AllPixelsWhite = 255;

        private const byte AllPixelsGray = 170;

        private const byte FirstPixelGraySecondWhite = 175;

        public ImageSource RenderGrid(int width, int height)
        {
            const int pixelsPerCell = 8;
            const int borderWidth = 1;

            var pf = PixelFormats.Gray4;
            var imageWidth = width * pixelsPerCell + borderWidth;
            var imageHeight = height * pixelsPerCell + borderWidth;
            var rawStride = (imageWidth * pf.BitsPerPixel + 7) / 8;
            var rawImage = new byte[rawStride * imageHeight];

            rawImage.Fill(AllPixelsWhite);

            var bitmap = new WriteableBitmap(imageWidth, imageHeight, 96, 96, pf, null);
            var imageRect = new Int32Rect(0, 0, imageWidth, imageHeight);
            bitmap.WritePixels(imageRect, rawImage, rawStride, 0);

            var fullRowBorder = new byte[imageWidth];
            fullRowBorder.Fill(AllPixelsGray);

            var partRowBorder = new byte[imageWidth];
            partRowBorder.Fill(AllPixelsWhite);

            for (var i = 0; i < imageWidth; i += pixelsPerCell / 2)
            {
                partRowBorder[i] = FirstPixelGraySecondWhite;
            }

            for (var y = 0; y < imageHeight; y++)
            {
                Int32Rect gridRect = new Int32Rect(0, y, imageWidth, 1);
                bitmap.WritePixels(
                    gridRect, 
                    y % pixelsPerCell == 0 ? fullRowBorder : partRowBorder, 
                    rawStride, 
                    0);
            }

            return bitmap;
        }
    }
}
