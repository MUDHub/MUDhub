using Microsoft.EntityFrameworkCore.Migrations;

namespace MUDhub.Server.Migrations
{
    public partial class AddRooms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GameId",
                table: "Races",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DefaultRoomId",
                table: "MudGames",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GameId",
                table: "Classes",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    GameId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Areas_MudGames_GameId",
                        column: x => x.GameId,
                        principalTable: "MudGames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    AreaId = table.Column<string>(nullable: false),
                    ImageKey = table.Column<string>(nullable: false),
                    X = table.Column<int>(nullable: false),
                    Y = table.Column<int>(nullable: false),
                    GameId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoomConnections",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Room1Id = table.Column<string>(nullable: false),
                    Room2Id = table.Column<string>(nullable: false),
                    LockType = table.Column<int>(nullable: false),
                    LockDescription = table.Column<string>(nullable: false),
                    LockAssociatedId = table.Column<string>(nullable: false),
                    RoomId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomConnections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomConnections_Rooms_Room1Id",
                        column: x => x.Room1Id,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomConnections_Rooms_Room2Id",
                        column: x => x.Room2Id,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomConnections_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoomInteractions",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    ExecutionMessage = table.Column<string>(nullable: false),
                    RoomId = table.Column<string>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    RelatedId = table.Column<string>(nullable: false),
                    GameId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomInteractions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomInteractions_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Races_GameId",
                table: "Races",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_MudGames_DefaultRoomId",
                table: "MudGames",
                column: "DefaultRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_GameId",
                table: "Classes",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Areas_GameId",
                table: "Areas",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomConnections_Room1Id",
                table: "RoomConnections",
                column: "Room1Id");

            migrationBuilder.CreateIndex(
                name: "IX_RoomConnections_Room2Id",
                table: "RoomConnections",
                column: "Room2Id");

            migrationBuilder.CreateIndex(
                name: "IX_RoomConnections_RoomId",
                table: "RoomConnections",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomInteractions_RoomId",
                table: "RoomInteractions",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_AreaId",
                table: "Rooms",
                column: "AreaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_MudGames_GameId",
                table: "Classes",
                column: "GameId",
                principalTable: "MudGames",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MudGames_Rooms_DefaultRoomId",
                table: "MudGames",
                column: "DefaultRoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Races_MudGames_GameId",
                table: "Races",
                column: "GameId",
                principalTable: "MudGames",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_MudGames_GameId",
                table: "Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_MudGames_Rooms_DefaultRoomId",
                table: "MudGames");

            migrationBuilder.DropForeignKey(
                name: "FK_Races_MudGames_GameId",
                table: "Races");

            migrationBuilder.DropTable(
                name: "RoomConnections");

            migrationBuilder.DropTable(
                name: "RoomInteractions");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Areas");

            migrationBuilder.DropIndex(
                name: "IX_Races_GameId",
                table: "Races");

            migrationBuilder.DropIndex(
                name: "IX_MudGames_DefaultRoomId",
                table: "MudGames");

            migrationBuilder.DropIndex(
                name: "IX_Classes_GameId",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "Races");

            migrationBuilder.DropColumn(
                name: "DefaultRoomId",
                table: "MudGames");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "Classes");
        }
    }
}
