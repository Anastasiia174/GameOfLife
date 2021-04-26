using GameOfLife.Engine;

namespace GameOfLife.Infrastructure
{
    public class GameConfiguration
    {
        public GameConfiguration(int width, int height, UniverseConfiguration universeConfiguration = UniverseConfiguration.Limited, bool isEditable = false)
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
