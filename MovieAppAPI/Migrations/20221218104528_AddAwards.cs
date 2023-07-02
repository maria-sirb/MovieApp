using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieAppAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddAwards : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OscarNominations",
                table: "Movies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OscarWins",
                table: "Movies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Born",
                table: "Directors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "OscarNominations",
                table: "Directors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OscarWins",
                table: "Directors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Born",
                table: "Actors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "OscarNominations",
                table: "Actors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OscarWins",
                table: "Actors",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OscarNominations",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "OscarWins",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "Born",
                table: "Directors");

            migrationBuilder.DropColumn(
                name: "OscarNominations",
                table: "Directors");

            migrationBuilder.DropColumn(
                name: "OscarWins",
                table: "Directors");

            migrationBuilder.DropColumn(
                name: "Born",
                table: "Actors");

            migrationBuilder.DropColumn(
                name: "OscarNominations",
                table: "Actors");

            migrationBuilder.DropColumn(
                name: "OscarWins",
                table: "Actors");
        }
    }
}
