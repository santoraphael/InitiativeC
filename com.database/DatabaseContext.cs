﻿using com.database.entities;
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

        public DbSet<User> Users { get; set; }
        public DbSet<TokenPool> TokenPool { get; set; }
        public DbSet<TokenBounty> TokenBounties { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                // Converter o nome da tabela para minúsculas
                entity.SetTableName(entity.GetTableName().ToLower());
            }

            base.OnModelCreating(modelBuilder);

            // Configurar a chave primária
            modelBuilder.Entity<User>()
                .HasKey(u => u.id);

            // Configurar índice único para stake_address
            modelBuilder.Entity<User>()
                .HasIndex(u => u.stake_address)
                .IsUnique();



            modelBuilder.Entity<TokenBounty>(entity =>
            {
                entity.ToTable("tokenbounties"); // Garante que o nome está em minúsculas
            });

            // Configurar a relação entre TokenBounty e User
            modelBuilder.Entity<TokenBounty>()
                .HasOne(tb => tb.User)
                .WithOne(u => u.TokenBounty)
                .HasForeignKey<TokenBounty>(tb => tb.id_usuario)
                .OnDelete(DeleteBehavior.Cascade); // Defina o comportamento de deleção conforme necessário

            // Garantir a unicidade de id_usuario na tabela TokenBounty
            modelBuilder.Entity<TokenBounty>()
                .HasIndex(tb => tb.id_usuario)
                .IsUnique();
        }
    }
}
