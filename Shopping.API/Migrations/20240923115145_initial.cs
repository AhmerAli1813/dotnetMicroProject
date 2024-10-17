﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopping.API.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Name", "UpdateAt" },
                values: new object[] { 1, new DateTime(2024, 9, 23, 16, 51, 41, 663, DateTimeKind.Local).AddTicks(5085), "Laptop", null });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "Description", "ImageUrl", "Name", "Price", "UpdateAt" },
                values: new object[] { 2, 1, new DateTime(2024, 9, 23, 16, 51, 41, 663, DateTimeKind.Local).AddTicks(5291), "This is my laptop", "data:image/jpeg;base64,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", "Dell Laptop", 120.04m, null });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}