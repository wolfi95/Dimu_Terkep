using Microsoft.EntityFrameworkCore.Migrations;

namespace DIMU.DAL.Migrations
{
    public partial class IntezmenyVezeto_Ig_Nullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Ig",
                table: "IntezmenyVezetok",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Ig",
                table: "IntezmenyVezetok",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
