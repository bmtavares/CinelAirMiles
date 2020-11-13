using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CinelAirMiles.Web.Backoffice.Data.Migrations
{
    public partial class AddedMilesTransactionTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MilesTransactionTypeId",
                table: "MilesTransactions",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MilesTransactionTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MilesTransactionTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MilesTransactions_MilesTransactionTypeId",
                table: "MilesTransactions",
                column: "MilesTransactionTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_MilesTransactions_MilesTransactionTypes_MilesTransactionTypeId",
                table: "MilesTransactions",
                column: "MilesTransactionTypeId",
                principalTable: "MilesTransactionTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MilesTransactions_MilesTransactionTypes_MilesTransactionTypeId",
                table: "MilesTransactions");

            migrationBuilder.DropTable(
                name: "MilesTransactionTypes");

            migrationBuilder.DropIndex(
                name: "IX_MilesTransactions_MilesTransactionTypeId",
                table: "MilesTransactions");

            migrationBuilder.DropColumn(
                name: "MilesTransactionTypeId",
                table: "MilesTransactions");
        }
    }
}
