using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MahlukHidup.Backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModelSectorRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NitrogenLevel",
                table: "SectorSoils",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrganicLevel",
                table: "SectorSoils",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DiseasePhotos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AgriculturalSectorId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CompanyId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PhotoUrl = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FileName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CapturedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiseasePhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiseasePhotos_AgriculturalSectors_AgriculturalSectorId",
                        column: x => x.AgriculturalSectorId,
                        principalTable: "AgriculturalSectors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DiseasePhotos_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SectorPest",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AgriculturalSectorId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    WerengCount = table.Column<int>(type: "int", nullable: false),
                    UlatCount = table.Column<int>(type: "int", nullable: false),
                    KutuCount = table.Column<int>(type: "int", nullable: false),
                    BelalangCount = table.Column<int>(type: "int", nullable: false),
                    DateReported = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectorPest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SectorPest_AgriculturalSectors_AgriculturalSectorId",
                        column: x => x.AgriculturalSectorId,
                        principalTable: "AgriculturalSectors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SectorWater",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AgriculturalSectorId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Usage = table.Column<int>(type: "int", nullable: false),
                    Rainfall = table.Column<int>(type: "int", nullable: false),
                    Efficiency = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectorWater", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SectorWater_AgriculturalSectors_AgriculturalSectorId",
                        column: x => x.AgriculturalSectorId,
                        principalTable: "AgriculturalSectors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SectorYield",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AgriculturalSectorId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Current = table.Column<double>(type: "double", nullable: false),
                    Target = table.Column<double>(type: "double", nullable: false),
                    Unit = table.Column<string>(type: "longtext", nullable: false)
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
                    table.PrimaryKey("PK_SectorYield", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SectorYield_AgriculturalSectors_AgriculturalSectorId",
                        column: x => x.AgriculturalSectorId,
                        principalTable: "AgriculturalSectors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_DiseasePhotos_AgriculturalSectorId",
                table: "DiseasePhotos",
                column: "AgriculturalSectorId");

            migrationBuilder.CreateIndex(
                name: "IX_DiseasePhotos_CompanyId",
                table: "DiseasePhotos",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_SectorPest_AgriculturalSectorId",
                table: "SectorPest",
                column: "AgriculturalSectorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SectorWater_AgriculturalSectorId",
                table: "SectorWater",
                column: "AgriculturalSectorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SectorYield_AgriculturalSectorId",
                table: "SectorYield",
                column: "AgriculturalSectorId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiseasePhotos");

            migrationBuilder.DropTable(
                name: "SectorPest");

            migrationBuilder.DropTable(
                name: "SectorWater");

            migrationBuilder.DropTable(
                name: "SectorYield");

            migrationBuilder.DropColumn(
                name: "NitrogenLevel",
                table: "SectorSoils");

            migrationBuilder.DropColumn(
                name: "OrganicLevel",
                table: "SectorSoils");
        }
    }
}
