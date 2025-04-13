using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThiTracNghiem.Migrations
{
    /// <inheritdoc />
    public partial class AddChuDeAndCauHoi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChuDes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenChuDe = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChuDes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CauHois",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoiDung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DapAnA = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DapAnB = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DapAnC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DapAnD = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DapAnDung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChuDeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CauHois", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CauHois_ChuDes_ChuDeId",
                        column: x => x.ChuDeId,
                        principalTable: "ChuDes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CauHois_ChuDeId",
                table: "CauHois",
                column: "ChuDeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CauHois");

            migrationBuilder.DropTable(
                name: "ChuDes");
        }
    }
}
