using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameOfLife.Data.Entities;

namespace GameOfLife.Data
{
    public class GameSavesRepository : IGameSavesRepository
    {
        private readonly GameContext _context;

        public GameSavesRepository(GameContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Save>> GetAllSavesAsync()
        {
            IQueryable<Save> query = _context.Saves;

            return await query.ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<Save> GetSaveByTitleAsync(string title)
        {
            IQueryable<Save> query = _context.Saves;
            query = query.Where(s => s.SaveTitle == title);

            return await query.FirstOrDefaultAsync();
        }

        public void AddSave(Save newSave)
        {
            _context.Saves.Add(newSave);
        }

        public void RemoveSave(Save saveToRemove)
        {
            _context.Saves.Remove(saveToRemove);
        }
    }
}
