using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FilmIncelemeProjesi.Migrations
{
    /// <inheritdoc />
    public partial class YorumlaraKullaniciEkle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "KullaniciAdi",
                table: "Yorumlar",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KullaniciAdi",
                table: "Yorumlar");
        }
    }
}
