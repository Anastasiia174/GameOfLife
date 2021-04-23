using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.Data.Entities
{
    public class Save
    {
        public int SaveId { get; set; }

        public DateTime SaveDtm { get; set; }

        public string SaveTitle { get; set; }

        public int SaveGeneration { get; set; }

        public byte[] SaveData { get; set; }

        public bool SaveGameEnded { get; set; }

        public bool SaveIsClosUniv { get; set; }
    }
}
