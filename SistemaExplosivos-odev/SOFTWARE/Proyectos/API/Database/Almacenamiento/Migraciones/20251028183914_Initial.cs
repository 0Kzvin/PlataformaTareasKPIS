using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Database.Almacenamiento.Migraciones
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConsumosDepositosAlmacenamiento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUnico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreDeposito = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Producto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumeroEconomico = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MasaInicial = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    MasaFinal = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    AlturaInicial = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    AlturaFinal = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    TotalDespachado = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    Ubicacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaHoraInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaHoraFinal = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsumosDepositosAlmacenamiento", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DepositosAlmacenamiento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUnico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apodo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Producto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CapacidadMaxima = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    AlturaMaxima = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    CapacidadOperativa = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    AlturaOperativa = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    LimiteAlto = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    LimiteMaximo = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    LimiteBajo = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    LimiteMinimo = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    Ubicacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false),
                    Borrado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepositosAlmacenamiento", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EquiposAlmacenamiento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUnico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumeroEconomico = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Apodo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CantidadActual = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    Capacidad = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    EsExterno = table.Column<bool>(type: "bit", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false),
                    Borrado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquiposAlmacenamiento", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EstatusDepositosAlmacenamiento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUnico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreDeposito = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Producto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Masa = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    Altura = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    Porcentaje = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    LimiteMaximo = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    LimiteAlto = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    LimiteBajo = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    LimiteMinimo = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    Ubicacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DispositivoDeMedicion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HayAlarma = table.Column<bool>(type: "bit", nullable: false),
                    FechaHora = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstatusDepositosAlmacenamiento", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MovimientosSupersacosAlmacenamiento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUnico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ubicacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Producto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CantidadInicial = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    CantidadFinal = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    CantidadMovimiento = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    FechaHora = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimientosSupersacosAlmacenamiento", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConsumosDepositosAlmacenamiento");

            migrationBuilder.DropTable(
                name: "DepositosAlmacenamiento");

            migrationBuilder.DropTable(
                name: "EquiposAlmacenamiento");

            migrationBuilder.DropTable(
                name: "EstatusDepositosAlmacenamiento");

            migrationBuilder.DropTable(
                name: "MovimientosSupersacosAlmacenamiento");
        }
    }
}
