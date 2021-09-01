using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace GraphQL.Migrations
{
    public partial class AddIdPrimaryKeyToUserJokeHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserJokeHistory",
                table: "UserJokeHistory");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserJokeHistory",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserJokeHistory",
                table: "UserJokeHistory",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserJokeHistory",
                table: "UserJokeHistory");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserJokeHistory");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserJokeHistory",
                table: "UserJokeHistory",
                columns: new[] { "JokeId", "UserId" });
        }
    }
}
