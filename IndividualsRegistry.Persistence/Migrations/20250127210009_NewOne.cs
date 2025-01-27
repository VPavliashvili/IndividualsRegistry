using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IndividualsRegistry.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class NewOne : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhoneNumbers_Individuals_Individualid",
                table: "PhoneNumbers");

            migrationBuilder.AlterColumn<int>(
                name: "Individualid",
                table: "PhoneNumbers",
                type: "Int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Int");

            migrationBuilder.AddForeignKey(
                name: "FK_PhoneNumbers_Individuals_Individualid",
                table: "PhoneNumbers",
                column: "Individualid",
                principalTable: "Individuals",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhoneNumbers_Individuals_Individualid",
                table: "PhoneNumbers");

            migrationBuilder.AlterColumn<int>(
                name: "Individualid",
                table: "PhoneNumbers",
                type: "Int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "Int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PhoneNumbers_Individuals_Individualid",
                table: "PhoneNumbers",
                column: "Individualid",
                principalTable: "Individuals",
                principalColumn: "Id");
        }
    }
}
