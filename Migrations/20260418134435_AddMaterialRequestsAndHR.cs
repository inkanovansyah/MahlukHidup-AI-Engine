using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MahlukHidup.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddMaterialRequestsAndHR : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JobLevelId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JobPositionId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReportsToUserId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "JobLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Rank = table.Column<int>(type: "int", nullable: false),
                    CanApprove = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobLevels", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "JobPositions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobPositions", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MaterialRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RequestNumber = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    RequesterId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    Justification = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TotalEstimatedPrice = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialRequests_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialRequests_Users_RequesterId",
                        column: x => x.RequesterId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ApprovalRoutes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MaterialRequestId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ApproverUserId = table.Column<int>(type: "int", nullable: false),
                    StepOrder = table.Column<int>(type: "int", nullable: false),
                    IsCurrentStep = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ActionTakenAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalRoutes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApprovalRoutes_MaterialRequests_MaterialRequestId",
                        column: x => x.MaterialRequestId,
                        principalTable: "MaterialRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApprovalRoutes_Users_ApproverUserId",
                        column: x => x.ApproverUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MaterialRequestItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MaterialRequestId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ItemName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Quantity = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    UnitOfMeasure = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EstimatedUnitPrice = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialRequestItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialRequestItems_MaterialRequests_MaterialRequestId",
                        column: x => x.MaterialRequestId,
                        principalTable: "MaterialRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DepartmentId",
                table: "Users",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_JobLevelId",
                table: "Users",
                column: "JobLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_JobPositionId",
                table: "Users",
                column: "JobPositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ReportsToUserId",
                table: "Users",
                column: "ReportsToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalRoutes_ApproverUserId",
                table: "ApprovalRoutes",
                column: "ApproverUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalRoutes_MaterialRequestId",
                table: "ApprovalRoutes",
                column: "MaterialRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialRequestItems_MaterialRequestId",
                table: "MaterialRequestItems",
                column: "MaterialRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialRequests_DepartmentId",
                table: "MaterialRequests",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialRequests_RequesterId",
                table: "MaterialRequests",
                column: "RequesterId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialRequests_RequestNumber",
                table: "MaterialRequests",
                column: "RequestNumber",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Departments_DepartmentId",
                table: "Users",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_JobLevels_JobLevelId",
                table: "Users",
                column: "JobLevelId",
                principalTable: "JobLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_JobPositions_JobPositionId",
                table: "Users",
                column: "JobPositionId",
                principalTable: "JobPositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_ReportsToUserId",
                table: "Users",
                column: "ReportsToUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Departments_DepartmentId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_JobLevels_JobLevelId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_JobPositions_JobPositionId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_ReportsToUserId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "ApprovalRoutes");

            migrationBuilder.DropTable(
                name: "JobLevels");

            migrationBuilder.DropTable(
                name: "JobPositions");

            migrationBuilder.DropTable(
                name: "MaterialRequestItems");

            migrationBuilder.DropTable(
                name: "MaterialRequests");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Users_DepartmentId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_JobLevelId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_JobPositionId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ReportsToUserId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "JobLevelId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "JobPositionId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ReportsToUserId",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
