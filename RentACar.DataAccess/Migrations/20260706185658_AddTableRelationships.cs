using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentACar.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddTableRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Rentals_CarId",
                table: "Rentals",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_CustomerId",
                table: "Rentals",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_DropOffOfficeId",
                table: "Rentals",
                column: "DropOffOfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_PickUpOfficeId",
                table: "Rentals",
                column: "PickUpOfficeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_Cars_CarId",
                table: "Rentals",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_Customers_CustomerId",
                table: "Rentals",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Cars_CarId",
                table: "Rentals");

            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Customers_CustomerId",
                table: "Rentals");

            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Offices_DropOffOfficeId",
                table: "Rentals");

            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Offices_PickUpOfficeId",
                table: "Rentals");

            migrationBuilder.DropIndex(
                name: "IX_Rentals_CarId",
                table: "Rentals");

            migrationBuilder.DropIndex(
                name: "IX_Rentals_CustomerId",
                table: "Rentals");

            migrationBuilder.DropIndex(
                name: "IX_Rentals_DropOffOfficeId",
                table: "Rentals");

            migrationBuilder.DropIndex(
                name: "IX_Rentals_PickUpOfficeId",
                table: "Rentals");
        }
    }
}
