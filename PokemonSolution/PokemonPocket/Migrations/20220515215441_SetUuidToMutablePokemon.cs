using Microsoft.EntityFrameworkCore.Migrations;

namespace PokemonPocket.Migrations
{
    public partial class SetUuidToMutablePokemon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Uuid",
                table: "Pokemons",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Uuid",
                table: "Pokemons");
        }
    }
}
