using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecureUrlShortener.Migrations
{
    /// <inheritdoc />
    public partial class AddUserOwnership : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
        name: "UserId",
        table: "ShortUrls",
        type: "int",
        nullable: false,
        defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ShortUrls_UserId",
                table: "ShortUrls",
                column: "UserId");

migrationBuilder.AddForeignKey(
        name: "FK_ShortUrls_Users_UserId",
        table: "ShortUrls",
        column: "UserId",
        principalTable: "Users",
        principalColumn: "Id",
        onDelete: ReferentialAction.Restrict);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShortUrls");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
