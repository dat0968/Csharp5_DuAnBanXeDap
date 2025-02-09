using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIBanXeDap.Migrations
{
    /// <inheritdoc />
    public partial class addMaCouponColumn_MaCouponTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
            name: "DaSuDung",
            table: "MaCoupons",
            type: "bit",
            nullable: false,
            defaultValue: false); // Cột mới mặc định là false
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
            name: "DaSuDung",
            table: "MaCoupons");
        }
    }
}
