using System;
using GameOfLife.Engine;

namespace GameOfLife.Infrastructure
{
    public class SaveGameMessage
    {
        public string GameTitle { get; set; }

        public Action<GameSave> SaveAction { get; set; }

        public Action<string> ErrorAction { get; set; }
    }
}
