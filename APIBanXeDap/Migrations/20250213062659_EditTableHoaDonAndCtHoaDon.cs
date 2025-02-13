using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIBanXeDap.Migrations
{
    /// <inheritdoc />
    public partial class EditTableHoaDonAndCtHoaDon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GiamGiaMaCoupon",
                table: "CHITIETHOADON");

            migrationBuilder.DropColumn(
                name: "PhiVanChuyen",
                table: "CHITIETHOADON");

            migrationBuilder.AddColumn<float>(
                name: "GiamGiaMaCoupon",
                table: "HOADON",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "PhiVanChuyen",
                table: "HOADON",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "TienGoc",
                table: "HOADON",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "TongTien",
                table: "HOADON",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GiamGiaMaCoupon",
                table: "HOADON");

            migrationBuilder.DropColumn(
                name: "PhiVanChuyen",
                table: "HOADON");

            migrationBuilder.DropColumn(
                name: "TienGoc",
                table: "HOADON");

            migrationBuilder.DropColumn(
                name: "TongTien",
                table: "HOADON");

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

            
        }
    }
}
