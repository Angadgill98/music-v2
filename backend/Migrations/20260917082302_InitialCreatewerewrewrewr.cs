using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreatewerewrewrewr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<Guid>>(
                name: "liked_albums",
                table: "UsersTable",
                type: "uuid[]",
                nullable: false);

            migrationBuilder.AddColumn<List<Guid>>(
                name: "liked_songs",
                table: "UsersTable",
                type: "uuid[]",
                nullable: false);

            migrationBuilder.AddColumn<List<Guid>>(
                name: "playlists",
                table: "UsersTable",
                type: "uuid[]",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "category",
                table: "SongsTable",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "visibility",
                table: "SongsTable",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "musician_id",
                table: "AlbumsTable",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "visibility",
                table: "AlbumsTable",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "MusiciansTable",
                columns: table => new
                {
                    musician_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    musician_name = table.Column<string>(type: "text", nullable: false),
                    songs = table.Column<List<Guid>>(type: "uuid[]", nullable: false),
                    albums = table.Column<List<Guid>>(type: "uuid[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MusiciansTable", x => x.musician_id);
                });

            migrationBuilder.CreateTable(
                name: "PlaylistsTable",
                columns: table => new
                {
                    playlist_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    songs = table.Column<List<Guid>>(type: "uuid[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaylistsTable", x => x.playlist_id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MusiciansTable");

            migrationBuilder.DropTable(
                name: "PlaylistsTable");

            migrationBuilder.DropColumn(
                name: "liked_albums",
                table: "UsersTable");

            migrationBuilder.DropColumn(
                name: "liked_songs",
                table: "UsersTable");

            migrationBuilder.DropColumn(
                name: "playlists",
                table: "UsersTable");

            migrationBuilder.DropColumn(
                name: "category",
                table: "SongsTable");

            migrationBuilder.DropColumn(
                name: "visibility",
                table: "SongsTable");

            migrationBuilder.DropColumn(
                name: "musician_id",
                table: "AlbumsTable");

            migrationBuilder.DropColumn(
                name: "visibility",
                table: "AlbumsTable");
        }
    }
}
