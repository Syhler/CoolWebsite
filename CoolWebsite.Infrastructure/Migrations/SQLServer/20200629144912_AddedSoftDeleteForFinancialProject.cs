using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoolWebsite.Infrastructure.Migrations.SQLServer
{
    public partial class AddedSoftDeleteForFinancialProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Receipts",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "ReceiptItems",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "FinancialProjects",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Deleted",
                table: "FinancialProjects",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Receipts_CreatedBy",
                table: "Receipts",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptItems_CreatedBy",
                table: "ReceiptItems",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialProjects_CreatedBy",
                table: "FinancialProjects",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialProjects_AspNetUsers_CreatedBy",
                table: "FinancialProjects",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiptItems_AspNetUsers_CreatedBy",
                table: "ReceiptItems",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Receipts_AspNetUsers_CreatedBy",
                table: "Receipts",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinancialProjects_AspNetUsers_CreatedBy",
                table: "FinancialProjects");

            migrationBuilder.DropForeignKey(
                name: "FK_ReceiptItems_AspNetUsers_CreatedBy",
                table: "ReceiptItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Receipts_AspNetUsers_CreatedBy",
                table: "Receipts");

            migrationBuilder.DropIndex(
                name: "IX_Receipts_CreatedBy",
                table: "Receipts");

            migrationBuilder.DropIndex(
                name: "IX_ReceiptItems_CreatedBy",
                table: "ReceiptItems");

            migrationBuilder.DropIndex(
                name: "IX_FinancialProjects_CreatedBy",
                table: "FinancialProjects");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "FinancialProjects");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Receipts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "ReceiptItems",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "FinancialProjects",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
