using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace GameOfLife.Extensions
{
    public static class ImageConverter
    {
        public static byte[] ImageToByteArray(Image image, ImageFormat format)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, format);
                return ms.ToArray();
            }
        }

        public static Bitmap ByteArrayToBitmap(byte[] data)
        {
            var ms = new MemoryStream(data);
            var bitmap = new Bitmap(ms);
            
            return bitmap;
        }
    }
}
