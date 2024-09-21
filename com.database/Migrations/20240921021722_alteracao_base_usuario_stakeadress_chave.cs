using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace com.database.Migrations
{
    /// <inheritdoc />
    public partial class alteracao_base_usuario_stakeadress_chave : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "stake_address",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Users_stake_address",
                table: "Users",
                column: "stake_address",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_stake_address",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "stake_address",
                table: "Users");
        }
    }
}
