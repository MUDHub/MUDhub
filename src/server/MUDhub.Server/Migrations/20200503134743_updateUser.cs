using Microsoft.EntityFrameworkCore.Migrations;

namespace MUDhub.Server.Migrations
{
    public partial class updateUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_MudJoinRequests_UserId",
                table: "MudJoinRequests",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MudJoinRequests_Users_UserId",
                table: "MudJoinRequests",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MudJoinRequests_Users_UserId",
                table: "MudJoinRequests");

            migrationBuilder.DropIndex(
                name: "IX_MudJoinRequests_UserId",
                table: "MudJoinRequests");
        }
    }
}
