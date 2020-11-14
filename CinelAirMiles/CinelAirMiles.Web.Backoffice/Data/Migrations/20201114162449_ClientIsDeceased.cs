using Microsoft.EntityFrameworkCore.Migrations;

namespace CinelAirMiles.Web.Backoffice.Data.Migrations
{
    public partial class ClientIsDeceased : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeceased",
                table: "Clients",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeceased",
                table: "Clients");
        }
    }
}
