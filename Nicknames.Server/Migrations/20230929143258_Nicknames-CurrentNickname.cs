using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nicknames.Server.Migrations
{
    /// <inheritdoc />
    public partial class NicknamesCurrentNickname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentNicknameId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_CurrentNicknameId",
                table: "Users",
                column: "CurrentNicknameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Nicknames_CurrentNicknameId",
                table: "Users",
                column: "CurrentNicknameId",
                principalTable: "Nicknames",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Nicknames_CurrentNicknameId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CurrentNicknameId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CurrentNicknameId",
                table: "Users");
        }
    }
}
