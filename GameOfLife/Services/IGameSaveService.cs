using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameOfLife.Engine;

namespace GameOfLife.Services
{
    public interface IGameSaveService
    {
        IEnumerable<GameSave> GetAllGameSaves();

        void SaveGameSave(GameSave save);

        void RemoveGameSave(GameSave save);
    }
}
