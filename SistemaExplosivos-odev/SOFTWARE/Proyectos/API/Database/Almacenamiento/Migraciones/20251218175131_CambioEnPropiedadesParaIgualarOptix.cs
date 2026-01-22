using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Database.Almacenamiento.Migraciones
{
    /// <inheritdoc />
    public partial class CambioEnPropiedadesParaIgualarOptix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConsumosDepositosAlmacenamiento");

            migrationBuilder.RenameColumn(
                name: "Porcentaje",
                table: "EstatusDepositosAlmacenamiento",
                newName: "Volumen");

            migrationBuilder.RenameColumn(
                name: "NombreDeposito",
                table: "EstatusDepositosAlmacenamiento",
                newName: "Deposito");

            migrationBuilder.RenameColumn(
                name: "Masa",
                table: "EstatusDepositosAlmacenamiento",
                newName: "PorcentajeNivel");

            migrationBuilder.CreateTable(
                name: "CargasDepositosAlmacenamientos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUnico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Deposito = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Producto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumeroEconomico = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ubicacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NivelInicial = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    NivelFinal = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    VolumenInicial = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    VolumenFinal = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    VolumenCargado = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    VolumenCargadoReal = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    FechaHoraInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaHoraFinal = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CargasDepositosAlmacenamientos", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CargasDepositosAlmacenamientos");

            migrationBuilder.RenameColumn(
                name: "Volumen",
                table: "EstatusDepositosAlmacenamiento",
                newName: "Porcentaje");

            migrationBuilder.RenameColumn(
                name: "PorcentajeNivel",
                table: "EstatusDepositosAlmacenamiento",
                newName: "Masa");

            migrationBuilder.RenameColumn(
                name: "Deposito",
                table: "EstatusDepositosAlmacenamiento",
                newName: "NombreDeposito");

            migrationBuilder.CreateTable(
                name: "ConsumosDepositosAlmacenamiento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AlturaFinal = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    AlturaInicial = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    FechaHoraFinal = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaHoraInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdUnico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MasaFinal = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    MasaInicial = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    NombreDeposito = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumeroEconomico = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Producto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalDespachado = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    Ubicacion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsumosDepositosAlmacenamiento", x => x.Id);
                });
        }
    }
}
