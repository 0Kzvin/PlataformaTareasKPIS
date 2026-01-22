using API.Database.Administracion.Entidades.Identidad;
using System.Collections.Generic;

namespace API.Utilidades.Constantes
{
    public static class ConstantesModulos
    {
        public const string ADMINISTRACION = "Administracion";
        public const int ID_ADMINISTRACION = 1;

        public const string ALMACENAMIENTO = "Almacenamiento";
        public const int ID_ALMACENAMIENTO = 2;

        public const string RECEPCION = "Recepcion";
        public const int ID_RECEPCION = 3;

        public const string ACCESORIOS = "Accesorios";
        public const int ID_ACCESORIOS = 4;

        public const string GERENCIA = "Gerencia";
        public const int ID_GERENCIA = 5;

        public static List<Modulos> LISTADO_MODULOS => new List<Modulos>
        {
            new Modulos
            {
                Id = ID_ADMINISTRACION,
                Nombre = ADMINISTRACION,
                NombreNormalizado = "ModuloAdministracion",
                Descripcion = "DescModuloAdministracion",
                Estado = true
            },
            new Modulos
            {
                Id = ID_ALMACENAMIENTO,
                Nombre = ALMACENAMIENTO,
                NombreNormalizado = "ModuloAlmacenamiento",
                Descripcion = "DescModuloAlmacenamiento",
                Estado = true
            },
            new Modulos
            {
                Id = ID_RECEPCION,
                Nombre = RECEPCION,
                NombreNormalizado = "ModuloRecepcion",
                Descripcion = "DescModuloRecepcion",
                Estado = true
            },
            new Modulos
            {
                Id = ID_ACCESORIOS,
                Nombre = ACCESORIOS,
                NombreNormalizado = "ModuloAccesorios",
                Descripcion = "DescModuloAccesorios",
                Estado = true
            },
            new Modulos
            {
                Id = ID_GERENCIA,
                Nombre = GERENCIA,
                NombreNormalizado = "ModuloGerencia",
                Descripcion = "DescModuloGerencia",
                Estado = true
            }
        };
    }
}
