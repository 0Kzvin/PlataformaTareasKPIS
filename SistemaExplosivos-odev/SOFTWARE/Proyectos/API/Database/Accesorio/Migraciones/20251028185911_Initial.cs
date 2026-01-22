using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Database.Accesorio.Migraciones
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoriasAccesorios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUnico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnidadDeMedida = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LimiteCapacidadMaximaSedena = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    LimiteConsumoMaximaSedena = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    Estatus = table.Column<bool>(type: "bit", nullable: false),
                    Borrado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriasAccesorios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConsumosAccesorios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUnico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Folio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Destino = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirmaReciboHanka = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirmaJefe = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirmaAlmacen = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstaCerrado = table.Column<bool>(type: "bit", nullable: false),
                    FechaHora = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsumosAccesorios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DestinosAccesorios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUnico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: false),
                    Borrado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DestinosAccesorios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EntradasAccesorios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUnico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumeroStock = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnidadDeMedida = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumeroPedido = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DMEntrada = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CantidadInicial = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    CantidadFinal = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    Transportista = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Placas = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Chofer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Proveedor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumeroFactura = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CantidadFacturada = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaHoraSalida = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaHoraFactura = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaHoraRecepcion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntradasAccesorios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FacturasAccesorios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUnico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ubicacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Producto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumeroFactura = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Folio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remision = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Proveedor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Transportista = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Equipo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Conductor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Origen = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CantidadFacturada = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    DiferenciaPesoContraFacturado = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    Remanente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tolerancia = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    FechaHora = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacturasAccesorios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProveedoresAccesorios",
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
                    table.PrimaryKey("PK_ProveedoresAccesorios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SalidasAccesorios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUnico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdConsumo = table.Column<int>(type: "int", nullable: false),
                    NumeroStock = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumeroSalida = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnidadDeMedida = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CantidadASacar = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    CantidadInicial = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    CantidadFinal = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    FactorCorrecion = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    CantidadCorregida = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    EsDevolucion = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalidasAccesorios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalidasAccesorios_ConsumosAccesorios_IdConsumo",
                        column: x => x.IdConsumo,
                        principalTable: "ConsumosAccesorios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Accesorios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUnico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdCategoria = table.Column<int>(type: "int", nullable: false),
                    IdProveedor = table.Column<int>(type: "int", nullable: false),
                    IdConjunto = table.Column<int>(type: "int", nullable: true),
                    NumeroStock = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Apodo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnidadDeMedida = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LimiteMaximo = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    LimiteAlto = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    LimiteBajo = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    LimiteMinimo = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    LimiteConsumoMaximo = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    LimiteConsumoAlto = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    CantidadActual = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    ConsumoPromedioDiario = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    ConsumoPromedioDiarioCalculado = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    FactorCorreccionKg = table.Column<decimal>(type: "decimal(30,2)", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false),
                    Borrado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accesorios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accesorios_Accesorios_IdConjunto",
                        column: x => x.IdConjunto,
                        principalTable: "Accesorios",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Accesorios_CategoriasAccesorios_IdCategoria",
                        column: x => x.IdCategoria,
                        principalTable: "CategoriasAccesorios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Accesorios_ProveedoresAccesorios_IdProveedor",
                        column: x => x.IdProveedor,
                        principalTable: "ProveedoresAccesorios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accesorios_IdCategoria",
                table: "Accesorios",
                column: "IdCategoria");

            migrationBuilder.CreateIndex(
                name: "IX_Accesorios_IdConjunto",
                table: "Accesorios",
                column: "IdConjunto");

            migrationBuilder.CreateIndex(
                name: "IX_Accesorios_IdProveedor",
                table: "Accesorios",
                column: "IdProveedor");

            migrationBuilder.CreateIndex(
                name: "IX_SalidasAccesorios_IdConsumo",
                table: "SalidasAccesorios",
                column: "IdConsumo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accesorios");

            migrationBuilder.DropTable(
                name: "DestinosAccesorios");

            migrationBuilder.DropTable(
                name: "EntradasAccesorios");

            migrationBuilder.DropTable(
                name: "FacturasAccesorios");

            migrationBuilder.DropTable(
                name: "SalidasAccesorios");

            migrationBuilder.DropTable(
                name: "CategoriasAccesorios");

            migrationBuilder.DropTable(
                name: "ProveedoresAccesorios");

            migrationBuilder.DropTable(
                name: "ConsumosAccesorios");
        }
    }
}
