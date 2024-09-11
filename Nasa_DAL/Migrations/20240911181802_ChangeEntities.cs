using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nasa_DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangeEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Meteorites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NameType = table.Column<int>(type: "int", nullable: false),
                    RecClass = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Mass = table.Column<double>(type: "float", nullable: false),
                    Fall = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Reclat = table.Column<double>(type: "float", nullable: false),
                    Reclong = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meteorites", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Geolocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Coordinates = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MeteoriteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Geolocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Geolocations_Meteorites_MeteoriteId",
                        column: x => x.MeteoriteId,
                        principalTable: "Meteorites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Geolocation_Type",
                table: "Geolocations",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_Geolocations_MeteoriteId",
                table: "Geolocations",
                column: "MeteoriteId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Meteorite_Name",
                table: "Meteorites",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Meteorite_RecClass",
                table: "Meteorites",
                column: "RecClass");

            migrationBuilder.CreateIndex(
                name: "IX_Meteorite_Year",
                table: "Meteorites",
                column: "Year");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Geolocations");

            migrationBuilder.DropTable(
                name: "Meteorites");
        }
    }
}
