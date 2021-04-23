using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.Engine
{
    public class GameSave
    {
        public int GenerationNumber { get; set; }

        public Bitmap Playground { get; set; }

        public bool GameEnded { get; set; }

        public UniverseConfiguration UniverseConfiguration { get; set; }
    }
}
