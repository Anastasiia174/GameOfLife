using System;

namespace GameOfLife.Infrastructure
{
    public class SaveLayoutMessage
    {
        public Action<GameLayout> SaveAction { get; set; }

        public Action<string> ErrorAction { get; set; }
    }
}
