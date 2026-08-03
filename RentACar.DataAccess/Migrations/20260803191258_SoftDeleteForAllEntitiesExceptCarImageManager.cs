using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentACar.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SoftDeleteForAllEntitiesExceptCarImageManager : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Users",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "UserOperationClaims",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Rentals",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "OperationClaims",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Offices",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Customers",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "ContactMessages",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "ContactInfos",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Colors",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Brands",
                newName: "IsDeleted");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "UserOperationClaims",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Rentals",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "OperationClaims",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Offices",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Customers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "ContactMessages",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "ContactInfos",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Colors",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Brands",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "UserOperationClaims");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "OperationClaims");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Offices");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "ContactMessages");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "ContactInfos");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Colors");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Brands");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Users",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "UserOperationClaims",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Rentals",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "OperationClaims",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Offices",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Customers",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "ContactMessages",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "ContactInfos",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Colors",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Brands",
                newName: "Status");
        }
    }
}
