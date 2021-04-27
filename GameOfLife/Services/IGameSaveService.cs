using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameOfLife.Engine;

namespace GameOfLife.Services
{
    public interface IGameSaveService : IDisposable
    {
        Task<IEnumerable<GameSave>> GetAllGameSavesAsync(bool includePlayground = false);

        Task<GameSave> GetGameSaveByTitleAsync(string title);

        Task<bool> SaveGameSaveAsync(GameSave save);

        Task<bool> RemoveGameSaveAsync(GameSave save);
    }
}
