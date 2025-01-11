using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIBanXeDap.Migrations
{
    /// <inheritdoc />
    public partial class createColumnNgayTaoForMaCouponTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "NgayTao",
                table: "MaCoupons",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON001",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 2, 10, 0, 9, 47, 546, DateTimeKind.Local).AddTicks(5081), new DateTime(2025, 1, 11, 0, 9, 47, 546, DateTimeKind.Local).AddTicks(5066) });

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON002",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 1, 26, 0, 9, 47, 546, DateTimeKind.Local).AddTicks(5095), new DateTime(2025, 1, 11, 0, 9, 47, 546, DateTimeKind.Local).AddTicks(5093) });

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON003",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 1, 31, 0, 9, 47, 546, DateTimeKind.Local).AddTicks(5115), new DateTime(2025, 1, 11, 0, 9, 47, 546, DateTimeKind.Local).AddTicks(5114) });

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON004",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 1, 21, 0, 9, 47, 546, DateTimeKind.Local).AddTicks(5118), new DateTime(2025, 1, 11, 0, 9, 47, 546, DateTimeKind.Local).AddTicks(5117) });

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON005",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 2, 5, 0, 9, 47, 546, DateTimeKind.Local).AddTicks(5121), new DateTime(2025, 1, 11, 0, 9, 47, 546, DateTimeKind.Local).AddTicks(5120) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NgayTao",
                table: "MaCoupons");

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON001",
                column: "NgayHetHan",
                value: new DateTime(2025, 2, 8, 21, 22, 32, 804, DateTimeKind.Local).AddTicks(8642));

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON002",
                column: "NgayHetHan",
                value: new DateTime(2025, 1, 24, 21, 22, 32, 804, DateTimeKind.Local).AddTicks(8670));

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON003",
                column: "NgayHetHan",
                value: new DateTime(2025, 1, 29, 21, 22, 32, 804, DateTimeKind.Local).AddTicks(8674));

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON004",
                column: "NgayHetHan",
                value: new DateTime(2025, 1, 19, 21, 22, 32, 804, DateTimeKind.Local).AddTicks(8679));

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON005",
                column: "NgayHetHan",
                value: new DateTime(2025, 2, 3, 21, 22, 32, 804, DateTimeKind.Local).AddTicks(8684));
        }
    }
}
