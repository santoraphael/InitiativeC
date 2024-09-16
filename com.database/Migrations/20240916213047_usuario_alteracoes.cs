using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace com.database.Migrations
{
    /// <inheritdoc />
    public partial class usuario_alteracoes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "walletaddress",
                table: "Users",
                newName: "wallet_address");

            migrationBuilder.AddColumn<string>(
                name: "confirmation_code_alphanumber",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "confirmation_code_number",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "confirmed",
                table: "Users",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "expiration_date_invitations",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "invitations_available",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "invite_code",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "invited_by",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "phone_number",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "Users",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "confirmation_code_alphanumber",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "confirmation_code_number",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "confirmed",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "email",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "expiration_date_invitations",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "invitations_available",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "invite_code",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "invited_by",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "name",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "phone_number",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "status",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "wallet_address",
                table: "Users",
                newName: "walletaddress");
        }
    }
}
