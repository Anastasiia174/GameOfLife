using System.Collections.Generic;
using System.Threading.Tasks;
using GameOfLife.Data.Entities;

namespace GameOfLife.Data
{
    public interface IGameLogsRepository
    {
        Task<IEnumerable<Log>> GetAllLogsAsync();

        Task<bool> SaveAllAsync();

        void AddLog(Log newLog);
    }
}
