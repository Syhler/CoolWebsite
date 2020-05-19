using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoolWebsite.Infrastructure.Migrations.SQLServer
{
    public partial class UpdatedFinancialDomainEntites : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BoughtAt",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "Receipts");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateVisited",
                table: "Receipts",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Receipts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Receipts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "FinancialProjects",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ReceiptItems",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Count = table.Column<int>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    ItemGroup = table.Column<int>(nullable: false),
                    ReceiptId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceiptItems_Receipts_ReceiptId",
                        column: x => x.ReceiptId,
                        principalTable: "Receipts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserReceiptItems",
                columns: table => new
                {
                    ReceiptItemId = table.Column<string>(nullable: false),
                    ApplicationUserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserReceiptItems", x => new { x.ApplicationUserId, x.ReceiptItemId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserReceiptItems_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserReceiptItems_ReceiptItems_ReceiptItemId",
                        column: x => x.ReceiptItemId,
                        principalTable: "ReceiptItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserReceiptItems_ReceiptItemId",
                table: "ApplicationUserReceiptItems",
                column: "ReceiptItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptItems_ReceiptId",
                table: "ReceiptItems",
                column: "ReceiptId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserReceiptItems");

            migrationBuilder.DropTable(
                name: "ReceiptItems");

            migrationBuilder.DropColumn(
                name: "DateVisited",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "FinancialProjects");

            migrationBuilder.AddColumn<DateTime>(
                name: "BoughtAt",
                table: "Receipts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Receipts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Total",
                table: "Receipts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
