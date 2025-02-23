using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIBanXeDap.Migrations
{
    /// <inheritdoc />
    public partial class addComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Binhluans",
                columns: table => new
                {
                    MaBinhLuan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoiDung = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaSP = table.Column<int>(type: "int", nullable: false),
                    MaKH = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Binhluans", x => x.MaBinhLuan);
                    table.ForeignKey(
                        name: "FK_Binhluans_KHACHHANG_MaKH",
                        column: x => x.MaKH,
                        principalTable: "KHACHHANG",
                        principalColumn: "MaKH",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Binhluans_SANPHAM_MaSP",
                        column: x => x.MaSP,
                        principalTable: "SANPHAM",
                        principalColumn: "MaSP",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Traloibinhluans",
                columns: table => new
                {
                    MaTraLoi = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoiDung = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ThoiGian = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaBinhLuan = table.Column<int>(type: "int", nullable: false),
                    MaNV = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Traloibinhluans", x => x.MaTraLoi);
                    table.ForeignKey(
                        name: "FK_Traloibinhluans_Binhluans_MaBinhLuan",
                        column: x => x.MaBinhLuan,
                        principalTable: "Binhluans",
                        principalColumn: "MaBinhLuan",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Traloibinhluans_NHANVIEN_MaNV",
                        column: x => x.MaNV,
                        principalTable: "NHANVIEN",
                        principalColumn: "MaNV",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON001",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 3, 22, 23, 58, 27, 680, DateTimeKind.Local).AddTicks(9114), new DateTime(2025, 2, 20, 23, 58, 27, 680, DateTimeKind.Local).AddTicks(9095) });

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON002",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 3, 7, 23, 58, 27, 680, DateTimeKind.Local).AddTicks(9124), new DateTime(2025, 2, 20, 23, 58, 27, 680, DateTimeKind.Local).AddTicks(9122) });

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON003",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 3, 12, 23, 58, 27, 680, DateTimeKind.Local).AddTicks(9128), new DateTime(2025, 2, 20, 23, 58, 27, 680, DateTimeKind.Local).AddTicks(9127) });

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON004",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 3, 2, 23, 58, 27, 680, DateTimeKind.Local).AddTicks(9139), new DateTime(2025, 2, 20, 23, 58, 27, 680, DateTimeKind.Local).AddTicks(9138) });

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON005",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 3, 17, 23, 58, 27, 680, DateTimeKind.Local).AddTicks(9142), new DateTime(2025, 2, 20, 23, 58, 27, 680, DateTimeKind.Local).AddTicks(9141) });

            migrationBuilder.CreateIndex(
                name: "IX_Binhluans_MaKH",
                table: "Binhluans",
                column: "MaKH");

            migrationBuilder.CreateIndex(
                name: "IX_Binhluans_MaSP",
                table: "Binhluans",
                column: "MaSP");

            migrationBuilder.CreateIndex(
                name: "IX_Traloibinhluans_MaBinhLuan",
                table: "Traloibinhluans",
                column: "MaBinhLuan");

            migrationBuilder.CreateIndex(
                name: "IX_Traloibinhluans_MaNV",
                table: "Traloibinhluans",
                column: "MaNV");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Traloibinhluans");

            migrationBuilder.DropTable(
                name: "Binhluans");

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON001",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 3, 20, 18, 48, 27, 3, DateTimeKind.Local).AddTicks(3337), new DateTime(2025, 2, 18, 18, 48, 27, 3, DateTimeKind.Local).AddTicks(3306) });

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON002",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 3, 5, 18, 48, 27, 3, DateTimeKind.Local).AddTicks(3360), new DateTime(2025, 2, 18, 18, 48, 27, 3, DateTimeKind.Local).AddTicks(3356) });

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON003",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 3, 10, 18, 48, 27, 3, DateTimeKind.Local).AddTicks(3372), new DateTime(2025, 2, 18, 18, 48, 27, 3, DateTimeKind.Local).AddTicks(3368) });

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON004",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 2, 28, 18, 48, 27, 3, DateTimeKind.Local).AddTicks(3382), new DateTime(2025, 2, 18, 18, 48, 27, 3, DateTimeKind.Local).AddTicks(3379) });

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON005",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 3, 15, 18, 48, 27, 3, DateTimeKind.Local).AddTicks(3392), new DateTime(2025, 2, 18, 18, 48, 27, 3, DateTimeKind.Local).AddTicks(3389) });
        }
    }
}
