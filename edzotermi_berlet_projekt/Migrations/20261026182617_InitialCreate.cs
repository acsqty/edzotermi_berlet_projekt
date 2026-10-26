using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace edzotermi_berlet_projekt.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BerletTipusok",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nev = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    ErvenyessegNap = table.Column<int>(type: "INTEGER", nullable: false),
                    BelepesekSzama = table.Column<int>(type: "INTEGER", nullable: true),
                    Ar = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BerletTipusok", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tagok",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nev = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    Telefon = table.Column<string>(type: "TEXT", nullable: true),
                    RegisztracioDatum = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tagok", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Berletek",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TagId = table.Column<int>(type: "INTEGER", nullable: false),
                    BerletTipusId = table.Column<int>(type: "INTEGER", nullable: false),
                    Vasarlas = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Lejarat = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FelhasznaltBelepes = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Berletek", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Berletek_BerletTipusok_BerletTipusId",
                        column: x => x.BerletTipusId,
                        principalTable: "BerletTipusok",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Berletek_Tagok_TagId",
                        column: x => x.TagId,
                        principalTable: "Tagok",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Belepesek",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BerletId = table.Column<int>(type: "INTEGER", nullable: false),
                    Idopont = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Belepesek", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Belepesek_Berletek_BerletId",
                        column: x => x.BerletId,
                        principalTable: "Berletek",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Belepesek_BerletId",
                table: "Belepesek",
                column: "BerletId");

            migrationBuilder.CreateIndex(
                name: "IX_Berletek_BerletTipusId",
                table: "Berletek",
                column: "BerletTipusId");

            migrationBuilder.CreateIndex(
                name: "IX_Berletek_TagId",
                table: "Berletek",
                column: "TagId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Belepesek");

            migrationBuilder.DropTable(
                name: "Berletek");

            migrationBuilder.DropTable(
                name: "BerletTipusok");

            migrationBuilder.DropTable(
                name: "Tagok");
        }
    }
}
