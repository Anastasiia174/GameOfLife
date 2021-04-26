using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameOfLife.Data.Entities;

namespace GameOfLife.Data
{
    public interface IGameLogsRepository
    {
        Task<IEnumerable<Log>> GetAllLogsAsync();

        Task<bool> SaveAllAsync();

        Task<Log> GetLogByTitleAsync(string title);

        void AddLog(Log newLog);

        void RemoveLog(Log log);
    }
}
