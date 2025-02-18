using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIBanXeDap.Migrations
{
    /// <inheritdoc />
    public partial class addnullabletoKhachHangTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "KHACHHANG",
                type: "char(40)",
                unicode: false,
                fixedLength: true,
                maxLength: 40,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(40)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 40);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "KHACHHANG",
                type: "char(40)",
                unicode: false,
                fixedLength: true,
                maxLength: 40,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "char(40)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 40,
                oldNullable: true);

        }
    }
}
