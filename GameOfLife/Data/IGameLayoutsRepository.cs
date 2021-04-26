using System.Collections.Generic;
using System.Threading.Tasks;
using GameOfLife.Data.Entities;

namespace GameOfLife.Data
{
    public interface IGameLayoutsRepository
    {
        Task<IEnumerable<Layout>> GetAllLayoutsAsync();

        Task<bool> SaveAllAsync();

        Task<Layout> GetLayoutByTitleAsync(string title);

        void AddLayout(Layout newLayout);

        void RemoveLayout(Layout layout);
    }
}
