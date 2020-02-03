using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoolWebsite.Infrastructure.Migrations
{
    public partial class AddedAudtiableToTestEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "TestEntities",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "TestEntities",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "TestEntities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "TestEntities",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "TestEntities");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "TestEntities");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "TestEntities");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "TestEntities");
        }
    }
}
