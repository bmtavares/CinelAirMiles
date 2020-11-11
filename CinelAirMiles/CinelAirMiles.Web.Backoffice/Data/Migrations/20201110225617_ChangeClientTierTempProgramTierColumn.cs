using Microsoft.EntityFrameworkCore.Migrations;

namespace CinelAirMiles.Web.Backoffice.Data.Migrations
{
    public partial class ChangeClientTierTempProgramTierColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChangeConfirmed",
                table: "ChangeClientsTierTemp");

            migrationBuilder.AddColumn<int>(
                name: "ProgramTierId",
                table: "ChangeClientsTierTemp",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChangeClientsTierTemp_ProgramTierId",
                table: "ChangeClientsTierTemp",
                column: "ProgramTierId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChangeClientsTierTemp_ProgramTiers_ProgramTierId",
                table: "ChangeClientsTierTemp",
                column: "ProgramTierId",
                principalTable: "ProgramTiers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChangeClientsTierTemp_ProgramTiers_ProgramTierId",
                table: "ChangeClientsTierTemp");

            migrationBuilder.DropIndex(
                name: "IX_ChangeClientsTierTemp_ProgramTierId",
                table: "ChangeClientsTierTemp");

            migrationBuilder.DropColumn(
                name: "ProgramTierId",
                table: "ChangeClientsTierTemp");

            migrationBuilder.AddColumn<bool>(
                name: "ChangeConfirmed",
                table: "ChangeClientsTierTemp",
                nullable: true);
        }
    }
}
