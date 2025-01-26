using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IndividualsRegistry.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Individuals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "Int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "NVarChar", nullable: false),
                    Surname = table.Column<string>(type: "NVarChar", nullable: false),
                    Gender = table.Column<string>(type: "NVarChar", nullable: false),
                    PersonalId = table.Column<string>(type: "NVarChar", nullable: false),
                    BirthDate = table.Column<DateOnly>(type: "Date", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: true),
                    Picture = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Individuals", x => x.Id);
                    table.CheckConstraint("CK_Gender", "Gender IN ('Male', 'Female')");
                    table.CheckConstraint("CK_Name_Characters", "Name NOT LIKE '%[^a-zA-Zა-ჰ]%'");
                    table.CheckConstraint("CK_Name_Length", "LEN(Name) >= 2 AND LEN(Name) <= 50");
                    table.CheckConstraint("CK_PersonalId", "PersonalId LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'");
                    table.CheckConstraint("CK_Surname_Characters", "Surname NOT LIKE '%[^a-zA-Zა-ჰ]%'");
                    table.CheckConstraint("CK_Surname_Length", "LEN(Surname) >= 2 AND LEN(Surname) <= 50");
                    table.ForeignKey(
                        name: "FK_Individuals_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "PhoneNumbers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Individualid = table.Column<int>(type: "Int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhoneNumbers", x => x.Id);
                    table.CheckConstraint("CK_PhoneNumberType", "Type IN ('Mobile', 'Office', 'Home')");
                    table.ForeignKey(
                        name: "FK_PhoneNumbers_Individuals_Individualid",
                        column: x => x.Individualid,
                        principalTable: "Individuals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Relations",
                columns: table => new
                {
                    IndividualId = table.Column<int>(type: "Int", nullable: false),
                    RelatedIndividualId = table.Column<int>(type: "Int", nullable: false),
                    RelationType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Relations", x => new { x.IndividualId, x.RelatedIndividualId });
                    table.ForeignKey(
                        name: "FK_Relations_Individuals_IndividualId",
                        column: x => x.IndividualId,
                        principalTable: "Individuals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Relations_Individuals_RelatedIndividualId",
                        column: x => x.RelatedIndividualId,
                        principalTable: "Individuals",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Individuals_CityId",
                table: "Individuals",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_PhoneNumbers_Individualid",
                table: "PhoneNumbers",
                column: "Individualid");

            migrationBuilder.CreateIndex(
                name: "IX_Relations_RelatedIndividualId",
                table: "Relations",
                column: "RelatedIndividualId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PhoneNumbers");

            migrationBuilder.DropTable(
                name: "Relations");

            migrationBuilder.DropTable(
                name: "Individuals");

            migrationBuilder.DropTable(
                name: "Cities");
        }
    }
}
