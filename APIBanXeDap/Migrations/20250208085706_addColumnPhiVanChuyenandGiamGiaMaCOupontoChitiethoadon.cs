using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIBanXeDap.Migrations
{
    /// <inheritdoc />
    public partial class addColumnPhiVanChuyenandGiamGiaMaCOupontoChitiethoadon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "GiamGiaMaCoupon",
                table: "CHITIETHOADON",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "PhiVanChuyen",
                table: "CHITIETHOADON",
                type: "real",
                nullable: false,
                defaultValue: 0f);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GiamGiaMaCoupon",
                table: "CHITIETHOADON");

            migrationBuilder.DropColumn(
                name: "PhiVanChuyen",
                table: "CHITIETHOADON");

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON001",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 2, 24, 21, 8, 48, 509, DateTimeKind.Local).AddTicks(1169), new DateTime(2025, 1, 25, 21, 8, 48, 509, DateTimeKind.Local).AddTicks(1151) });

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON002",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 2, 9, 21, 8, 48, 509, DateTimeKind.Local).AddTicks(1179), new DateTime(2025, 1, 25, 21, 8, 48, 509, DateTimeKind.Local).AddTicks(1177) });

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON003",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 2, 14, 21, 8, 48, 509, DateTimeKind.Local).AddTicks(1182), new DateTime(2025, 1, 25, 21, 8, 48, 509, DateTimeKind.Local).AddTicks(1181) });

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON004",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 2, 4, 21, 8, 48, 509, DateTimeKind.Local).AddTicks(1186), new DateTime(2025, 1, 25, 21, 8, 48, 509, DateTimeKind.Local).AddTicks(1185) });

            migrationBuilder.UpdateData(
                table: "MaCoupons",
                keyColumn: "Code",
                keyValue: "COUPON005",
                columns: new[] { "NgayHetHan", "NgayTao" },
                values: new object[] { new DateTime(2025, 2, 19, 21, 8, 48, 509, DateTimeKind.Local).AddTicks(1190), new DateTime(2025, 1, 25, 21, 8, 48, 509, DateTimeKind.Local).AddTicks(1188) });
        }
    }
}
