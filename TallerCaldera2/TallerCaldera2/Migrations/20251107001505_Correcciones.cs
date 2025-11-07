using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TallerCaldera2.Migrations
{
    /// <inheritdoc />
    public partial class Correcciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileAttachments");

            migrationBuilder.RenameColumn(
                name: "SketchMarksJson",
                table: "Vehicles",
                newName: "PhotoPath");

            migrationBuilder.AlterColumn<string>(
                name: "Plate",
                table: "Vehicles",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Sketches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleId = table.Column<int>(type: "int", nullable: false),
                    Plate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BaseImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                    X = table.Column<double>(type: "float", nullable: false),
                    Y = table.Column<double>(type: "float", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Severity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                name: "IX_Sketches_VehicleId",
                table: "Sketches",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_SketchMarks_SketchId",
                table: "SketchMarks",
                column: "SketchId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SketchMarks");

            migrationBuilder.DropTable(
                name: "Sketches");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_Plate",
                table: "Vehicles");

            migrationBuilder.RenameColumn(
                name: "PhotoPath",
                table: "Vehicles",
                newName: "SketchMarksJson");

            migrationBuilder.AlterColumn<string>(
                name: "Plate",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateTable(
                name: "FileAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaintenanceId = table.Column<int>(type: "int", nullable: true),
                    VehicleId = table.Column<int>(type: "int", nullable: true),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RelativePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileAttachments_Maintenances_MaintenanceId",
                        column: x => x.MaintenanceId,
                        principalTable: "Maintenances",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FileAttachments_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileAttachments_MaintenanceId",
                table: "FileAttachments",
                column: "MaintenanceId");

            migrationBuilder.CreateIndex(
                name: "IX_FileAttachments_VehicleId",
                table: "FileAttachments",
                column: "VehicleId");
        }
    }
}
