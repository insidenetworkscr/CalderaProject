using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TallerCaldera2.Migrations
{
    /// <inheritdoc />
    public partial class RemoveVehicleImagesAndFiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SketchJson",
                table: "Maintenances");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SketchJson",
                table: "Maintenances",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
