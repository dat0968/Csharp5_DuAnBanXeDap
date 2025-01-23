using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIBanXeDap.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnHinhForKhachtableandNhanVienTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Hinh",
                table: "NHANVIEN",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Hinh",
                table: "KHACHHANG",
                type: "nvarchar(max)",
                nullable: true);

           
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hinh",
                table: "NHANVIEN");

            migrationBuilder.DropColumn(
                name: "Hinh",
                table: "KHACHHANG");

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON001",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 2, 14, 8, 24, 38, 277, DateTimeKind.Local).AddTicks(1718), new DateTime(2025, 1, 15, 8, 24, 38, 277, DateTimeKind.Local).AddTicks(1700) });

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON002",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 1, 30, 8, 24, 38, 277, DateTimeKind.Local).AddTicks(1732), new DateTime(2025, 1, 15, 8, 24, 38, 277, DateTimeKind.Local).AddTicks(1728) });

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON003",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 2, 4, 8, 24, 38, 277, DateTimeKind.Local).AddTicks(1738), new DateTime(2025, 1, 15, 8, 24, 38, 277, DateTimeKind.Local).AddTicks(1737) });

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON004",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 1, 25, 8, 24, 38, 277, DateTimeKind.Local).AddTicks(1759), new DateTime(2025, 1, 15, 8, 24, 38, 277, DateTimeKind.Local).AddTicks(1757) });

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON005",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 2, 9, 8, 24, 38, 277, DateTimeKind.Local).AddTicks(1764), new DateTime(2025, 1, 15, 8, 24, 38, 277, DateTimeKind.Local).AddTicks(1762) });
        }
    }
}
