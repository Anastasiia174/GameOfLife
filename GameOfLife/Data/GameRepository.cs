using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameOfLife.Data.Entities;

namespace GameOfLife.Data
{
    public class GameRepository : IGameRepository
    {
        private readonly GameContext _context;

        public GameRepository(GameContext context)
        {
            _context = context;

            _context.Saves.Load();
        }

        public IEnumerable<Save> GetAllSaves()
        {
            return _context.Saves.ToList();
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }

        public Save GetSaveById(int id)
        {
            throw new NotImplementedException();
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
