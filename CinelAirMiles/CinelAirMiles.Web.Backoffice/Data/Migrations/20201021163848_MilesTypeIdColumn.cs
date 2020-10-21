using Microsoft.EntityFrameworkCore.Migrations;

namespace CinelAirMiles.Web.Backoffice.Data.Migrations
{
    public partial class MilesTypeIdColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Miles_MilesTypeId",
                table: "Miles");

            migrationBuilder.AlterColumn<int>(
                name: "MilesTypeId",
                table: "Miles",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Miles_MilesTypeId",
                table: "Miles",
                column: "MilesTypeId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Miles_MilesTypeId",
                table: "Miles");

            migrationBuilder.AlterColumn<int>(
                name: "MilesTypeId",
                table: "Miles",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_Miles_MilesTypeId",
                table: "Miles",
                column: "MilesTypeId");
        }
    }
}
