using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TallerCaldera2.Migrations
{
    /// <inheritdoc />
    public partial class ProformaSimple : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProformaDetalles");

            migrationBuilder.RenameColumn(
                name: "Total",
                table: "Proformas",
                newName: "PrecioUnitario");

            migrationBuilder.AddColumn<int>(
                name: "Cantidad",
                table: "Proformas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DescripcionServicio",
                table: "Proformas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cantidad",
                table: "Proformas");

            migrationBuilder.DropColumn(
                name: "DescripcionServicio",
                table: "Proformas");

            migrationBuilder.RenameColumn(
                name: "PrecioUnitario",
                table: "Proformas",
                newName: "Total");

            migrationBuilder.CreateTable(
                name: "ProformaDetalles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProformaId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProformaDetalles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProformaDetalles_Proformas_ProformaId",
                        column: x => x.ProformaId,
                        principalTable: "Proformas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProformaDetalles_ProformaId",
                table: "ProformaDetalles",
                column: "ProformaId");
        }
    }
}
