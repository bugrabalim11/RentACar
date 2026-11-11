using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentACar.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddEnumTransmissionType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Önceki InitialCreate ile string (text) olarak gelen eski kolonu siliyoruz
            migrationBuilder.DropColumn(
                name: "TransmissionType",
                table: "Cars");

            // Ardından enum (int) tipindeki yeni kolonu ekliyoruz
            migrationBuilder.AddColumn<int>(
                name: "TransmissionType",
                table: "Cars",
                type: "integer",
                nullable: false,
                defaultValue: 1); // Varsayılan olarak 1 (Manual)
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransmissionType",
                table: "Cars");

            migrationBuilder.AddColumn<string>(
                name: "TransmissionType",
                table: "Cars",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
