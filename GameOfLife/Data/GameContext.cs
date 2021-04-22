using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameOfLife.Data.Entities;

namespace GameOfLife.Data
{
    public class GameContext : DbContext
    {
        public GameContext()
        : base("DefaultConnection")
        {
            
        }

        public DbSet<Save> Saves { get; set; }
    }
}
