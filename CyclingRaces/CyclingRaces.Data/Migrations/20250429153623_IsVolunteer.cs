using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CyclingRaces.Data.Migrations
{
    /// <inheritdoc />
    public partial class IsVolunteer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVolunteer",
                table: "Participations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RaceId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RaceId",
                table: "AspNetUsers",
                column: "RaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Races_RaceId",
                table: "AspNetUsers",
                column: "RaceId",
                principalTable: "Races",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Races_RaceId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_RaceId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsVolunteer",
                table: "Participations");

            migrationBuilder.DropColumn(
                name: "RaceId",
                table: "AspNetUsers");
        }
    }
}
