﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace MUDhub.Server.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MudGames",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    ImageKey = table.Column<string>(nullable: true),
                    IsPublic = table.Column<bool>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    AutoRestart = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MudGames", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Lastname = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: false),
                    Role = table.Column<int>(nullable: false),
                    PasswordResetKey = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MudJoinRequests",
                columns: table => new
                {
                    MudId = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    State = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MudJoinRequests", x => new { x.MudId, x.UserId });
                    table.ForeignKey(
                        name: "FK_MudJoinRequests_MudGames_MudId",
                        column: x => x.MudId,
                        principalTable: "MudGames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MudJoinRequests");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "MudGames");
        }
    }
}
