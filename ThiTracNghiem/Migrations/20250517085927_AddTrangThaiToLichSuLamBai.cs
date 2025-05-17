using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThiTracNghiem.Migrations
{
    /// <inheritdoc />
    public partial class AddTrangThaiToLichSuLamBai : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TrangThai",
                table: "LichSuLamBais",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrangThai",
                table: "LichSuLamBais");
        }
    }
}
