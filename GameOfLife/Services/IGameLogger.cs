using System.Drawing;

namespace GameOfLife.Services
{
    public interface IGameLogger
    {
        void LogInfo(string message);

        void LogInfo(string message, Bitmap playground);
    }
}
