using Microsoft.EntityFrameworkCore.Migrations;

namespace Lesson2ModelleringEntity.Migrations
{
    public partial class UpdatedSongsWithLyrics : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Lyrics",
                table: "Song",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Lyrics",
                table: "Song");
        }
    }
}
