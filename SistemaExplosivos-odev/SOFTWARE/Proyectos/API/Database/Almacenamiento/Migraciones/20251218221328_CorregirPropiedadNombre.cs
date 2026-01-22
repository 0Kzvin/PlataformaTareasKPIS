using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Database.Almacenamiento.Migraciones
{
    /// <inheritdoc />
    public partial class CorregirPropiedadNombre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FechaHoraInicio",
                table: "CargasDepositosAlmacenamientos",
                newName: "FechaHoraInicial");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FechaHoraInicial",
                table: "CargasDepositosAlmacenamientos",
                newName: "FechaHoraInicio");
        }
    }
}
