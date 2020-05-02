using Microsoft.EntityFrameworkCore.Migrations;

namespace MUDhub.Server.Migrations
{
    public partial class AddMudGameReference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "MudGames",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_MudGames_OwnerId",
                table: "MudGames",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_MudGames_Users_OwnerId",
                table: "MudGames",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MudGames_Users_OwnerId",
                table: "MudGames");

            migrationBuilder.DropIndex(
                name: "IX_MudGames_OwnerId",
                table: "MudGames");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "MudGames");
        }
    }
}
