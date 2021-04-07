using Microsoft.EntityFrameworkCore.Migrations;

namespace GraphQL.Migrations
{
    public partial class AddUserJokeHistoryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JokeUser");

            migrationBuilder.CreateTable(
                name: "UserJokeHistory",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    JokeId = table.Column<int>(type: "integer", nullable: false),
                    Bookmarked = table.Column<bool>(type: "boolean", nullable: false),
                    Rating = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserJokeHistory", x => new { x.JokeId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserJokeHistory_Jokes_JokeId",
                        column: x => x.JokeId,
                        principalTable: "Jokes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserJokeHistory_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserJokeHistory_JokeId_UserId",
                table: "UserJokeHistory",
                columns: new[] { "JokeId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserJokeHistory_UserId",
                table: "UserJokeHistory",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserJokeHistory");

            migrationBuilder.CreateTable(
                name: "JokeUser",
                columns: table => new
                {
                    JokesId = table.Column<int>(type: "integer", nullable: false),
                    UsersId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JokeUser", x => new { x.JokesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_JokeUser_Jokes_JokesId",
                        column: x => x.JokesId,
                        principalTable: "Jokes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JokeUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JokeUser_UsersId",
                table: "JokeUser",
                column: "UsersId");
        }
    }
}
