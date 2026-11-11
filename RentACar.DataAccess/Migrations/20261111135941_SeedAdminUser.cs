using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RentACar.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "OperationClaims",
                columns: new[] { "Id", "CreatedDate", "DeletedDate", "IsDeleted", "Name", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "admin", null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "user", null }
                });

            migrationBuilder.InsertData(
                table: "UserOperationClaims",
                columns: new[] { "Id", "CreatedDate", "DeletedDate", "IsDeleted", "OperationClaimId", "UpdatedDate", "UserId" },
                values: new object[] { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 1, null, 1 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDate", "DeletedDate", "Email", "FirstName", "IsDeleted", "LastName", "PasswordHash", "PasswordSalt", "UpdatedDate" },
                values: new object[] { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "admin@rentacar.com", "System", false, "Admin", new byte[] { 180, 192, 64, 32, 230, 169, 164, 144, 236, 193, 180, 60, 56, 5, 104, 255, 61, 196, 212, 45, 147, 28, 44, 146, 176, 254, 124, 214, 84, 48, 195, 124, 16, 224, 85, 67, 78, 40, 3, 179, 130, 208, 113, 229, 54, 200, 138, 5, 229, 125, 86, 55, 61, 46, 133, 53, 218, 177, 210, 169, 117, 86, 236, 0 }, new byte[] { 82, 101, 110, 116, 65, 67, 97, 114, 45, 83, 101, 99, 114, 101, 116, 45, 83, 116, 97, 116, 105, 99, 45, 83, 97, 108, 116, 45, 75, 101, 121, 45, 49, 50, 51 }, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OperationClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "OperationClaims",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "UserOperationClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
