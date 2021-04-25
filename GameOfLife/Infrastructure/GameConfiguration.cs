using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameOfLife.Engine;

namespace GameOfLife.Infrastructure
{
    public class GameConfiguration
    {
        public GameConfiguration(int width, int height, UniverseConfiguration universeConfiguration, bool isEditable = false)
        {
            Width = width;
            Height = height;
            UniverseConfiguration = universeConfiguration;
            IsEditable = isEditable;
        }

        public int Width { get; }

        public int Height { get; }

        public UniverseConfiguration UniverseConfiguration { get; }

        public bool IsEditable { get; }
    }
}
