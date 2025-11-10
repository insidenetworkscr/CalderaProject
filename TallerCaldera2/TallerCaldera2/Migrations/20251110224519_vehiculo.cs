using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TallerCaldera2.Migrations
{
    /// <inheritdoc />
    public partial class vehiculo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alerts_Vehicles_VehicleId",
                table: "Alerts");

            migrationBuilder.DropForeignKey(
                name: "FK_Maintenances_Vehicles_VehicleId",
                table: "Maintenances");

            migrationBuilder.DropTable(
                name: "SketchMarks");

            migrationBuilder.DropTable(
                name: "Sketches");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vehicles",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_Plate",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Maintenances_VehicleId",
                table: "Maintenances");

            migrationBuilder.DropIndex(
                name: "IX_Alerts_VehicleId",
                table: "Alerts");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Plate",
                table: "Maintenances");

            migrationBuilder.DropColumn(
                name: "VehicleId",
                table: "Maintenances");

            migrationBuilder.AddColumn<string>(
                name: "VehiclePlate",
                table: "Maintenances",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VehiclePlate",
                table: "Alerts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vehicles",
                table: "Vehicles",
                column: "Plate");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_Plate",
                table: "Vehicles",
                column: "Plate",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Maintenances_VehiclePlate",
                table: "Maintenances",
                column: "VehiclePlate");

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_VehiclePlate",
                table: "Alerts",
                column: "VehiclePlate");

            migrationBuilder.AddForeignKey(
                name: "FK_Alerts_Vehicles_VehiclePlate",
                table: "Alerts",
                column: "VehiclePlate",
                principalTable: "Vehicles",
                principalColumn: "Plate",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Maintenances_Vehicles_VehiclePlate",
                table: "Maintenances",
                column: "VehiclePlate",
                principalTable: "Vehicles",
                principalColumn: "Plate",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alerts_Vehicles_VehiclePlate",
                table: "Alerts");

            migrationBuilder.DropForeignKey(
                name: "FK_Maintenances_Vehicles_VehiclePlate",
                table: "Maintenances");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vehicles",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_Plate",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Maintenances_VehiclePlate",
                table: "Maintenances");

            migrationBuilder.DropIndex(
                name: "IX_Alerts_VehiclePlate",
                table: "Alerts");

            migrationBuilder.DropColumn(
                name: "VehiclePlate",
                table: "Maintenances");

            migrationBuilder.DropColumn(
                name: "VehiclePlate",
                table: "Alerts");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Vehicles",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "Plate",
                table: "Maintenances",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "VehicleId",
                table: "Maintenances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vehicles",
                table: "Vehicles",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Sketches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleId = table.Column<int>(type: "int", nullable: false),
                    BaseImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Plate = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sketches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sketches_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SketchMarks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SketchId = table.Column<int>(type: "int", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Severity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    X = table.Column<double>(type: "float", nullable: false),
                    Y = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SketchMarks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SketchMarks_Sketches_SketchId",
                        column: x => x.SketchId,
                        principalTable: "Sketches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_Plate",
                table: "Vehicles",
                column: "Plate");

            migrationBuilder.CreateIndex(
                name: "IX_Maintenances_VehicleId",
                table: "Maintenances",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_VehicleId",
                table: "Alerts",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Sketches_VehicleId",
                table: "Sketches",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_SketchMarks_SketchId",
                table: "SketchMarks",
                column: "SketchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Alerts_Vehicles_VehicleId",
                table: "Alerts",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Maintenances_Vehicles_VehicleId",
                table: "Maintenances",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
