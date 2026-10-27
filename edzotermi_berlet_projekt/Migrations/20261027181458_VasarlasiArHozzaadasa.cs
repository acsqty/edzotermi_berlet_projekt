using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace edzotermi_berlet_projekt.Migrations
{
    /// <inheritdoc />
    public partial class VasarlasiArHozzaadasa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "VasarlasiAr",
                table: "Berletek",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.Sql(@"
        UPDATE Berletek
        SET VasarlasiAr = (
            SELECT Ar
            FROM BerletTipusok
            WHERE BerletTipusok.Id = Berletek.BerletTipusId
        )
        WHERE VasarlasiAr = 0;
    ");
        }
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VasarlasiAr",
                table: "Berletek");
        }
    }
}
