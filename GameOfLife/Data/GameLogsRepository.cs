using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using GameOfLife.Data.Entities;

namespace GameOfLife.Data
{
    public class GameLogsRepository : IGameLogsRepository
    {
        private readonly GameContext _context;

        public GameLogsRepository(GameContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Log>> GetAllLogsAsync()
        {
            IQueryable<Log> query = _context.Logs;

            return await query.ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

        public void AddLog(Log newLog)
        {
            _context.Logs.Add(newLog);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
