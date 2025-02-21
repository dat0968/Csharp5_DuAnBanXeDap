using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIBanXeDap.Migrations
{
    /// <inheritdoc />
    public partial class add_LyDoHuy_TblHoaDon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LyDoHuy",
                table: "HOADON",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON001",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 3, 24, 9, 29, 1, 472, DateTimeKind.Local).AddTicks(8415), new DateTime(2025, 2, 22, 9, 29, 1, 472, DateTimeKind.Local).AddTicks(8398) });

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON002",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 3, 9, 9, 29, 1, 472, DateTimeKind.Local).AddTicks(8430), new DateTime(2025, 2, 22, 9, 29, 1, 472, DateTimeKind.Local).AddTicks(8428) });

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON003",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 3, 14, 9, 29, 1, 472, DateTimeKind.Local).AddTicks(8441), new DateTime(2025, 2, 22, 9, 29, 1, 472, DateTimeKind.Local).AddTicks(8438) });

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON004",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 3, 4, 9, 29, 1, 472, DateTimeKind.Local).AddTicks(8450), new DateTime(2025, 2, 22, 9, 29, 1, 472, DateTimeKind.Local).AddTicks(8448) });

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON005",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 3, 19, 9, 29, 1, 472, DateTimeKind.Local).AddTicks(8460), new DateTime(2025, 2, 22, 9, 29, 1, 472, DateTimeKind.Local).AddTicks(8458) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LyDoHuy",
                table: "HOADON");

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
