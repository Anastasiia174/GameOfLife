using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameOfLife.Infrastructure;

namespace GameOfLife.Services
{
    public class GameLogService : IGameLogService
    {
        public Task<IEnumerable<GameLog>> GetAllGameLogsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveGameLogAsync(GameLog save)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveGameLogAsync(GameLog save)
        {
            throw new NotImplementedException();
        }
    }
}
