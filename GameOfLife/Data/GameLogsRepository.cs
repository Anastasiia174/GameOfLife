using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
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

        public async Task<Log> GetLogByTitleAsync(string title)
        {
            IQueryable<Log> query = _context.Logs;
            query = query.Where(log => log.LogTitle == title);

            return await query.FirstOrDefaultAsync();
        }

        public void AddLog(Log newLog)
        {
            _context.Logs.Add(newLog);
        }

        public void RemoveLog(Log log)
        {
            _context.Logs.Remove(log);
        }
    }
}
