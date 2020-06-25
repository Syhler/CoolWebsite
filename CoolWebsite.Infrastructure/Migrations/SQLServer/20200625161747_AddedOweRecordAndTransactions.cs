using Microsoft.EntityFrameworkCore.Migrations;

namespace CoolWebsite.Infrastructure.Migrations.SQLServer
{
    public partial class AddedOweRecordAndTransactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OweRecords",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    FinancialProjectId = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    OwedUserId = table.Column<string>(nullable: false),
                    Amount = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OweRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OweRecords_FinancialProjects_FinancialProjectId",
                        column: x => x.FinancialProjectId,
                        principalTable: "FinancialProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OweRecords_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OweRecords_AspNetUsers_OwedUserId",
                        column: x => x.OwedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    FromUserId = table.Column<string>(nullable: false),
                    ToUserId = table.Column<string>(nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    TransactionType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_AspNetUsers_FromUserId",
                        column: x => x.FromUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transactions_AspNetUsers_ToUserId",
                        column: x => x.ToUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OweRecords_FinancialProjectId",
                table: "OweRecords",
                column: "FinancialProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_OweRecords_OwedUserId",
                table: "OweRecords",
                column: "OwedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_FromUserId",
                table: "Transactions",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ToUserId",
                table: "Transactions",
                column: "ToUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OweRecords");

            migrationBuilder.DropTable(
                name: "Transactions");
        }
    }
}
