using Microsoft.EntityFrameworkCore.Migrations;

namespace CoolWebsite.Infrastructure.Migrations.SQLServer
{
    public partial class AddedDeletedBy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeletedByUserId",
                table: "Receipts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedByUserId",
                table: "FinancialProjects",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Receipts_DeletedByUserId",
                table: "Receipts",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialProjects_DeletedByUserId",
                table: "FinancialProjects",
                column: "DeletedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialProjects_AspNetUsers_DeletedByUserId",
                table: "FinancialProjects",
                column: "DeletedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Receipts_AspNetUsers_DeletedByUserId",
                table: "Receipts",
                column: "DeletedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinancialProjects_AspNetUsers_DeletedByUserId",
                table: "FinancialProjects");

            migrationBuilder.DropForeignKey(
                name: "FK_Receipts_AspNetUsers_DeletedByUserId",
                table: "Receipts");

            migrationBuilder.DropIndex(
                name: "IX_Receipts_DeletedByUserId",
                table: "Receipts");

            migrationBuilder.DropIndex(
                name: "IX_FinancialProjects_DeletedByUserId",
                table: "FinancialProjects");

            migrationBuilder.DropColumn(
                name: "DeletedByUserId",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "DeletedByUserId",
                table: "FinancialProjects");
        }
    }
}
