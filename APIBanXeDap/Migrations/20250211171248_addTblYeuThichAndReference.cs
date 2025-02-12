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

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON001",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 3, 14, 0, 12, 43, 426, DateTimeKind.Local).AddTicks(9735), new DateTime(2025, 2, 12, 0, 12, 43, 426, DateTimeKind.Local).AddTicks(9703) });

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON002",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 2, 27, 0, 12, 43, 426, DateTimeKind.Local).AddTicks(9771), new DateTime(2025, 2, 12, 0, 12, 43, 426, DateTimeKind.Local).AddTicks(9763) });

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON003",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 3, 4, 0, 12, 43, 426, DateTimeKind.Local).AddTicks(9794), new DateTime(2025, 2, 12, 0, 12, 43, 426, DateTimeKind.Local).AddTicks(9788) });

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON004",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 2, 22, 0, 12, 43, 426, DateTimeKind.Local).AddTicks(9815), new DateTime(2025, 2, 12, 0, 12, 43, 426, DateTimeKind.Local).AddTicks(9810) });

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON005",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 3, 9, 0, 12, 43, 426, DateTimeKind.Local).AddTicks(9838), new DateTime(2025, 2, 12, 0, 12, 43, 426, DateTimeKind.Local).AddTicks(9832) });

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

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON001",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 2, 22, 17, 13, 18, 248, DateTimeKind.Local).AddTicks(6172), new DateTime(2025, 1, 23, 17, 13, 18, 248, DateTimeKind.Local).AddTicks(6156) });

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON002",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 2, 7, 17, 13, 18, 248, DateTimeKind.Local).AddTicks(6183), new DateTime(2025, 1, 23, 17, 13, 18, 248, DateTimeKind.Local).AddTicks(6181) });

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON003",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 2, 12, 17, 13, 18, 248, DateTimeKind.Local).AddTicks(6188), new DateTime(2025, 1, 23, 17, 13, 18, 248, DateTimeKind.Local).AddTicks(6187) });

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON004",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 2, 2, 17, 13, 18, 248, DateTimeKind.Local).AddTicks(6193), new DateTime(2025, 1, 23, 17, 13, 18, 248, DateTimeKind.Local).AddTicks(6191) });

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON005",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 2, 17, 17, 13, 18, 248, DateTimeKind.Local).AddTicks(6275), new DateTime(2025, 1, 23, 17, 13, 18, 248, DateTimeKind.Local).AddTicks(6274) });
        }
    }
}
