using Microsoft.EntityFrameworkCore.Migrations;

namespace CoolWebsite.Infrastructure.Migrations.SQLServer
{
    public partial class FixForWrongForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OweRecords_AspNetUsers_Id",
                table: "OweRecords");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "OweRecords",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_OweRecords_UserId",
                table: "OweRecords",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_OweRecords_AspNetUsers_UserId",
                table: "OweRecords",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OweRecords_AspNetUsers_UserId",
                table: "OweRecords");

            migrationBuilder.DropIndex(
                name: "IX_OweRecords_UserId",
                table: "OweRecords");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "OweRecords",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddForeignKey(
                name: "FK_OweRecords_AspNetUsers_Id",
                table: "OweRecords",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
