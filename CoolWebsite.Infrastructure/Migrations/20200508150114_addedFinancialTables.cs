using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoolWebsite.Infrastructure.Migrations
{
    public partial class addedFinancialTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FinancialProjects",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialProjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FinancialProjectApplicationUsers",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    FinancialProjectId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialProjectApplicationUsers", x => new { x.UserId, x.FinancialProjectId });
                    table.ForeignKey(
                        name: "FK_FinancialProjectApplicationUsers_FinancialProjects_Financial~",
                        column: x => x.FinancialProjectId,
                        principalTable: "FinancialProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FinancialProjectApplicationUsers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Receipts",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModified = table.Column<DateTime>(nullable: true),
                    Total = table.Column<double>(nullable: false),
                    FinancialProjectId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receipts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Receipts_FinancialProjects_FinancialProjectId",
                        column: x => x.FinancialProjectId,
                        principalTable: "FinancialProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IndividualReceipts",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModified = table.Column<DateTime>(nullable: true),
                    Total = table.Column<double>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    ReceiptId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndividualReceipts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IndividualReceipts_Receipts_ReceiptId",
                        column: x => x.ReceiptId,
                        principalTable: "Receipts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IndividualReceipts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinancialProjectApplicationUsers_FinancialProjectId",
                table: "FinancialProjectApplicationUsers",
                column: "FinancialProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_IndividualReceipts_ReceiptId",
                table: "IndividualReceipts",
                column: "ReceiptId");

            migrationBuilder.CreateIndex(
                name: "IX_IndividualReceipts_UserId",
                table: "IndividualReceipts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Receipts_FinancialProjectId",
                table: "Receipts",
                column: "FinancialProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinancialProjectApplicationUsers");

            migrationBuilder.DropTable(
                name: "IndividualReceipts");

            migrationBuilder.DropTable(
                name: "Receipts");

            migrationBuilder.DropTable(
                name: "FinancialProjects");
        }
    }
}
