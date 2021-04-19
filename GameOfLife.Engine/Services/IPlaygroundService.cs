using System.Drawing;

namespace GameOfLife.Engine.Services
{
    internal interface IPlaygroundService
    {
        Bitmap CreatePlayground(int width, int height);

        Bitmap ToggleCellState(int x, int y, Bitmap playground);
    }
}
