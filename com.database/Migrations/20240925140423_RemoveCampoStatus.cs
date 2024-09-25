using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace com.database.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCampoStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TokenBounties_Users_id_usuario",
                table: "tokenbounties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TokenPool",
                table: "tokenpool");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TokenBounties",
                table: "tokenbounties");

            migrationBuilder.DropColumn(
                name: "status",
                table: "users");

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

            migrationBuilder.RenameIndex(
                name: "IX_users_stake_address",
                table: "users",
                newName: "IX_Users_stake_address");

            migrationBuilder.RenameIndex(
                name: "IX_tokenbounties_id_usuario",
                table: "tokenbounties",
                newName: "IX_TokenBounties_id_usuario");

            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TokenPool",
                table: "tokenpool",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TokenBounties",
                table: "tokenbounties",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_TokenBounties_Users_id_usuario",
                table: "tokenbounties",
                column: "id_usuario",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
