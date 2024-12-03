using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace assignment2herhealth.Data.Migrations
{
    /// <inheritdoc />
    public partial class periodHealth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PeriodEntry",
                columns: table => new
                {
                    PeriodEntryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    PeriodStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NextPredictedPeriodDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Photo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeriodEntry", x => x.PeriodEntryId);
                });

            migrationBuilder.CreateTable(
                name: "HealthSuggestion",
                columns: table => new
                {
                    HealthSuggestionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PeriodEntryId = table.Column<int>(type: "int", nullable: false),
                    WaterIntake = table.Column<int>(type: "int", nullable: false),
                    HealthyFoods = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthSuggestion", x => x.HealthSuggestionId);
                    table.ForeignKey(
                        name: "FK_HealthSuggestion_PeriodEntry_PeriodEntryId",
                        column: x => x.PeriodEntryId,
                        principalTable: "PeriodEntry",
                        principalColumn: "PeriodEntryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HealthSuggestion_PeriodEntryId",
                table: "HealthSuggestion",
                column: "PeriodEntryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HealthSuggestion");

            migrationBuilder.DropTable(
                name: "PeriodEntry");
        }
    }
}
