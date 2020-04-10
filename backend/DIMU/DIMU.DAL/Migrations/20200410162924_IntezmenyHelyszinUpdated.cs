using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DIMU.DAL.Migrations
{
    public partial class IntezmenyHelyszinUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Intezmenyek");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Intezmenyek");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "IntezmenyHelyszinek",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "IntezmenyHelyszinek",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<string>(
                name: "Datum",
                table: "Esemenyek",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "IntezmenyHelyszinek");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "IntezmenyHelyszinek");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Intezmenyek",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Intezmenyek",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Datum",
                table: "Esemenyek",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
