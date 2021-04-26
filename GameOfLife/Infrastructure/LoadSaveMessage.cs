using GameOfLife.Engine;

namespace GameOfLife.Infrastructure
{
    public class LoadSaveMessage
    {
        public LoadSaveMessage(GameSave gameToLoad)
        {
            GameToLoad = gameToLoad;
        }

        public GameSave GameToLoad { get; set; }
    }
}
