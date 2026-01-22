using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Database.Recepcion.Migraciones
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConductoresRecepcion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUnico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false),
                    Borrado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConductoresRecepcion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrigenesRecepcion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUnico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false),
                    Borrado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrigenesRecepcion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProveedoresRecepcion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUnico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apodo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RFC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Foto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tolerancia = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    Estatus = table.Column<bool>(type: "bit", nullable: false),
                    Borrado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProveedoresRecepcion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Recepciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUnico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Folio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumeroPedido = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrdenEmbarque = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Producto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DMEntrada = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaHoraSalida = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DiferenciaEntreFechas = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    PesoBruto = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    PesoTara = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    PesoNeto = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    Diferencia = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    FechaHoraRecepcion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ubicacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Transportista = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Placas = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Chofer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Proveedor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RFCProveedor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumeroFactura = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CantidadFactura = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    NumeroBoletaFactura = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaHoraFactura = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recepciones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransportistasRecepcion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUnico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdProveedor = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apodo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: false),
                    Borrado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportistasRecepcion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransportistasRecepcion_ProveedoresRecepcion_IdProveedor",
                        column: x => x.IdProveedor,
                        principalTable: "ProveedoresRecepcion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransaccionesRecepcion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUnico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdRecepcion = table.Column<int>(type: "int", nullable: false),
                    Ubicacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Producto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumeroFactura = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Folio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remision = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KilosTransaccion = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    KilosFactura = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    PesoBruto = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    PesoTara = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    PesoNeto = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    Diferencia = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    DiferenciaPesoContraFacturado = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    Remanente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tolerancia = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    FechaHora = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransaccionesRecepcion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransaccionesRecepcion_Recepciones_IdRecepcion",
                        column: x => x.IdRecepcion,
                        principalTable: "Recepciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EquiposRecepcion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUnico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdTransportista = table.Column<int>(type: "int", nullable: false),
                    Placa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false),
                    Borrado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquiposRecepcion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquiposRecepcion_TransportistasRecepcion_IdTransportista",
                        column: x => x.IdTransportista,
                        principalTable: "TransportistasRecepcion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquiposRecepcion_IdTransportista",
                table: "EquiposRecepcion",
                column: "IdTransportista");

            migrationBuilder.CreateIndex(
                name: "IX_TransaccionesRecepcion_IdRecepcion",
                table: "TransaccionesRecepcion",
                column: "IdRecepcion");

            migrationBuilder.CreateIndex(
                name: "IX_TransportistasRecepcion_IdProveedor",
                table: "TransportistasRecepcion",
                column: "IdProveedor");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConductoresRecepcion");

            migrationBuilder.DropTable(
                name: "EquiposRecepcion");

            migrationBuilder.DropTable(
                name: "OrigenesRecepcion");

            migrationBuilder.DropTable(
                name: "TransaccionesRecepcion");

            migrationBuilder.DropTable(
                name: "TransportistasRecepcion");

            migrationBuilder.DropTable(
                name: "Recepciones");

            migrationBuilder.DropTable(
                name: "ProveedoresRecepcion");
        }
    }
}
