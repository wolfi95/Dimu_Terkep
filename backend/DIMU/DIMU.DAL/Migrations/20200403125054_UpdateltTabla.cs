using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DIMU.DAL.Migrations
{
    public partial class UpdateltTabla : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IntezmenyVezeto_Intezmenyek_IntezmenyId",
                table: "IntezmenyVezeto");

            migrationBuilder.DropTable(
                name: "Muveszek");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IntezmenyVezeto",
                table: "IntezmenyVezeto");

            migrationBuilder.DropColumn(
                name: "Esemenyek",
                table: "Intezmenyek");

            migrationBuilder.DropColumn(
                name: "Helyszin",
                table: "Intezmenyek");

            migrationBuilder.DropColumn(
                name: "Megjegyzes",
                table: "Intezmenyek");

            migrationBuilder.RenameTable(
                name: "IntezmenyVezeto",
                newName: "IntezmenyVezetok");

            migrationBuilder.RenameIndex(
                name: "IX_IntezmenyVezeto_IntezmenyId",
                table: "IntezmenyVezetok",
                newName: "IX_IntezmenyVezetok_IntezmenyId");

            migrationBuilder.AlterColumn<int>(
                name: "Megszunes",
                table: "Intezmenyek",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "IntezmenyId",
                table: "IntezmenyVezetok",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_IntezmenyVezetok",
                table: "IntezmenyVezetok",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Esemenyek",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Nev = table.Column<string>(nullable: true),
                    Datum = table.Column<DateTime>(nullable: false),
                    Szervezo = table.Column<string>(nullable: true),
                    IntezmenyId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Esemenyek", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Esemenyek_Intezmenyek_IntezmenyId",
                        column: x => x.IntezmenyId,
                        principalTable: "Intezmenyek",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IntezmenyHelyszinek",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Helyszin = table.Column<string>(nullable: true),
                    Nyitas = table.Column<int>(nullable: false),
                    Koltozes = table.Column<int>(nullable: true),
                    IntezmenyId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntezmenyHelyszinek", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IntezmenyHelyszinek_Intezmenyek_IntezmenyId",
                        column: x => x.IntezmenyId,
                        principalTable: "Intezmenyek",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Esemenyek_IntezmenyId",
                table: "Esemenyek",
                column: "IntezmenyId");

            migrationBuilder.CreateIndex(
                name: "IX_IntezmenyHelyszinek_IntezmenyId",
                table: "IntezmenyHelyszinek",
                column: "IntezmenyId");

            migrationBuilder.AddForeignKey(
                name: "FK_IntezmenyVezetok_Intezmenyek_IntezmenyId",
                table: "IntezmenyVezetok",
                column: "IntezmenyId",
                principalTable: "Intezmenyek",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IntezmenyVezetok_Intezmenyek_IntezmenyId",
                table: "IntezmenyVezetok");

            migrationBuilder.DropTable(
                name: "Esemenyek");

            migrationBuilder.DropTable(
                name: "IntezmenyHelyszinek");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IntezmenyVezetok",
                table: "IntezmenyVezetok");

            migrationBuilder.RenameTable(
                name: "IntezmenyVezetok",
                newName: "IntezmenyVezeto");

            migrationBuilder.RenameIndex(
                name: "IX_IntezmenyVezetok_IntezmenyId",
                table: "IntezmenyVezeto",
                newName: "IX_IntezmenyVezeto_IntezmenyId");

            migrationBuilder.AlterColumn<int>(
                name: "Megszunes",
                table: "Intezmenyek",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Esemenyek",
                table: "Intezmenyek",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Helyszin",
                table: "Intezmenyek",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Megjegyzes",
                table: "Intezmenyek",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "IntezmenyId",
                table: "IntezmenyVezeto",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddPrimaryKey(
                name: "PK_IntezmenyVezeto",
                table: "IntezmenyVezeto",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Muveszek",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IntezmenyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Nev = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Muveszek", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Muveszek_Intezmenyek_IntezmenyId",
                        column: x => x.IntezmenyId,
                        principalTable: "Intezmenyek",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Muveszek_IntezmenyId",
                table: "Muveszek",
                column: "IntezmenyId");

            migrationBuilder.AddForeignKey(
                name: "FK_IntezmenyVezeto_Intezmenyek_IntezmenyId",
                table: "IntezmenyVezeto",
                column: "IntezmenyId",
                principalTable: "Intezmenyek",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
