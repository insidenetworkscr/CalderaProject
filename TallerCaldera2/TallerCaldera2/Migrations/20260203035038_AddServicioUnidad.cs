using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TallerCaldera2.Migrations
{
    /// <inheritdoc />
    public partial class AddServicioUnidad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceItem_Maintenances_MaintenanceId",
                table: "MaintenanceItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MaintenanceItem",
                table: "MaintenanceItem");

            migrationBuilder.RenameTable(
                name: "MaintenanceItem",
                newName: "MaintenanceItems");

            migrationBuilder.RenameIndex(
                name: "IX_MaintenanceItem_MaintenanceId",
                table: "MaintenanceItems",
                newName: "IX_MaintenanceItems_MaintenanceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MaintenanceItems",
                table: "MaintenanceItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceItems_Maintenances_MaintenanceId",
                table: "MaintenanceItems",
                column: "MaintenanceId",
                principalTable: "Maintenances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceItems_Maintenances_MaintenanceId",
                table: "MaintenanceItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MaintenanceItems",
                table: "MaintenanceItems");

            migrationBuilder.RenameTable(
                name: "MaintenanceItems",
                newName: "MaintenanceItem");

            migrationBuilder.RenameIndex(
                name: "IX_MaintenanceItems_MaintenanceId",
                table: "MaintenanceItem",
                newName: "IX_MaintenanceItem_MaintenanceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MaintenanceItem",
                table: "MaintenanceItem",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceItem_Maintenances_MaintenanceId",
                table: "MaintenanceItem",
                column: "MaintenanceId",
                principalTable: "Maintenances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
