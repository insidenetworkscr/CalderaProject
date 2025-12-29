using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TallerCaldera2.Migrations
{
    /// <inheritdoc />
    public partial class CreateAppointments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Vehicles_VehiclePlate",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_VehiclePlate",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "VehiclePlate",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "Appointments",
                newName: "Vehicle");

            migrationBuilder.RenameColumn(
                name: "ClientPhone",
                table: "Appointments",
                newName: "Plate");

            migrationBuilder.RenameColumn(
                name: "ClientName",
                table: "Appointments",
                newName: "PhoneNumber");

            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "Vehicle",
                table: "Appointments",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "Plate",
                table: "Appointments",
                newName: "ClientPhone");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Appointments",
                newName: "ClientName");

            migrationBuilder.AddColumn<string>(
                name: "VehiclePlate",
                table: "Appointments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_VehiclePlate",
                table: "Appointments",
                column: "VehiclePlate");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Vehicles_VehiclePlate",
                table: "Appointments",
                column: "VehiclePlate",
                principalTable: "Vehicles",
                principalColumn: "Plate",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
