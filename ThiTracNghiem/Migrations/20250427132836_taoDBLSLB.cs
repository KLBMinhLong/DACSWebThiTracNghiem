using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThiTracNghiem.Migrations
{
    /// <inheritdoc />
    public partial class taoDBLSLB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LichSuLamBais",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenTaiKhoan = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DeThiId = table.Column<int>(type: "int", nullable: false),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayNopBai = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Diem = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LichSuLamBais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LichSuLamBais_DeThis_DeThiId",
                        column: x => x.DeThiId,
                        principalTable: "DeThis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LichSuLamBais_TaiKhoans_TenTaiKhoan",
                        column: x => x.TenTaiKhoan,
                        principalTable: "TaiKhoans",
                        principalColumn: "TenTaiKhoan",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietLamBais",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LichSuLamBaiId = table.Column<int>(type: "int", nullable: false),
                    CauHoiId = table.Column<int>(type: "int", nullable: false),
                    DapAnChon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DungHaySai = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietLamBais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChiTietLamBais_CauHois_CauHoiId",
                        column: x => x.CauHoiId,
                        principalTable: "CauHois",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietLamBais_LichSuLamBais_LichSuLamBaiId",
                        column: x => x.LichSuLamBaiId,
                        principalTable: "LichSuLamBais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietLamBais_CauHoiId",
                table: "ChiTietLamBais",
                column: "CauHoiId");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietLamBais_LichSuLamBaiId",
                table: "ChiTietLamBais",
                column: "LichSuLamBaiId");

            migrationBuilder.CreateIndex(
                name: "IX_LichSuLamBais_DeThiId",
                table: "LichSuLamBais",
                column: "DeThiId");

            migrationBuilder.CreateIndex(
                name: "IX_LichSuLamBais_TenTaiKhoan",
                table: "LichSuLamBais",
                column: "TenTaiKhoan");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietLamBais");

            migrationBuilder.DropTable(
                name: "LichSuLamBais");

        }
    }
}
