using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using GameOfLife.Data.Entities;

namespace GameOfLife.Data
{
    public class GameLayoutsRepository : IGameLayoutsRepository
    {
        private readonly GameContext _context;

        public GameLayoutsRepository(GameContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Layout>> GetAllLayoutsAsync()
        {
            IQueryable<Layout> query = _context.Layouts;

            return await query.ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<Layout> GetLayoutByTitleAsync(string title)
        {
            IQueryable<Layout> query = _context.Layouts;
            query = query.Where(l => l.LayoutTitle == title);

            return await query.FirstOrDefaultAsync();
        }

        public void AddLayout(Layout newLayout)
        {
            _context.Layouts.Add(newLayout);
        }

        public void RemoveLayout(Layout layout)
        {
            _context.Layouts.Remove(layout);
        }
    }
}
