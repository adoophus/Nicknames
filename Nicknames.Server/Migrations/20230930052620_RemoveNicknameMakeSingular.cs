using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Nicknames.Server.Migrations
{
    /// <inheritdoc />
    public partial class RemoveNicknameMakeSingular : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Nicknames_CurrentNicknameId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Nicknames");

            migrationBuilder.DropIndex(
                name: "IX_Users_CurrentNicknameId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CurrentNicknameId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "GameId",
                table: "Users",
                type: "longtext",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nickname",
                table: "Users",
                type: "varchar(80)",
                maxLength: 80,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nickname",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "GameId",
                table: "Users",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AddColumn<int>(
                name: "CurrentNicknameId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Users",
                type: "longtext",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Nicknames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nicknames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Nicknames_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CurrentNicknameId",
                table: "Users",
                column: "CurrentNicknameId");

            migrationBuilder.CreateIndex(
                name: "IX_Nicknames_UserId",
                table: "Nicknames",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Nicknames_CurrentNicknameId",
                table: "Users",
                column: "CurrentNicknameId",
                principalTable: "Nicknames",
                principalColumn: "Id");
        }
    }
}
