using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IndividualsRegistry.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Individuals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "Int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "NVarChar(10)", maxLength: 10, nullable: false),
                    Surname = table.Column<string>(type: "NVarChar(10)", maxLength: 10, nullable: false),
                    Gender = table.Column<string>(type: "NVarChar(10)", maxLength: 10, nullable: false),
                    PersonalId = table.Column<string>(type: "NVarChar(11)", fixedLength: true, maxLength: 11, nullable: false),
                    BirthDate = table.Column<DateOnly>(type: "Date", nullable: false),
                    CityId = table.Column<int>(type: "Int", nullable: true),
                    Picture = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Individuals", x => x.Id);
                    table.CheckConstraint("CK_Gender", "Gender IN ('Male', 'Female')");
                    table.CheckConstraint("CK_Individual_MinimumAge", "DATEDIFF(YEAR, BirthDate, GETDATE()) >= 18");
                    table.CheckConstraint("CK_Name_Characters", "Name NOT LIKE '%[^a-zA-Zა-ჰ]%'");
                    table.CheckConstraint("CK_Name_Length", "LEN(Name) >= 2 AND LEN(Name) <= 50");
                    table.CheckConstraint("CK_PersonalId", "PersonalId LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'");
                    table.CheckConstraint("CK_Surname_Characters", "Surname NOT LIKE '%[^a-zA-Zა-ჰ]%'");
                    table.CheckConstraint("CK_Surname_Length", "LEN(Surname) >= 2 AND LEN(Surname) <= 50");
                });

            migrationBuilder.CreateTable(
                name: "PhoneNumbers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
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
                        principalColumn: "Id");
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
                name: "UX_Individual_PersonalId",
                table: "Individuals",
                column: "PersonalId",
                unique: true);

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
        }
    }
}
