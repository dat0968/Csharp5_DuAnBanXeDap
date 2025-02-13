using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIBanXeDap.Migrations
{
    /// <inheritdoc />
    public partial class haskeyforchitiethoadon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.AddPrimaryKey(
                name: "PK_CHITIETHOADON",
                table: "CHITIETHOADON",
                columns: new[] { "MaHoaDon", "MaSP", "MaMau", "MaKichThuoc" });

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON001",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 3, 14, 10, 36, 8, 472, DateTimeKind.Local).AddTicks(5960), new DateTime(2025, 2, 12, 10, 36, 8, 472, DateTimeKind.Local).AddTicks(5947) });

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON002",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 2, 27, 10, 36, 8, 472, DateTimeKind.Local).AddTicks(5972), new DateTime(2025, 2, 12, 10, 36, 8, 472, DateTimeKind.Local).AddTicks(5967) });

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON003",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 3, 4, 10, 36, 8, 472, DateTimeKind.Local).AddTicks(6028), new DateTime(2025, 2, 12, 10, 36, 8, 472, DateTimeKind.Local).AddTicks(6026) });

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON004",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 2, 22, 10, 36, 8, 472, DateTimeKind.Local).AddTicks(6032), new DateTime(2025, 2, 12, 10, 36, 8, 472, DateTimeKind.Local).AddTicks(6031) });

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON005",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 3, 9, 10, 36, 8, 472, DateTimeKind.Local).AddTicks(6035), new DateTime(2025, 2, 12, 10, 36, 8, 472, DateTimeKind.Local).AddTicks(6034) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CHITIETHOADON",
                table: "CHITIETHOADON");

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON001",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 3, 10, 15, 57, 3, 917, DateTimeKind.Local).AddTicks(5600), new DateTime(2025, 2, 8, 15, 57, 3, 917, DateTimeKind.Local).AddTicks(5584) });

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON002",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 2, 23, 15, 57, 3, 917, DateTimeKind.Local).AddTicks(5612), new DateTime(2025, 2, 8, 15, 57, 3, 917, DateTimeKind.Local).AddTicks(5610) });

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON003",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 2, 28, 15, 57, 3, 917, DateTimeKind.Local).AddTicks(5617), new DateTime(2025, 2, 8, 15, 57, 3, 917, DateTimeKind.Local).AddTicks(5616) });

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON004",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 2, 18, 15, 57, 3, 917, DateTimeKind.Local).AddTicks(5623), new DateTime(2025, 2, 8, 15, 57, 3, 917, DateTimeKind.Local).AddTicks(5621) });

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON005",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 3, 5, 15, 57, 3, 917, DateTimeKind.Local).AddTicks(5630), new DateTime(2025, 2, 8, 15, 57, 3, 917, DateTimeKind.Local).AddTicks(5627) });

            migrationBuilder.CreateIndex(
                name: "IX_CHITIETHOADON_MaHoaDon",
                table: "CHITIETHOADON",
                column: "MaHoaDon");
        }
    }
}
