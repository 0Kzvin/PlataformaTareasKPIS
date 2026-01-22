using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Database.Accesorio.Migraciones
{
    /// <inheritdoc />
    public partial class EstatusAEstado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Estatus",
                table: "ProveedoresAccesorios",
                newName: "Estado");

            migrationBuilder.RenameColumn(
                name: "Estatus",
                table: "CategoriasAccesorios",
                newName: "Estado");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Estado",
                table: "ProveedoresAccesorios",
                newName: "Estatus");

            migrationBuilder.RenameColumn(
                name: "Estado",
                table: "CategoriasAccesorios",
                newName: "Estatus");
        }
    }
}
