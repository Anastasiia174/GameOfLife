using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameOfLife.Infrastructure;

namespace GameOfLife.Services
{
    public interface IGameLogService
    {
        Task<IEnumerable<GameLog>> GetAllGameLogsAsync();

        Task<bool> SaveGameLogAsync(GameLog save);

        Task<bool> RemoveGameLogAsync(GameLog save);
    }
}
