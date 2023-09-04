using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Salvo.Migrations
{
    public partial class addSalvoEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Salvos",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Turn = table.Column<int>(nullable: false),
                    GamePlayerId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salvos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Salvos_GamesPlayers_GamePlayerId",
                        column: x => x.GamePlayerId,
                        principalTable: "GamesPlayers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalvoLocations",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Location = table.Column<string>(nullable: true),
                    SalvoId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalvoLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalvoLocations_Salvos_SalvoId",
                        column: x => x.SalvoId,
                        principalTable: "Salvos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalvoLocations_SalvoId",
                table: "SalvoLocations",
                column: "SalvoId");

            migrationBuilder.CreateIndex(
                name: "IX_Salvos_GamePlayerId",
                table: "Salvos",
                column: "GamePlayerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalvoLocations");

            migrationBuilder.DropTable(
                name: "Salvos");
        }
    }
}
