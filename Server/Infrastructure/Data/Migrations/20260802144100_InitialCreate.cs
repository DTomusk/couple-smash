using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pairings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstMemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    SecondMemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsExempted = table.Column<bool>(type: "boolean", nullable: false),
                    CompatibilityRating = table.Column<decimal>(type: "numeric", nullable: false),
                    NumberOfRatings = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pairings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pairings_Members_FirstMemberId",
                        column: x => x.FirstMemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pairings_Members_SecondMemberId",
                        column: x => x.SecondMemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pairings_FirstMemberId",
                table: "Pairings",
                column: "FirstMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Pairings_SecondMemberId",
                table: "Pairings",
                column: "SecondMemberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pairings");

            migrationBuilder.DropTable(
                name: "Members");
        }
    }
}
