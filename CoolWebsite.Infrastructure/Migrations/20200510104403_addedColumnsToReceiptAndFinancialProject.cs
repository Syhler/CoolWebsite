using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoolWebsite.Infrastructure.Migrations
{
    public partial class addedColumnsToReceiptAndFinancialProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "FinancialProjects");

            migrationBuilder.AddColumn<DateTime>(
                name: "BoughtAt",
                table: "Receipts",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Receipts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "FinancialProjects",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BoughtAt",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "FinancialProjects");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "FinancialProjects",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
