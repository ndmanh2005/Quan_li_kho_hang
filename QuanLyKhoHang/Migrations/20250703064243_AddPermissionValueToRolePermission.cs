using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyKhoHang.Migrations
{
    /// <inheritdoc />
    public partial class AddPermissionValueToRolePermission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanCreate",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "CanDelete",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "CanEdit",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "CanExport",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "CanImport",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "CanView",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "FunctionCode",
                table: "RolePermissions");

            migrationBuilder.AddColumn<string>(
                name: "PermissionValue",
                table: "RolePermissions",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PermissionValue",
                table: "RolePermissions");

            migrationBuilder.AddColumn<bool>(
                name: "CanCreate",
                table: "RolePermissions",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanDelete",
                table: "RolePermissions",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanEdit",
                table: "RolePermissions",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanExport",
                table: "RolePermissions",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanImport",
                table: "RolePermissions",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanView",
                table: "RolePermissions",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "FunctionCode",
                table: "RolePermissions",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
