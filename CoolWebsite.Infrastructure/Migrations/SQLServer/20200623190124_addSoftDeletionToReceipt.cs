using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoolWebsite.Infrastructure.Migrations.SQLServer
{
    public partial class addSoftDeletionToReceipt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Deleted",
                table: "Receipts",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Receipts");
        }
    }
}
