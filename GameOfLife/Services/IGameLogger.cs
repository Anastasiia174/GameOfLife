using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameOfLife.Infrastructure;

namespace GameOfLife.Services
{
    public interface IGameLogger
    {
        void LogInfo(string message);

        void LogInfo(string message, Bitmap playground);
    }
}
