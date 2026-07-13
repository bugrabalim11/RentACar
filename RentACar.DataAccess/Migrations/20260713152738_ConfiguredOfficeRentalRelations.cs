using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentACar.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ConfiguredOfficeRentalRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Offices_DropOffOfficeId",
                table: "Rentals");

            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Offices_PickUpOfficeId",
                table: "Rentals");

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_Offices_DropOffOfficeId",
                table: "Rentals",
                column: "DropOffOfficeId",
                principalTable: "Offices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_Offices_PickUpOfficeId",
                table: "Rentals",
                column: "PickUpOfficeId",
                principalTable: "Offices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Offices_DropOffOfficeId",
                table: "Rentals");

            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Offices_PickUpOfficeId",
                table: "Rentals");

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_Offices_DropOffOfficeId",
                table: "Rentals",
                column: "DropOffOfficeId",
                principalTable: "Offices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_Offices_PickUpOfficeId",
                table: "Rentals",
                column: "PickUpOfficeId",
                principalTable: "Offices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
