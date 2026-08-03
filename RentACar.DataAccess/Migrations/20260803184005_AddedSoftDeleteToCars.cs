using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentACar.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedSoftDeleteToCars : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Cars",
                newName: "IsDeleted");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Cars",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Cars");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Cars",
                newName: "Status");
        }
    }
}
