using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameOfLife.Infrastructure;

namespace GameOfLife.Services
{
    public interface IGameLayoutService
    {
        Task<IEnumerable<GameLayout>> GetAllGameLayoutsAsync(bool includeLayout = false);

        Task<GameLayout> GetGameLayoutByTitleAsync(string title);

        Task<bool> SaveGameLayoutAsync(GameLayout layout);

        Task<bool> RemoveGameLayoutAsync(GameLayout layout);
    }
}
