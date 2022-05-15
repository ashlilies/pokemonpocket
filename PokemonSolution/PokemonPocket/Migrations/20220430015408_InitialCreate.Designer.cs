﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PokemonPocket;

namespace PokemonPocket.Migrations
{
    [DbContext(typeof(PokemonDbContext))]
    [Migration("20220430015408_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.16");

            modelBuilder.Entity("PokemonPocket.Pokemon", b =>
                {
                    b.Property<int>("PokemonId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Exp")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Hp")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Skill")
                        .HasColumnType("TEXT");

                    b.HasKey("PokemonId");

                    b.ToTable("Pokemons");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Pokemon");
                });

            modelBuilder.Entity("PokemonPocket.Charmander", b =>
                {
                    b.HasBaseType("PokemonPocket.Pokemon");

                    b.HasDiscriminator().HasValue("Charmander");
                });

            modelBuilder.Entity("PokemonPocket.Eevee", b =>
                {
                    b.HasBaseType("PokemonPocket.Pokemon");

                    b.HasDiscriminator().HasValue("Eevee");
                });

            modelBuilder.Entity("PokemonPocket.Pikachu", b =>
                {
                    b.HasBaseType("PokemonPocket.Pokemon");

                    b.HasDiscriminator().HasValue("Pikachu");
                });

            modelBuilder.Entity("PokemonPocket.Charmeleon", b =>
                {
                    b.HasBaseType("PokemonPocket.Charmander");

                    b.HasDiscriminator().HasValue("Charmeleon");
                });

            modelBuilder.Entity("PokemonPocket.Flareon", b =>
                {
                    b.HasBaseType("PokemonPocket.Eevee");

                    b.HasDiscriminator().HasValue("Flareon");
                });

            modelBuilder.Entity("PokemonPocket.Raichu", b =>
                {
                    b.HasBaseType("PokemonPocket.Pikachu");

                    b.HasDiscriminator().HasValue("Raichu");
                });
#pragma warning restore 612, 618
        }
    }
}