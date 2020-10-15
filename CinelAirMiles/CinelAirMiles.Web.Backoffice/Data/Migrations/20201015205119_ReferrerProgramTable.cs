using Microsoft.EntityFrameworkCore.Migrations;

namespace CinelAirMiles.Web.Backoffice.Data.Migrations
{
    public partial class ReferrerProgramTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReferrersProgram",
                columns: table => new
                {
                    ReferrerClientId = table.Column<int>(nullable: false),
                    ReferredClientId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReferrersProgram", x => new { x.ReferredClientId, x.ReferrerClientId });
                    table.ForeignKey(
                        name: "FK_ReferrersProgram_Clients_ReferredClientId",
                        column: x => x.ReferredClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReferrersProgram_Clients_ReferrerClientId",
                        column: x => x.ReferrerClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReferrersProgram_ReferredClientId",
                table: "ReferrersProgram",
                column: "ReferredClientId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReferrersProgram_ReferrerClientId",
                table: "ReferrersProgram",
                column: "ReferrerClientId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReferrersProgram");
        }
    }
}
