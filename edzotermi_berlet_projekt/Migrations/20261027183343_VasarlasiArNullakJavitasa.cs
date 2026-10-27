using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace edzotermi_berlet_projekt.Migrations
{
    /// <inheritdoc />
    public partial class VasarlasiArNullakJavitasa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        UPDATE Berletek
        SET VasarlasiAr = (
            SELECT Ar
            FROM BerletTipusok
            WHERE BerletTipusok.Id = Berletek.BerletTipusId
        )
        WHERE CAST(VasarlasiAr AS REAL) = 0;
    ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
