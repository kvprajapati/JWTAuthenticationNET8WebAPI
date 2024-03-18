using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JWTAuthenticationWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddtrackingIdinformation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                table: "LoginModels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Timestamp",
                table: "LoginModels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TrackingID",
                table: "LoginModels",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "LoginModels",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "IpAddress", "Timestamp", "TrackingID" },
                values: new object[] { null, null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IpAddress",
                table: "LoginModels");

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "LoginModels");

            migrationBuilder.DropColumn(
                name: "TrackingID",
                table: "LoginModels");
        }
    }
}
