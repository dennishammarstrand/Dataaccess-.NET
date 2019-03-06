using Microsoft.EntityFrameworkCore.Migrations;

namespace Lesson2ModelleringEntity.Migrations
{
    public partial class AddingRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Song_AlbumID",
                table: "Song",
                column: "AlbumID");

            migrationBuilder.CreateIndex(
                name: "IX_Album_ArtistID",
                table: "Album",
                column: "ArtistID");

            migrationBuilder.AddForeignKey(
                name: "FK_Album_Artist_ArtistID",
                table: "Album",
                column: "ArtistID",
                principalTable: "Artist",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Song_Album_AlbumID",
                table: "Song",
                column: "AlbumID",
                principalTable: "Album",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Album_Artist_ArtistID",
                table: "Album");

            migrationBuilder.DropForeignKey(
                name: "FK_Song_Album_AlbumID",
                table: "Song");

            migrationBuilder.DropIndex(
                name: "IX_Song_AlbumID",
                table: "Song");

            migrationBuilder.DropIndex(
                name: "IX_Album_ArtistID",
                table: "Album");
        }
    }
}
