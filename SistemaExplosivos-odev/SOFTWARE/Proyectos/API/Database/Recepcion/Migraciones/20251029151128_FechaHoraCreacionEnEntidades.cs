using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Database.Recepcion.Migraciones
{
    /// <inheritdoc />
    public partial class FechaHoraCreacionEnEntidades : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Estatus",
                table: "ProveedoresRecepcion",
                newName: "Estado");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaModificacion",
                table: "TransportistasRecepcion",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaRegistro",
                table: "TransportistasRecepcion",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaModificacion",
                table: "ProveedoresRecepcion",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaRegistro",
                table: "ProveedoresRecepcion",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaModificacion",
                table: "EquiposRecepcion",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaRegistro",
                table: "EquiposRecepcion",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaModificacion",
                table: "TransportistasRecepcion");

            migrationBuilder.DropColumn(
                name: "FechaRegistro",
                table: "TransportistasRecepcion");

            migrationBuilder.DropColumn(
                name: "FechaModificacion",
                table: "ProveedoresRecepcion");

            migrationBuilder.DropColumn(
                name: "FechaRegistro",
                table: "ProveedoresRecepcion");

            migrationBuilder.DropColumn(
                name: "FechaModificacion",
                table: "EquiposRecepcion");

            migrationBuilder.DropColumn(
                name: "FechaRegistro",
                table: "EquiposRecepcion");

            migrationBuilder.RenameColumn(
                name: "Estado",
                table: "ProveedoresRecepcion",
                newName: "Estatus");
        }
    }
}
