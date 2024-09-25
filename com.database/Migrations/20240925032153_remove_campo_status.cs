using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace com.database.Migrations
{
    /// <inheritdoc />
    public partial class remove_campo_status : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TokenBounties_Users_id_usuario",
                table: "TokenBounties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TokenPool",
                table: "TokenPool");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TokenBounties",
                table: "TokenBounties");

            migrationBuilder.DropColumn(
                name: "status",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "users");

            migrationBuilder.RenameTable(
                name: "TokenPool",
                newName: "tokenpool");

            migrationBuilder.RenameTable(
                name: "TokenBounties",
                newName: "tokenbounties");

            migrationBuilder.RenameIndex(
                name: "IX_Users_stake_address",
                table: "users",
                newName: "IX_users_stake_address");

            migrationBuilder.RenameIndex(
                name: "IX_TokenBounties_id_usuario",
                table: "tokenbounties",
                newName: "IX_tokenbounties_id_usuario");

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                table: "users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tokenpool",
                table: "tokenpool",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tokenbounties",
                table: "tokenbounties",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_tokenbounties_users_id_usuario",
                table: "tokenbounties",
                column: "id_usuario",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tokenbounties_users_id_usuario",
                table: "tokenbounties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tokenpool",
                table: "tokenpool");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tokenbounties",
                table: "tokenbounties");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "tokenpool",
                newName: "TokenPool");

            migrationBuilder.RenameTable(
                name: "tokenbounties",
                newName: "TokenBounties");

            migrationBuilder.RenameIndex(
                name: "IX_users_stake_address",
                table: "Users",
                newName: "IX_Users_stake_address");

            migrationBuilder.RenameIndex(
                name: "IX_tokenbounties_id_usuario",
                table: "TokenBounties",
                newName: "IX_TokenBounties_id_usuario");

            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TokenPool",
                table: "TokenPool",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TokenBounties",
                table: "TokenBounties",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_TokenBounties_Users_id_usuario",
                table: "TokenBounties",
                column: "id_usuario",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
