using System;
using System.Drawing;

namespace GameOfLife.Infrastructure
{
    public class GameLog
    {
        public string Event { get; set; }

        public DateTime EventDateTime { get; set; }

        public Bitmap Playground { get; set; }
    }
}
