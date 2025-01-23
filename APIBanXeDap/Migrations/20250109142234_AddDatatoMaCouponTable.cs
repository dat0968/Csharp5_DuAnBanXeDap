using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace APIBanXeDap.Migrations
{
    /// <inheritdoc />
    public partial class AddDatatoMaCouponTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "MaCoupons",
                columns: new[] { "Code", "MinimumOrderAmount", "NgayHetHan", "PhanTramGiam", "SoTienGiam", "TrangThai" },
                values: new object[,]
                {
                    { "COUPON001", 200000m, new DateTime(2025, 2, 8, 21, 22, 32, 804, DateTimeKind.Local).AddTicks(8642), null, 50000m, true },
                    { "COUPON002", 300000m, new DateTime(2025, 1, 24, 21, 22, 32, 804, DateTimeKind.Local).AddTicks(8670), 10f, null, true },
                    { "COUPON003", 500000m, new DateTime(2025, 1, 29, 21, 22, 32, 804, DateTimeKind.Local).AddTicks(8674), null, 100000m, false },
                    { "COUPON004", 400000m, new DateTime(2025, 1, 19, 21, 22, 32, 804, DateTimeKind.Local).AddTicks(8679), 20f, null, true },
                    { "COUPON005", 600000m, new DateTime(2025, 2, 3, 21, 22, 32, 804, DateTimeKind.Local).AddTicks(8684), null, 150000m, true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON001");

            migrationBuilder.DeleteData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON002");

            migrationBuilder.DeleteData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON003");

            migrationBuilder.DeleteData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON004");

            migrationBuilder.DeleteData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON005");
        }
    }
}
