using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace reservationAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedApartmentModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApartmentId",
                table: "Users",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Apartments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rooms = table.Column<int>(type: "int", nullable: false),
                    PricePerDay = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxGuests = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntranceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LeavingDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apartments", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_ApartmentId",
                table: "Users",
                column: "ApartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Apartments_ApartmentId",
                table: "Users",
                column: "ApartmentId",
                principalTable: "Apartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Apartments_ApartmentId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Apartments");

            migrationBuilder.DropIndex(
                name: "IX_Users_ApartmentId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ApartmentId",
                table: "Users");
        }
    }
}
