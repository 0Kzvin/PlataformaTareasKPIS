using API.Database.Administracion.DTOs.CorreosAutomaticos;
using API.Database.Administracion.DTOs.DLogs;
using API.Database.Administracion.DTOs.Identidad;
using API.Database.Administracion.DTOs.Modulos;
using API.Database.Administracion.Entidades.General;
using API.Database.Administracion.Entidades.Identidad;
using AutoMapper;
using CronExpressionDescriptor;
using System;
using System.Collections.Generic;
using System.Linq;

namespace API.Servicios.Preterminados.Automapper.Mapeos
{
    public class AutoMapperAdministracion : Profile
    {
        // ? Variable utilizada para unir o separar strings de arrayas
        string splitMarker = ";";

        public AutoMapperAdministracion()
        {
            //Identidad
            CreateMap<RegistroUsuarioDTO, Usuarios>()
                .ForMember(x => x.FechaRegistro,
                    opt => opt.MapFrom(y => DateTime.Now))
                .ForMember(x => x.FechaModificacion,
                    opt => opt.MapFrom(y => DateTime.Now))
                .ForMember(x => x.NombreCompleto,
                    opt => opt.MapFrom(y => y.Nombre + " " + y.Apellidos))
                .ForMember(x => x.Estado,
                    opt => opt.MapFrom(y => true));

            CreateMap<Usuarios, UsuarioDTO>()
                .ForMember(x => x.Usuario,
                    opt => opt.MapFrom(src => src.UserName))
                .ForMember(x => x.IdRol,
                    opt => opt.MapFrom(src => src.UsuariosRoles.Select(p => p.RoleId).FirstOrDefault()))
                .ForMember(x => x.NombreRol,
                    opt => opt.MapFrom(src => src.UsuariosRoles.Select(p => p.Rol.Name).FirstOrDefault()));

            CreateMap<Permisos, PermisoDTO>()
                .ForMember(x => x.Nombre,
                    opt => opt.MapFrom(src => src.Nombre))
                .ForMember(x => x.NombreNormalizado,
                    opt => opt.MapFrom(src => src.NombreNormalizado))
                .ForMember(x => x.GrupoNombre,
                    opt => opt.MapFrom(src => src.GrupoPermiso.GrupoNombre))
                .ForMember(x => x.GrupoNombreNormalizado,
                    opt => opt.MapFrom(src => src.GrupoPermiso.GrupoNombreNormalizado))
                .ForMember(x => x.IdRolesAsignados,
                    opt => opt.MapFrom(src => src.RolesPermisos.Select(p => p.IdRol)))
                .ForMember(x => x.IdModulo,
                    opt => opt.MapFrom(src => src.GrupoPermiso.IdModulo))
                .ForMember(x => x.NombreModulo,
                    opt => opt.MapFrom(src => src.GrupoPermiso.Modulo.Nombre))
                .ForMember(x => x.NombreNormalizadoModulo,
                    opt => opt.MapFrom(src => src.GrupoPermiso.Modulo.NombreNormalizado))
                .ForMember(x => x.DescripcionModulo,
                    opt => opt.MapFrom(src => src.GrupoPermiso.Modulo.Descripcion));

            CreateMap<Roles, RolDTO>()
                .ForMember(x => x.Id,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(x => x.NombreRol,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(x => x.NombreRolNormalizado,
                    opt => opt.MapFrom(src => src.NormalizedName))
                .ForMember(x => x.NumeroPermisos,
                    opt => opt.MapFrom(src => src.RolesPermisos.Count()))
                .ForMember(x => x.Estado,
                    opt => opt.MapFrom(src => src.Estado));

            CreateMap<Dictionary<Tuple<string, string>, List<PermisoDTO>>, List<PermisosAgrupadosDTO>>()
                .ForMember("Item", opt => opt.Ignore())
                .AfterMap((s, d, af) =>
                {
                    foreach (var entry in s)
                    {
                        var tupleKey = entry.Key;
                        d.Add(new PermisosAgrupadosDTO
                        {
                            Permisos = entry.Value,
                            NumeroPermisos = entry.Value.Count(),
                            GrupoNombre = tupleKey.Item1,
                            GrupoNombreNormalizado = tupleKey.Item2,
                        });
                    }
                });

            CreateMap<Dictionary<Tuple<Tuple<string, string>, Tuple<string, string>>, List<PermisoDTO>>, List<PermisosAgrupadosDTO>>()
                .ForMember("Item", opt => opt.Ignore())
                .AfterMap((s, d, af) =>
                {
                    foreach (var entry in s)
                    {
                        int idModulo = 0;
                        var tupleKey = entry.Key;
                        int.TryParse(tupleKey.Item2.Item1, out idModulo);
                        d.Add(new PermisosAgrupadosDTO
                        {
                            Permisos = entry.Value,
                            NumeroPermisos = entry.Value.Count(),
                            GrupoNombre = tupleKey.Item1.Item1,
                            GrupoNombreNormalizado = tupleKey.Item1.Item2,
                            IdModulo = idModulo,
                            NombreModulo = tupleKey.Item2.Item2,
                        });
                    }
                });

            CreateMap<Permisos, PermisoOtorgadoDTO>()
                .ForMember(x => x.GrupoNombre,
                    opt => opt.MapFrom(src => src.GrupoPermiso.GrupoNombre));

            CreateMap<EditarUsuarioDTO, Usuarios>()
                .ForMember(x => x.UserName,
                    opt => opt.MapFrom(src => src.Username))
                .ForMember(x => x.Email,
                    opt => opt.MapFrom(src => src.Email))
                .ForMember(x => x.FechaModificacion,
                    opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<CambiarFotoPerfilDTO, Usuarios>()
                .ForMember(x => x.FechaModificacion,
                    opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<EditarMiUsuarioDTO, Usuarios>()
                .ForMember(x => x.UserName,
                    opt => opt.MapFrom(src => src.Username))
                .ForMember(x => x.Email,
                    opt => opt.MapFrom(src => src.Email))
                .ForMember(x => x.FechaModificacion,
                    opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<CorreosAutomaticos, CorreoAutomaticoDTO>()
                .ForMember(x => x.ExpresionCronHumanizada,
                    opt => opt.MapFrom(src => ExpressionDescriptor.GetDescription(src.ExpresionCron, new Options() { Locale = "es-MX" })))
                .ForMember(x => x.ListaDestinatarios,
                    opt => opt.MapFrom(src => !String.IsNullOrWhiteSpace(src.ListaDestinatarios) ? src.ListaDestinatarios.Split(splitMarker, StringSplitOptions.None).ToList() : new List<string>()));
            CreateMap<CorreosAutomaticos, EditarCorreoAutomaticoDTO>()
                .ReverseMap()
                .ForMember(x => x.ListaDestinatarios,
                    opt => opt.MapFrom(src => String.Join(splitMarker, src.ListaDestinatarios)))
                .ForAllMembers(x => x.DoNotAllowNull());

            ////Logs
            //CreateMap<Logs, LogsDTO>();

            //// Configuración de AutoMapper
            //CreateMap<Logs, SincronizarLogDTO>()
            //    .ForMember(dest => dest.Mensaje, opt => opt.MapFrom(src => src.Mensaje))
            //    .ForMember(dest => dest.NivelLog, opt => opt.MapFrom(src => src.Nivel))
            //    .ForMember(dest => dest.FechaHora, opt => opt.MapFrom(src => src.FechaHora))
            //    .ForMember(dest => dest.Metodo, opt => opt.MapFrom(src => src.Accion))
            //    .ForMember(dest => dest.Origen, opt => opt.MapFrom(src => src.Usuario))
            //    .ForMember(dest => dest.Cliente, opt => opt.MapFrom(src => src.Direccion))
            //    .ForMember(dest => dest.DescripcionCliente, opt => opt.MapFrom(src => src.DatosPeticion))
            //    .ForMember(dest => dest.IdUnico, opt => opt.Ignore());

            //// Mapeo inverso (SincronizarLogDTO a Logs)
            //CreateMap<SincronizarLogDTO, Logs>()
            //    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            //    .ForMember(dest => dest.Mensaje, opt => opt.MapFrom(src => src.Mensaje))
            //    .ForMember(dest => dest.Nivel, opt => opt.MapFrom(src => src.NivelLog))
            //    .ForMember(dest => dest.FechaHora, opt => opt.MapFrom(src => src.FechaHora))
            //    .ForMember(dest => dest.Accion, opt => opt.MapFrom(src => src.Metodo))
            //    .ForMember(dest => dest.Usuario, opt => opt.MapFrom(src => src.Origen))
            //    .ForMember(dest => dest.Direccion, opt => opt.MapFrom(src => src.Cliente))
            //    .ForMember(dest => dest.DatosPeticion, opt => opt.MapFrom(src => src.DescripcionCliente));

            //Modulos
            CreateMap<Modulos, ModuloDTO>();
        }
    }
}
