using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TwoFAAPI.Migrations
{
    /// <inheritdoc />
    public partial class backupcoe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BackupCode",
                table: "HrEmp",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Is2faenable",
                table: "HrEmp",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TotpDisableon",
                table: "HrEmp",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TotpDisablereason",
                table: "HrEmp",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "TotpEnableon",
                table: "HrEmp",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TotpSecretkey",
                table: "HrEmp",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TotpVerifyCode",
                table: "HrEmp",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "HrEmpTwoFABackupcode",
                columns: table => new
                {
                    BackupCodeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmpId = table.Column<int>(type: "integer", nullable: false),
                    BackupCode = table.Column<string>(type: "text", nullable: false),
                    IsUsed = table.Column<bool>(type: "boolean", nullable: true),
                    OrgId = table.Column<int>(type: "integer", nullable: true),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<int>(type: "integer", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HrEmpTwoFABackupcode", x => x.BackupCodeId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HrEmpTwoFABackupcode");

            migrationBuilder.DropColumn(
                name: "BackupCode",
                table: "HrEmp");

            migrationBuilder.DropColumn(
                name: "Is2faenable",
                table: "HrEmp");

            migrationBuilder.DropColumn(
                name: "TotpDisableon",
                table: "HrEmp");

            migrationBuilder.DropColumn(
                name: "TotpDisablereason",
                table: "HrEmp");

            migrationBuilder.DropColumn(
                name: "TotpEnableon",
                table: "HrEmp");

            migrationBuilder.DropColumn(
                name: "TotpSecretkey",
                table: "HrEmp");

            migrationBuilder.DropColumn(
                name: "TotpVerifyCode",
                table: "HrEmp");
        }
    }
}
