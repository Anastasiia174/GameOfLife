using System.Collections.Generic;
using System.Threading.Tasks;
using GameOfLife.Infrastructure;

namespace GameOfLife.Services
{
    public interface IGameLogService
    {
        Task<IEnumerable<GameLog>> GetAllGameLogsAsync();

        Task<bool> SaveGameLogAsync(GameLog save);
    }
}
