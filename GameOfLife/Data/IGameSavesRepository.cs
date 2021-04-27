using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameOfLife.Data.Entities;

namespace GameOfLife.Data
{
    public interface IGameSavesRepository : IDisposable
    {
        Task<IEnumerable<Save>> GetAllSavesAsync();

        Task<bool> SaveAllAsync();

        Task<Save> GetSaveByTitleAsync(string title);

        void AddSave(Save newSave);

        void RemoveSave(Save saveToRemove);
    }
}
