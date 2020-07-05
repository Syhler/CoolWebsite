using Microsoft.EntityFrameworkCore.Migrations;

namespace CoolWebsite.Infrastructure.Migrations.SQLServer
{
    public partial class AddedForeginKEyToTransactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FinancialProjectId",
                table: "Transactions",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_FinancialProjectId",
                table: "Transactions",
                column: "FinancialProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_FinancialProjects_FinancialProjectId",
                table: "Transactions",
                column: "FinancialProjectId",
                principalTable: "FinancialProjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_FinancialProjects_FinancialProjectId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_FinancialProjectId",
                table: "Transactions");

            migrationBuilder.AlterColumn<string>(
                name: "FinancialProjectId",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
