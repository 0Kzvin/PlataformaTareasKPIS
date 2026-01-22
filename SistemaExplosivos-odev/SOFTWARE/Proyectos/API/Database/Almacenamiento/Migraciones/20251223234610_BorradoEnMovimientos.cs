using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Database.Almacenamiento.Migraciones
{
    /// <inheritdoc />
    public partial class BorradoEnMovimientos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Borrado",
                table: "MovimientosSupersacosAlmacenamiento",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Borrado",
                table: "MovimientosSupersacosAlmacenamiento");
        }
    }
}
