using Microsoft.EntityFrameworkCore.Migrations;

namespace CinelAirMiles.Web.Backoffice.Data.Migrations
{
    public partial class FixMileTypeFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Miles_MilesTypeId",
                table: "Miles");

            migrationBuilder.CreateIndex(
                name: "IX_Miles_MilesTypeId",
                table: "Miles",
                column: "MilesTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Miles_MilesTypeId",
                table: "Miles");

            migrationBuilder.CreateIndex(
                name: "IX_Miles_MilesTypeId",
                table: "Miles",
                column: "MilesTypeId",
                unique: true);
        }
    }
}
