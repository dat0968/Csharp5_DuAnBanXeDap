using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIBanXeDap.Migrations
{
    /// <inheritdoc />
    public partial class addTblYeuThichAndReference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "YEUTHICH",
                columns: table => new
                {
                    Ma = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaDoiTuong = table.Column<int>(type: "int", nullable: false),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false),
                    DoiTuongYeuThich = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YEUTHICH", x => x.Ma);
                    table.ForeignKey(
                        name: "FK_YEUTHICH_KHACHHANG_MaNguoiDung",
                        column: x => x.MaNguoiDung,
                        principalTable: "KHACHHANG",
                        principalColumn: "MaKH");
                    table.ForeignKey(
                        name: "FK_YEUTHICH_SANPHAM_MaDoiTuong",
                        column: x => x.MaDoiTuong,
                        principalTable: "SANPHAM",
                        principalColumn: "MaSP");
                });

   

            migrationBuilder.CreateIndex(
                name: "IX_YEUTHICH_MaDoiTuong",
                table: "YEUTHICH",
                column: "MaDoiTuong");

            migrationBuilder.CreateIndex(
                name: "IX_YEUTHICH_MaNguoiDung",
                table: "YEUTHICH",
                column: "MaNguoiDung");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "YEUTHICH");

     
        }
    }
}
