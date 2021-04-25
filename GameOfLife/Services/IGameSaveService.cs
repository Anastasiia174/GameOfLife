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
        Task<IEnumerable<GameSave>> GetAllGameSavesAsync();

        Task<bool> SaveGameSaveAsync(GameSave save);

        Task<bool> RemoveGameSaveAsync(GameSave save);
    }
}
