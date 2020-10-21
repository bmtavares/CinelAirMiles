using Microsoft.EntityFrameworkCore.Migrations;

namespace CinelAirMiles.Web.Backoffice.Data.Migrations
{
    public partial class MileDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Miles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Miles");
        }
    }
}
