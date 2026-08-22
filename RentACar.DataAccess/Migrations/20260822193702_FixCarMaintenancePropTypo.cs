using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentACar.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixCarMaintenancePropTypo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CeheckOutTime",
                table: "CarMaintenances",
                newName: "CheckOutTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CheckOutTime",
                table: "CarMaintenances",
                newName: "CeheckOutTime");
        }
    }
}
