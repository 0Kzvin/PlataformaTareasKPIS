using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Database.Almacenamiento.Migraciones
{
    /// <inheritdoc />
    public partial class IdProductoEnMovimientoSuperSacos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Producto",
                table: "MovimientosSupersacosAlmacenamiento",
                newName: "IdProducto");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdProducto",
                table: "MovimientosSupersacosAlmacenamiento",
                newName: "Producto");
        }
    }
}
