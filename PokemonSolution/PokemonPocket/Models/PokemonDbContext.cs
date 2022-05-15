/*
 * Name: Ashlee Tan
 * Admin number: 211362G
 */

using System;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace PokemonPocket {
    public class PokemonDbContext : DbContext {
        public DbSet<Pokemon> Pokemons { get; set; }
        public string DbPath { get; }

        public PokemonDbContext() {
            var path = Directory.GetCurrentDirectory();
            DbPath = System.IO.Path.Join(path, "pocket.db");
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options) {
            options.UseSqlite($"Data Source={DbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Charmander>().HasBaseType<Pokemon>();
            modelBuilder.Entity<Charmeleon>().HasBaseType<Charmander>();
            modelBuilder.Entity<Eevee>().HasBaseType<Pokemon>();
            modelBuilder.Entity<Flareon>().HasBaseType<Eevee>();
            modelBuilder.Entity<Pikachu>().HasBaseType<Pokemon>();
            modelBuilder.Entity<Raichu>().HasBaseType<Pikachu>();
        }
    }
}