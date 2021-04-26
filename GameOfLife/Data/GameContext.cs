using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
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

        public DbSet<Log> Logs { get; set; }

        public DbSet<Layout> Layouts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Save>()
                .Property(p => p.SaveId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Log>()
                .Property(p => p.LogId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Layout>()
                .Property(p => p.LayoutId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}
