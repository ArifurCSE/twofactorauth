using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwoFAAPI.Migrations
{
    /// <inheritdoc />
    public partial class emailandpassfieldadd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "HrEmp",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "HrEmp",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "HrEmp");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "HrEmp");
        }
    }
}
