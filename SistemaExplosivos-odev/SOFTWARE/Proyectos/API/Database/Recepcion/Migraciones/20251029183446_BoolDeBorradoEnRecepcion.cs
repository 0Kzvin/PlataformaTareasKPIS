using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Database.Recepcion.Migraciones
{
    /// <inheritdoc />
    public partial class BoolDeBorradoEnRecepcion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Borrado",
                table: "Recepciones",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Borrado",
                table: "Recepciones");
        }
    }
}
