using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JWTAuthenticationWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class Updateusernameandpassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "LoginModels",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "Password", "UserName" },
                values: new object[] { "admin@123", "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "LoginModels",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "Password", "UserName" },
                values: new object[] { "def@123", "johndoe" });
        }
    }
}
