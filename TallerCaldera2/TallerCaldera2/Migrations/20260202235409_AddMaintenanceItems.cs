using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TallerCaldera2.Migrations
{
    /// <inheritdoc />
    public partial class AddMaintenanceItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MaintenanceItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Servicio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Unidad = table.Column<int>(type: "int", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaintenanceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaintenanceItem_Maintenances_MaintenanceId",
                        column: x => x.MaintenanceId,
                        principalTable: "Maintenances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceItem_MaintenanceId",
                table: "MaintenanceItem",
                column: "MaintenanceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaintenanceItem");
        }
    }
}
