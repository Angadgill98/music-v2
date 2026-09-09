using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class othertables2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "AlbumsTable",
                columns: table => new
                {
                    album_id = table.Column<Guid>(type: "uuid", nullable: false),
                    album_name = table.Column<string>(type: "text", nullable: false),
                    likes = table.Column<int>(type: "integer", nullable: false),
                    Musiciansmusician_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlbumsTable", x => x.album_id);
                    table.ForeignKey(
                        name: "FK_AlbumsTable_Musicians_Musiciansmusician_id",
                        column: x => x.Musiciansmusician_id,
                        principalTable: "Musicians",
                        principalColumn: "musician_id");
                });

            migrationBuilder.CreateTable(
                name: "SongsTable",
                columns: table => new
                {
                    song_id = table.Column<Guid>(type: "uuid", nullable: false),
                    song_name = table.Column<string>(type: "text", nullable: false),
                    likes = table.Column<int>(type: "integer", nullable: false),
                    Albumsalbum_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SongsTable", x => x.song_id);
                    table.ForeignKey(
                        name: "FK_SongsTable_AlbumsTable_Albumsalbum_id",
                        column: x => x.Albumsalbum_id,
                        principalTable: "AlbumsTable",
                        principalColumn: "album_id");
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
                name: "IX_AlbumsTable_Musiciansmusician_id",
                table: "AlbumsTable",
                column: "Musiciansmusician_id");

            migrationBuilder.CreateIndex(
                name: "IX_MusiciansSongs_songssong_id",
                table: "MusiciansSongs",
                column: "songssong_id");

            migrationBuilder.CreateIndex(
                name: "IX_SongsTable_Albumsalbum_id",
                table: "SongsTable",
                column: "Albumsalbum_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MusiciansSongs");

            migrationBuilder.DropTable(
                name: "SongsTable");

            migrationBuilder.DropTable(
                name: "AlbumsTable");

            migrationBuilder.DropTable(
                name: "Musicians");
        }
    }
}
