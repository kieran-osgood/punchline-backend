using Microsoft.EntityFrameworkCore.Migrations;

namespace GraphQL.Migrations
{
    public partial class AddSeparateRatingColumnsForJokes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Score",
                table: "Jokes",
                newName: "SkipRating");

            migrationBuilder.AddColumn<int>(
                name: "NegativeRating",
                table: "Jokes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PositiveRating",
                table: "Jokes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Categories",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NegativeRating",
                table: "Jokes");

            migrationBuilder.DropColumn(
                name: "PositiveRating",
                table: "Jokes");

            migrationBuilder.RenameColumn(
                name: "SkipRating",
                table: "Jokes",
                newName: "Score");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Categories",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
