using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NanoSingular.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Identity",
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1", "754fe3d2-14aa-456b-b71a-20bc9869acb9", "root", "ROOT" },
                    { "2", "c15ad436-c380-4adb-ab2d-0e2492bee14d", "admin", "ADMIN" },
                    { "3", "b4f79638-4077-44fa-b9c8-bae6e919226c", "editor", "EDITOR" },
                    { "4", "1f920f41-87aa-4289-be0e-84da5b887f6a", "basic", "BASIC" }
                });

            migrationBuilder.InsertData(
                schema: "Identity",
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedBy", "CreatedOn", "DarkModeDefault", "DeletedBy", "DeletedOn", "Email", "EmailConfirmed", "FirstName", "ImageUrl", "IsActive", "IsDeleted", "LastModifiedBy", "LastModifiedOn", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PageSizeDefault", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "55555555-5555-5555-5555-555555555555", 0, "48cf6657-577c-45e3-a528-b782856bd4e2", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, null, null, "admin@email.com", true, "Default", null, true, false, null, null, "Admin", false, null, "ADMIN@EMAIL.COM", "ADMIN@EMAIL.COM.ROOT", 10, "AQAAAAIAAYagAAAAEHKUiac9f17y86vlGimlB7ITitErzBTVkoazSKya15OlQGFe10lxSEn5N6gcp7xzCg==", null, true, "c3c3b81b-1ccf-4e5c-a0fd-988199e83179", false, "admin@email.com.root" });

            migrationBuilder.InsertData(
                schema: "Identity",
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "1", "55555555-5555-5555-5555-555555555555" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Identity",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                schema: "Identity",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "3");

            migrationBuilder.DeleteData(
                schema: "Identity",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "4");

            migrationBuilder.DeleteData(
                schema: "Identity",
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "1", "55555555-5555-5555-5555-555555555555" });

            migrationBuilder.DeleteData(
                schema: "Identity",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                schema: "Identity",
                table: "Users",
                keyColumn: "Id",
                keyValue: "55555555-5555-5555-5555-555555555555");
        }
    }
}
