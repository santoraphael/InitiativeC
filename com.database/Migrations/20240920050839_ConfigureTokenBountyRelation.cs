using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace com.database.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureTokenBountyRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TokenBounties_id_usuario",
                table: "TokenBounties",
                column: "id_usuario",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TokenBounties_Users_id_usuario",
                table: "TokenBounties",
                column: "id_usuario",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TokenBounties_Users_id_usuario",
                table: "TokenBounties");

            migrationBuilder.DropIndex(
                name: "IX_TokenBounties_id_usuario",
                table: "TokenBounties");
        }
    }
}
