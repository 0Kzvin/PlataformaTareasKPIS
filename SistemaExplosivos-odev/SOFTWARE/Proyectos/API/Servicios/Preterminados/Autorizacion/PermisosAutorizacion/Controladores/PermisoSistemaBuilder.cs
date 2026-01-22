using API.Database.Administracion.Entidades.Identidad;
using System;

namespace API.Servicios.Preterminados.Autorizacion.PermisosAutorizacion.Controladores
{
    public class PermisoSistemaBuilder
    {
        private readonly GruposPermisos _gruposPermiso;

        public PermisoSistemaBuilder(GruposPermisos gruposPermiso)
        {
            _gruposPermiso = gruposPermiso;
        }

        public Permisos CrearPermiso(string nombrePermiso, string descripcion, string idUnico)
        {
            return new Permisos
            {
                NombreNormalizado = $"{nombrePermiso} {_gruposPermiso.GrupoNombre}",
                Descripcion = descripcion,
                Nombre = nombrePermiso,
                Id = Guid.CreateVersion7().ToString(),
                IdUnico = idUnico,
                IdGrupoPermiso = _gruposPermiso.Id
            };
        }
    }
}
