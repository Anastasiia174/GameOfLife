namespace GameOfLife.Engine
{
    public interface IGameEngine
    {
        int CurrentGenerationNumber { get; }

        bool GameEnded { get; }

        void MoveToNextGeneration();

        void ChangeUniverseState(Cell cell);
    }
}
