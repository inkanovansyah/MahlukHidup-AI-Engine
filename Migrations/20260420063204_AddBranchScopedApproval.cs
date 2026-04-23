using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MahlukHidup.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddBranchScopedApproval : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEmailVerified",
                table: "Users");

            migrationBuilder.AddColumn<Guid>(
                name: "BranchId",
                table: "Users",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "BranchId",
                table: "MaterialRequests",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<bool>(
                name: "IsMaster",
                table: "Branches",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Users_BranchId",
                table: "Users",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialRequests_BranchId",
                table: "MaterialRequests",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_MaterialRequests_Branches_BranchId",
                table: "MaterialRequests",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Branches_BranchId",
                table: "Users",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaterialRequests_Branches_BranchId",
                table: "MaterialRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Branches_BranchId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_BranchId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_MaterialRequests_BranchId",
                table: "MaterialRequests");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "MaterialRequests");

            migrationBuilder.DropColumn(
                name: "IsMaster",
                table: "Branches");

            migrationBuilder.AddColumn<bool>(
                name: "IsEmailVerified",
                table: "Users",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
