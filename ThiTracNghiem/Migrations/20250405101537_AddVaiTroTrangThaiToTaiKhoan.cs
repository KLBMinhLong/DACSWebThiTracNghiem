using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThiTracNghiem.Migrations
{
    /// <inheritdoc />
    public partial class AddVaiTroTrangThaiToTaiKhoan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AnhDaiDienUrl",
                table: "TaiKhoans",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ThoiGianTao",
                table: "TaiKhoans",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "TrangThaiKhoa",
                table: "TaiKhoans",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "VaiTro",
                table: "TaiKhoans",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnhDaiDienUrl",
                table: "TaiKhoans");

            migrationBuilder.DropColumn(
                name: "ThoiGianTao",
                table: "TaiKhoans");

            migrationBuilder.DropColumn(
                name: "TrangThaiKhoa",
                table: "TaiKhoans");

            migrationBuilder.DropColumn(
                name: "VaiTro",
                table: "TaiKhoans");
        }
    }
}
