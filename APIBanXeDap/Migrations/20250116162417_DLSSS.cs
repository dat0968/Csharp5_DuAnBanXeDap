using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIBanXeDap.Migrations
{
    /// <inheritdoc />
    public partial class DLSSS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON001",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 2, 15, 23, 24, 16, 697, DateTimeKind.Local).AddTicks(7534), new DateTime(2025, 1, 16, 23, 24, 16, 697, DateTimeKind.Local).AddTicks(7509) });

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON002",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 1, 31, 23, 24, 16, 697, DateTimeKind.Local).AddTicks(7548), new DateTime(2025, 1, 16, 23, 24, 16, 697, DateTimeKind.Local).AddTicks(7546) });

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON003",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 2, 5, 23, 24, 16, 697, DateTimeKind.Local).AddTicks(7556), new DateTime(2025, 1, 16, 23, 24, 16, 697, DateTimeKind.Local).AddTicks(7555) });

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON004",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 1, 26, 23, 24, 16, 697, DateTimeKind.Local).AddTicks(7570), new DateTime(2025, 1, 16, 23, 24, 16, 697, DateTimeKind.Local).AddTicks(7569) });

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON005",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 2, 10, 23, 24, 16, 697, DateTimeKind.Local).AddTicks(7574), new DateTime(2025, 1, 16, 23, 24, 16, 697, DateTimeKind.Local).AddTicks(7573) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON001",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 2, 15, 23, 22, 55, 63, DateTimeKind.Local).AddTicks(5295), new DateTime(2025, 1, 16, 23, 22, 55, 63, DateTimeKind.Local).AddTicks(5266) });

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON002",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 1, 31, 23, 22, 55, 63, DateTimeKind.Local).AddTicks(5312), new DateTime(2025, 1, 16, 23, 22, 55, 63, DateTimeKind.Local).AddTicks(5309) });

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON003",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 2, 5, 23, 22, 55, 63, DateTimeKind.Local).AddTicks(5317), new DateTime(2025, 1, 16, 23, 22, 55, 63, DateTimeKind.Local).AddTicks(5316) });

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON004",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 1, 26, 23, 22, 55, 63, DateTimeKind.Local).AddTicks(5323), new DateTime(2025, 1, 16, 23, 22, 55, 63, DateTimeKind.Local).AddTicks(5322) });

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON005",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 2, 10, 23, 22, 55, 63, DateTimeKind.Local).AddTicks(5328), new DateTime(2025, 1, 16, 23, 22, 55, 63, DateTimeKind.Local).AddTicks(5327) });
        }
    }
}
