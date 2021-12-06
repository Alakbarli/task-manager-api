using Microsoft.EntityFrameworkCore.Migrations;

namespace TaskManagerApi.Migrations
{
    public partial class addorgidtotask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                table: "Tasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_OrganizationId",
                table: "Tasks",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Organizations_OrganizationId",
                table: "Tasks",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Organizations_OrganizationId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_OrganizationId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Tasks");
        }
    }
}
