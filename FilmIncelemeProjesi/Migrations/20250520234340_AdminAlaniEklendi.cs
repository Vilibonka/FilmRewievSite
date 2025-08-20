using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FilmIncelemeProjesi.Migrations
{
    /// <inheritdoc />
    public partial class AdminAlaniEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AdminMi",
                table: "Kullanicilar",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminMi",
                table: "Kullanicilar");
        }
    }
}
