using System.Drawing;

namespace GameOfLife.Engine
{
    public interface IGameEngine
    {
        int CurrentGenerationNumber { get; }

        bool GameEnded { get; }

        Bitmap Playground { get; }

        void MoveToNextGeneration();

        void ChangeUniverseState(Cell cell);

        GameSave SaveGame();

        void LoadGame(GameSave save);

        void RandomizeGame();

        void ResetGame(int width, int height, UniverseConfiguration configuration);
    }
}
