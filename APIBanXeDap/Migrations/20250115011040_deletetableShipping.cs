using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIBanXeDap.Migrations
{
    /// <inheritdoc />
    public partial class deletetableShipping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vanchuyen");

            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vanchuyen"
            );
        }
    }
}
