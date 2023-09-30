using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nicknames.Server.Migrations;

/// <inheritdoc />
public partial class JWTToken : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "Platform",
            table: "Users",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<string>(
            name: "Token",
            table: "Users",
            type: "longtext",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Platform",
            table: "Users");

        migrationBuilder.DropColumn(
            name: "Token",
            table: "Users");
    }
}
