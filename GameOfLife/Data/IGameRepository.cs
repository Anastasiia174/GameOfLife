using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameOfLife.Data.Entities;

namespace GameOfLife.Data
{
    public interface IGameRepository
    {
        IEnumerable<Save> GetAllSaves();

        bool SaveAll();

        Save GetSaveById(int id);

        void AddSave(Save newSave);

        void RemoveSave(Save saveToRemove);
    }
}
