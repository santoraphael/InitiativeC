using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace com.database.Migrations
{
    /// <inheritdoc />
    public partial class CAMPO_CULTURE_TABELA_USUARIO : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "currentCulture",
                table: "users",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "currentCulture",
                table: "users");
        }
    }
}
