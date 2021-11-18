using Microsoft.EntityFrameworkCore.Migrations;

namespace AtaRK_Back.Migrations
{
    public partial class UpdateTechNessageAnswer_AddSystemAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SystemAdminId",
                table: "TechMessageAnswers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TechMessageAnswers_SystemAdminId",
                table: "TechMessageAnswers",
                column: "SystemAdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_TechMessageAnswers_SystemAdmins_SystemAdminId",
                table: "TechMessageAnswers",
                column: "SystemAdminId",
                principalTable: "SystemAdmins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TechMessageAnswers_SystemAdmins_SystemAdminId",
                table: "TechMessageAnswers");

            migrationBuilder.DropIndex(
                name: "IX_TechMessageAnswers_SystemAdminId",
                table: "TechMessageAnswers");

            migrationBuilder.DropColumn(
                name: "SystemAdminId",
                table: "TechMessageAnswers");
        }
    }
}
