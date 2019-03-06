using Microsoft.EntityFrameworkCore.Migrations;

namespace Lesson2ModelleringEntity.Migrations
{
    public partial class AddingRelationsThirdWay : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Album_Artist_ArtistID",
                table: "Album");

            migrationBuilder.DropForeignKey(
                name: "FK_Song_Album_AlbumID",
                table: "Song");

            migrationBuilder.AlterColumn<int>(
                name: "AlbumID",
                table: "Song",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "ArtistID",
                table: "Album",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Album_Artist_ArtistID",
                table: "Album",
                column: "ArtistID",
                principalTable: "Artist",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Song_Album_AlbumID",
                table: "Song",
                column: "AlbumID",
                principalTable: "Album",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Album_Artist_ArtistID",
                table: "Album");

            migrationBuilder.DropForeignKey(
                name: "FK_Song_Album_AlbumID",
                table: "Song");

            migrationBuilder.AlterColumn<int>(
                name: "AlbumID",
                table: "Song",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ArtistID",
                table: "Album",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

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
    }
}
