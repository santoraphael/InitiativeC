using com.database.entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public DbSet<user> users { get; set; }
        public DbSet<tokenpool> tokenpool { get; set; }
        public DbSet<tokenbounty> tokenbounties { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                // Converter o nome da tabela para minúsculas
                entity.SetTableName(entity.GetTableName().ToLower());
            }

            base.OnModelCreating(modelBuilder);

            // Configurar a relação entre TokenBounty e User
            modelBuilder.Entity<tokenbounty>()
                .HasOne(tb => tb.user)
                .WithOne(u => u.tokenbounty)
                .HasForeignKey<tokenbounty>(tb => tb.id_usuario)
                .OnDelete(DeleteBehavior.Cascade); // Defina o comportamento de deleção conforme necessário

            // Garantir a unicidade de id_usuario na tabela TokenBounty
            modelBuilder.Entity<tokenbounty>()
                .HasIndex(tb => tb.id_usuario)
                .IsUnique();
        }
    }
}
