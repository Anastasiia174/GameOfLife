using System;
using System.Drawing;

namespace GameOfLife.Engine
{
    public class GameSave
    {
        public int GenerationNumber { get; set; }

        public Bitmap Playground { get; set; }

        public bool GameEnded { get; set; }

        public UniverseConfiguration UniverseConfiguration { get; set; }

        public DateTime DateTime { get; set; }

        public string Title { get; set; }
    }
}
