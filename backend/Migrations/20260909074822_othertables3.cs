using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class othertables3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlbumsTable_Musicians_Musiciansmusician_id",
                table: "AlbumsTable");

            migrationBuilder.DropForeignKey(
                name: "FK_SongsTable_AlbumsTable_Albumsalbum_id",
                table: "SongsTable");

            migrationBuilder.DropTable(
                name: "MusiciansSongs");

            migrationBuilder.DropTable(
                name: "Musicians");

            migrationBuilder.DropIndex(
                name: "IX_SongsTable_Albumsalbum_id",
                table: "SongsTable");

            migrationBuilder.DropIndex(
                name: "IX_AlbumsTable_Musiciansmusician_id",
                table: "AlbumsTable");

            migrationBuilder.DropColumn(
                name: "Albumsalbum_id",
                table: "SongsTable");

            migrationBuilder.DropColumn(
                name: "Musiciansmusician_id",
                table: "AlbumsTable");

            migrationBuilder.AddColumn<Guid>(
                name: "musician_id",
                table: "SongsTable",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<List<Guid>>(
                name: "other_singers",
                table: "SongsTable",
                type: "uuid[]",
                nullable: false);

            migrationBuilder.AddColumn<List<Guid>>(
                name: "songs",
                table: "AlbumsTable",
                type: "uuid[]",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "musician_id",
                table: "SongsTable");

            migrationBuilder.DropColumn(
                name: "other_singers",
                table: "SongsTable");

            migrationBuilder.DropColumn(
                name: "songs",
                table: "AlbumsTable");

            migrationBuilder.AddColumn<Guid>(
                name: "Albumsalbum_id",
                table: "SongsTable",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Musiciansmusician_id",
                table: "AlbumsTable",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Musicians",
                columns: table => new
                {
                    musician_id = table.Column<Guid>(type: "uuid", nullable: false),
                    musician_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Musicians", x => x.musician_id);
                });

            migrationBuilder.CreateTable(
                name: "MusiciansSongs",
                columns: table => new
                {
                    other_singersmusician_id = table.Column<Guid>(type: "uuid", nullable: false),
                    songssong_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MusiciansSongs", x => new { x.other_singersmusician_id, x.songssong_id });
                    table.ForeignKey(
                        name: "FK_MusiciansSongs_Musicians_other_singersmusician_id",
                        column: x => x.other_singersmusician_id,
                        principalTable: "Musicians",
                        principalColumn: "musician_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MusiciansSongs_SongsTable_songssong_id",
                        column: x => x.songssong_id,
                        principalTable: "SongsTable",
                        principalColumn: "song_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SongsTable_Albumsalbum_id",
                table: "SongsTable",
                column: "Albumsalbum_id");

            migrationBuilder.CreateIndex(
                name: "IX_AlbumsTable_Musiciansmusician_id",
                table: "AlbumsTable",
                column: "Musiciansmusician_id");

            migrationBuilder.CreateIndex(
                name: "IX_MusiciansSongs_songssong_id",
                table: "MusiciansSongs",
                column: "songssong_id");

            migrationBuilder.AddForeignKey(
                name: "FK_AlbumsTable_Musicians_Musiciansmusician_id",
                table: "AlbumsTable",
                column: "Musiciansmusician_id",
                principalTable: "Musicians",
                principalColumn: "musician_id");

            migrationBuilder.AddForeignKey(
                name: "FK_SongsTable_AlbumsTable_Albumsalbum_id",
                table: "SongsTable",
                column: "Albumsalbum_id",
                principalTable: "AlbumsTable",
                principalColumn: "album_id");
        }
    }
}
