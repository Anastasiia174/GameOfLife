using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
