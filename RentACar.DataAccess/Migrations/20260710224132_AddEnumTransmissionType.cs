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
            // AlterColumn yerine AddColumn yapıyoruz
            migrationBuilder.AddColumn<int>(
                name: "TransmissionType",
                table: "Cars",
                type: "integer",
                nullable: false,
                defaultValue: 1); // Varsayılan olarak 1 (Manual) atıyoruz
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransmissionType",
                table: "Cars");
        }
    }
}
