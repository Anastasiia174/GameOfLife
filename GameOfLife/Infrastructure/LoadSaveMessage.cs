using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameOfLife.Engine;

namespace GameOfLife.Infrastructure
{
    public class LoadSaveMessage
    {
        public LoadSaveMessage(GameSave gameToSave)
        {
            GameToSave = gameToSave;
        }

        public GameSave GameToSave { get; set; }
    }
}
