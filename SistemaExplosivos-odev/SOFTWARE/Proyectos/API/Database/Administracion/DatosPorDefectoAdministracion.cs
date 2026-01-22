using API.Database.Administracion.Entidades.General;
using API.Database.Administracion.Entidades.Identidad;
using API.Servicios.Preterminados.Autorizacion;
using API.Utilidades.Constantes;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Database.Administracion
{
    public class DatosPorDefectoAdministracion
    {
        public static async Task InsertarDatos(ModuloAdministracionExplosivosContext context, UserManager<Usuarios> usuarioManager, RoleManager<Roles> roleManager)
        {
            List<string> rolesDefault = await CrearRolesPredeterminados(roleManager);

            await InsertarModulos(context);
            await InsertarGruposPermisos(context);
            await VerificarIntegridadPermisosLocales(context);
            List<Permisos> permisosNuevos = await InsertarPermisosSistema(context);
            await InsertarRolesPermisosSistema(context, permisosNuevos);
            await InsertarUsuarioPredeterminado(usuarioManager, rolesDefault);
            await InsertarModulosRoles(context);
            await InsertarRegistrosCorreosAutomaticos(context);
        }

        private static async Task<List<string>> CrearRolesPredeterminados(RoleManager<Roles> roleManager)
        {
            // INSERTANDO ROLES POR DEFECTO
            List<string> rolesDefault = new List<string>();

            bool existenRoles = await roleManager.Roles.AnyAsync();

            if (existenRoles)
            {
                return rolesDefault;
            }

            Roles rolSuperUsuario = new Roles
            {
                Name = ConstantesRoles.SUPER_USUARIO_G3,
                EstaOculto = true,
                Descripcion = "Rol por defecto para super usuarios",
                SuperUsuario = true
            };

            Roles rolAdmin = new Roles
            {
                Name = ConstantesRoles.ADMINISTRADOR_PREDETERMINADO,
                Descripcion = "Rol por defecto para administradores del sistema",
            };

            Roles rolLimitado = new Roles
            {
                Name = ConstantesRoles.LIMITADO,
                Descripcion = "Rol por defecto si algún usuario no cuenta con ningún rol",
            };


            await roleManager.CreateAsync(rolSuperUsuario);
            await roleManager.CreateAsync(rolAdmin);
            await roleManager.CreateAsync(rolLimitado);

            rolesDefault.Add(rolSuperUsuario.Name);
            rolesDefault.Add(rolAdmin.Name);

            return rolesDefault;
        }

        private static async Task InsertarModulos(ModuloAdministracionExplosivosContext context)
        {
            // Obtener la lista de módulos predeterminados
            var modulosSistemaPredeterminados = ConstantesModulos.LISTADO_MODULOS;

            bool existenModulos = await context.Modulos.AsNoTracking().AnyAsync();

            if (existenModulos)
            {
                int conteoModulosDB = await context.Modulos.AsNoTracking().CountAsync();
                int conteoModulosPredeterminados = modulosSistemaPredeterminados.Count;

                if (conteoModulosDB == conteoModulosPredeterminados)
                {
                    return;
                }

                var modulosDB = await context.Modulos.AsNoTracking().ToListAsync();

                // Si ya hay módulos en la base de datos, verificar individualmente cuáles no existen
                var modulosPorAgregar = modulosSistemaPredeterminados
                    .Where(modulo => !modulosDB.Exists(m => m.Id == modulo.Id))
                    .ToList();

                if (modulosPorAgregar.Any())
                {
                    await context.Modulos.AddRangeAsync(modulosPorAgregar);
                }
            }
            else
            {
                // Si no hay módulos en la base de datos, insertar todos los módulos predeterminados de una sola vez
                await context.Modulos.AddRangeAsync(modulosSistemaPredeterminados);
            }

            // Guardar cambios en la base de datos
            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error al insertar módulos: {e.Message}");
            }
        }

        private static async Task InsertarGruposPermisos(ModuloAdministracionExplosivosContext context)
        {
            try
            {
                var gruposPermisosPredeterminados = ConstantesGruposPermisos.ObtenerGruposPermisosPredefinidos();
                bool existenPermisos = await context.GruposPermisos.AsNoTracking().AnyAsync();

                if (!existenPermisos)
                {
                    // Si no hay grupos de permisos en la base de datos, insertar todos los permisos
                    await context.GruposPermisos.AddRangeAsync(gruposPermisosPredeterminados);
                    await context.SaveChangesAsync();
                }
                else
                {
                    int conteoGruposPermisosDB = await context.GruposPermisos.AsNoTracking().CountAsync();
                    int conteoGruposPermisosPredeterminados = gruposPermisosPredeterminados.Count;

                    if (conteoGruposPermisosDB == conteoGruposPermisosPredeterminados)
                    {
                        return;
                    }

                    var gruposPermisosDB = await context.GruposPermisos.AsNoTracking().ToListAsync();

                    var gruposPermisosPorAgregar = gruposPermisosPredeterminados
                        .Where(grupo => !gruposPermisosDB.Exists(x => x.GrupoNombre.ToUpper() == grupo.GrupoNombre.ToUpper()))
                        .ToList();

                    if (gruposPermisosPorAgregar.Any())
                    {
                        await context.GruposPermisos.AddRangeAsync(gruposPermisosPorAgregar);
                        await context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error al insertar grupos de permisos: {e.Message}");
            }
        }

        private static async Task VerificarIntegridadPermisosLocales(ModuloAdministracionExplosivosContext context)
        {
            bool existenPermisos = await context.Permisos.AsNoTracking().AnyAsync();

            if (!existenPermisos) return;

            var permisosLocales = PermisosDefault.Todos();

            bool existenPermisosSinIdUnicos = permisosLocales.Exists(x => string.IsNullOrWhiteSpace(x.IdUnico));

            if (existenPermisosSinIdUnicos)
            {
                throw new Exception("Existen permisos sin Id Unico. Favor de revisar los permisos agregados y/o ejecutar la herramienta de diagnostico");
            }

            var conteoPermisosPorIdUnico = permisosLocales.GroupBy(x => x.IdUnico).Count();

            if (permisosLocales.Count != conteoPermisosPorIdUnico)
            {
                throw new Exception("Se han detectado Id Unicos repetidos en permisos por lo cual se detuvo la ejecución. Favor de revisar los últimos permisos agregados y/o ejecutar la herramienta de diagnostico");
            }

            var conteoPermisosPorNombre = permisosLocales.GroupBy(x => x.Nombre).Count();

            if (permisosLocales.Count != conteoPermisosPorNombre)
            {
                throw new Exception("Se han detectado Nombre Llave repetidos en permisos por lo cual se detuvo la ejecución. Favor de revisar los últimos permisos agregados y/o ejecutar la herramienta de diagnostico");
            }
        }

        private static async Task<List<Permisos>> InsertarPermisosSistema(ModuloAdministracionExplosivosContext context)
        {
            try
            {
                var permisosSistemaPredeterminados = PermisosDefault.Todos();
                bool existenPermisos = await context.Permisos.AsNoTracking().AnyAsync();

                //BUSCAR PERMISOS REPETIDOS
                //var gruposPermisosXd = permisosSistemaPredeterminados.GroupBy(x => x.IdUnico).Select(persimoGrupo => new
                //{
                //    IdPermiso = persimoGrupo.Key,
                //    Instancias = persimoGrupo.Count(),
                //}).OrderByDescending(x => x.Instancias).ToList();

                if (!existenPermisos)
                {
                    // Si no hay grupos de permisos en la base de datos, insertar todos los permisos
                    await context.Permisos.AddRangeAsync(permisosSistemaPredeterminados);
                    await context.SaveChangesAsync();

                    return permisosSistemaPredeterminados;
                }
                else
                {
                    int conteoPermisosDB = await context.Permisos.AsNoTracking().CountAsync();
                    int conteoPermisosPredeterminados = permisosSistemaPredeterminados.Count;

                    if (conteoPermisosDB == conteoPermisosPredeterminados)
                    {
                        return new List<Permisos>();
                    }

                    var permisosDB = await context.Permisos.AsNoTracking().ToListAsync();

                    var permisosPorAgregar = permisosSistemaPredeterminados
                        .Where(permiso => !permisosDB.Exists(x => x.Nombre.ToUpper() == permiso.Nombre.ToUpper() || x.IdUnico == permiso.IdUnico))
                        .ToList();

                    if (!permisosPorAgregar.Any())
                    {
                        return new List<Permisos>();
                    }

                    Console.WriteLine($"INSERTANDO {permisosPorAgregar.Count} PERMISOS NUEVOS...\n\n");
                    await context.Permisos.AddRangeAsync(permisosPorAgregar);
                    await context.SaveChangesAsync();
                    return permisosPorAgregar;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error al insertar permisos: {e.Message}");

                return new List<Permisos>();
            }
        }

        private static async Task InsertarRolesPermisosSistema(ModuloAdministracionExplosivosContext context, List<Permisos> permisosNuevos)
        {
            try
            {
                if (!permisosNuevos.Any())
                {
                    return;
                }

                //obtener roles predeterminados
                Roles rolG3SuperUsuario = await context.Roles.AsNoTracking().FirstOrDefaultAsync(x => x.NormalizedName == ConstantesRoles.SUPER_USUARIO_G3.ToUpper() && x.SuperUsuario);
                Roles rolAdmin = await context.Roles.AsNoTracking().FirstOrDefaultAsync(x => x.NormalizedName == ConstantesRoles.ADMINISTRADOR_PREDETERMINADO.ToUpper());
                Roles rolLimitado = await context.Roles.AsNoTracking().FirstOrDefaultAsync(x => x.NormalizedName == ConstantesRoles.LIMITADO.ToUpper());

                Console.WriteLine($"INSERTANDO {permisosNuevos.Count} PERMISOS NUEVOS A ROLES...\n\n");

                foreach (var permiso in permisosNuevos)
                {
                    Console.WriteLine($"INSERTADO PERMISO {permiso.Nombre} ROLES PREDETERMINADOS...\n");
                    await context.RolesPermisos.AddAsync(new RolesPermisos
                    {
                        IdPermiso = permiso.Id,
                        IdRol = rolG3SuperUsuario.Id,
                    });

                    await context.RolesPermisos.AddAsync(new RolesPermisos
                    {
                        IdPermiso = permiso.Id,
                        IdRol = rolAdmin.Id,
                    });

                    if ((permiso.Nombre.Contains("Listar") || permiso.Nombre == "AccesoTablero") && permiso.GrupoPermiso?.IdModulo != ConstantesModulos.ID_ADMINISTRACION)
                    {
                        await context.RolesPermisos.AddAsync(new RolesPermisos
                        {
                            IdPermiso = permiso.Id,
                            IdRol = rolLimitado.Id,
                        });
                    }
                }

                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error al insertar permisos: {e.Message}");
            }
        }
        private static async Task InsertarUsuarioPredeterminado(UserManager<Usuarios> usuarioManager, List<string> rolesDefault)
        {
            bool existenUsuarios = await usuarioManager.Users.AsNoTracking().AnyAsync();

            if (!existenUsuarios)
            {
                var ahora = DateTime.Now;
                var usuarioDefault = new Usuarios
                {
                    Nombre = "G3",
                    Apellidos = "Ingenieria",
                    NombreCompleto = "G3 Ingenieria",
                    Email = "admin@g3innovaciones.com",
                    FechaRegistro = ahora,
                    FechaModificacion = ahora,
                    UserName = "G3",
                    PhoneNumber = "6626888606",
                    Estado = true,
                };

                await usuarioManager.CreateAsync(usuarioDefault, "G3Ingenieri@!");
                await usuarioManager.AddToRolesAsync(usuarioDefault, rolesDefault);
            }
        }

        private static async Task InsertarRegistrosCorreosAutomaticos(ModuloAdministracionExplosivosContext context)
        {
            //try
            //{
            //    bool existeCorreo = false;

            //    var correoEstatusTanques = new CorreosAutomaticos()
            //    {
            //        IdModulo = ConstantesModulos.ID_ALMACENAMIENTO,
            //        NombreModulo = ConstantesModulos.ALMACENAMIENTO,
            //        NombreClave = EnviarCorreoAlmacenamientoJob.NOMBRE_CLAVE,
            //        Nombre = "Reporte de Estatus y Corte de Tanques",
            //        ExpresionCron = "0 0 0 * * ?",
            //        Descripcion = "Reporte de Estatus y Corte de Tanques diario",
            //        Activo = false,
            //        ListaDestinatarios = "",
            //    };

            //    existeCorreo = await context.CorreosAutomaticos
            //                .AnyAsync(x => x.NombreClave == correoEstatusTanques.NombreClave);

            //    if (!existeCorreo)
            //    {
            //        await context.CorreosAutomaticos.AddAsync(correoEstatusTanques);
            //        await context.SaveChangesAsync();
            //    }

            //    var correoEstatusCoriolis = new CorreosAutomaticos()
            //    {
            //        IdModulo = ConstantesModulos.ID_BOMBEO,
            //        NombreModulo = ConstantesModulos.BOMBEO,
            //        NombreClave = EnviarCorreoBombeoJob.NOMBRE_CLAVE,
            //        Nombre = "Reporte de Estatus y Corte de Bombeos",
            //        ExpresionCron = "0 0 0 * * ?",
            //        Descripcion = "Reporte de Estatus y Corte de Bombeos diario",
            //        Activo = false,
            //        ListaDestinatarios = "",
            //    };

            //    existeCorreo = await context.CorreosAutomaticos
            //                .AnyAsync(x => x.NombreClave == correoEstatusCoriolis.NombreClave);

            //    if (!existeCorreo)
            //    {
            //        await context.CorreosAutomaticos.AddAsync(correoEstatusCoriolis);
            //        await context.SaveChangesAsync();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    var xd = ex.Message;
            //}
        }

        private static async Task InsertarModulosRoles(ModuloAdministracionExplosivosContext context)
        {
            // Lista para acumular las relaciones entre roles y módulos a insertar

            var rolesModulosPorInsertar = new List<RolesModulos>();

            // Se obtienen todos los módulos de la base de datos
            var modulos = await context.Modulos.Select(x => x.Id).ToListAsync();

            //obtener roles predeterminados
            Roles rolG3SuperUsuario = await context.Roles.AsNoTracking().FirstOrDefaultAsync(x => x.NormalizedName == ConstantesRoles.SUPER_USUARIO_G3.ToUpper() && x.SuperUsuario);
            Roles rolAdmin = await context.Roles.AsNoTracking().FirstOrDefaultAsync(x => x.NormalizedName == ConstantesRoles.ADMINISTRADOR_PREDETERMINADO.ToUpper());
            Roles rolLimitado = await context.Roles.AsNoTracking().FirstOrDefaultAsync(x => x.NormalizedName == ConstantesRoles.LIMITADO.ToUpper());

            // Si no existe el rol de superusuario no se insertarán módulos para este rol
            if (rolG3SuperUsuario != null)
            {
                // Verifica si ya existen relaciones para todos los módulos y el administrador predeterminado
                var superRoles = await context.RolesModulos.Where(x => x.IdRol == rolG3SuperUsuario.Id).ToListAsync();

                bool existenTodosLosModulosEnRol = modulos.Count == superRoles.Count;

                if (!existenTodosLosModulosEnRol)
                {
                    var superRolesFaltantes = modulos.Except(superRoles.Select(x => x.IdModulo)).ToList();

                    foreach (var modulo in superRolesFaltantes)
                    {
                        rolesModulosPorInsertar.Add(
                                new RolesModulos
                                {
                                    IdModulo = modulo,
                                    IdRol = rolG3SuperUsuario.Id,
                                    EsAdministrador = true
                                }
                            );
                    }
                }
            }

            // Si no existe el rol de administrador predeterminado, no se insertarán módulos para este rol
            if (rolAdmin != null)
            {
                var adminRoles = await context.RolesModulos.Where(x => x.IdRol == rolG3SuperUsuario.Id).ToListAsync();

                // Verifica si ya existen relaciones para todos los módulos y el administrador predeterminado
                bool existenTodosLosModulosEnRol = modulos.Count == adminRoles.Count;

                if (!existenTodosLosModulosEnRol)
                {
                    var adminRolesFaltantes = modulos.Except(adminRoles.Select(x => x.IdModulo)).ToList();

                    // Inserción de módulos para el rol de administrador predeterminado
                    foreach (var modulo in adminRolesFaltantes)
                    {
                        rolesModulosPorInsertar.Add(
                                new RolesModulos
                                {
                                    IdModulo = modulo,
                                    IdRol = rolAdmin.Id,
                                    EsAdministrador = true
                                }
                            );
                    }
                }
            }

            // Si no existe el rol, no se insertarán módulos para este rol
            if (rolLimitado != null)
            {
                // Verifica si ya existen relaciones para todos los módulos y el administrador predeterminado
                var cantidadModulosEnRol = await context.RolesModulos.Where(x => x.IdRol == rolLimitado.Id && x.IdModulo != ConstantesModulos.ID_ADMINISTRACION).ToListAsync();
                int cantidadModulos = modulos.Where(x => x != ConstantesModulos.ID_ADMINISTRACION).Count();
                bool existenTodosLosModulosEnRol = cantidadModulos == cantidadModulosEnRol.Count;

                if (!existenTodosLosModulosEnRol)
                {
                    var limitadoFaltantes = modulos.Except(cantidadModulosEnRol.Select(x => x.IdModulo)).ToList();

                    // Inserción de módulos para el rol de administrador predeterminado
                    foreach (var modulo in limitadoFaltantes)
                    {
                        if (modulo == ConstantesModulos.ID_ADMINISTRACION)
                        {
                            continue;
                        }

                        rolesModulosPorInsertar.Add(
                                new RolesModulos
                                {
                                    IdModulo = modulo,
                                    IdRol = rolLimitado.Id,
                                    EsAdministrador = false
                                }
                            );
                    }
                }
            }

            if (rolesModulosPorInsertar.Any())
            {
                try
                {
                    await context.RolesModulos.AddRangeAsync(rolesModulosPorInsertar);
                    await context.SaveChangesAsync();
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Error al insertar roles de los módulos");
                }
            }
        }
    }
}
