using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Database.Almacenamiento.Migraciones
{
    /// <inheritdoc />
    public partial class IdUnicoProductoEnAltas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Producto",
                table: "DepositosAlmacenamiento",
                newName: "IdProducto");

            migrationBuilder.AddColumn<string>(
                name: "IdProducto",
                table: "EquiposAlmacenamiento",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdProducto",
                table: "EquiposAlmacenamiento");

            migrationBuilder.RenameColumn(
                name: "IdProducto",
                table: "DepositosAlmacenamiento",
                newName: "Producto");
        }
    }
}
