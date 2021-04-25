﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameOfLife.Data.Entities;

namespace GameOfLife.Data
{
    public interface IGameRepository
    {
        Task<IEnumerable<Save>> GetAllSavesAsync();

        Task<bool> SaveAllAsync();

        Task<Save> GetSaveByTitleAsync(string title);

        void AddSave(Save newSave);

        void RemoveSave(Save saveToRemove);
    }
}
