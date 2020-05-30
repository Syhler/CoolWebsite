using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoolWebsite.Infrastructure.Migrations.SQLServer
{
    public partial class addedAuddbleToReceiptItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "ReceiptItems",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ReceiptItems",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "ReceiptItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "ReceiptItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "ReceiptItems");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ReceiptItems");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "ReceiptItems");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "ReceiptItems");
        }
    }
}
