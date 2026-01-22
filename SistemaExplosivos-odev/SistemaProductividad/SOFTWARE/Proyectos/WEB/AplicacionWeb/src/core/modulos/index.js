export const idModuloAdministracion = 1
export const idModuloDepartamentos = 2
export const idModuloTareas = 3
export const idModuloKpis = 4
export const idModuloReportes = 5
export const idModuloNotificaciones = 6
export const idModuloAuditoria = 7

export const idModuloIds = {
  idModuloAdministracion: 1,
  idModuloDepartamentos: 2,
  idModuloTareas: 3,
  idModuloKpis: 4,
  idModuloReportes: 5,
  idModuloNotificaciones: 6,
  idModuloAuditoria: 7,
}

export const nombresModulos = {
  moduloAdministracion: 'moduloAdministracion',
  moduloDepartamentos: 'moduloDepartamentos',
  moduloTareas: 'moduloTareas',
  moduloKpis: 'moduloKpis',
  moduloReportes: 'moduloReportes',
  moduloNotificaciones: 'moduloNotificaciones',
  moduloAuditoria: 'moduloAuditoria',
}

export const Todos = () => {
  return Object.values(idModuloIds)
}

//// np = NOMBRE PÁGINA
export const detallesModulos = [
  {
    id: idModuloIds.idModuloAdministracion,
    nombre: 'Administracion',
    npPrincipal: 'AdministracionUsuarios',
    npNoAutorizado: 'NoAutorizado',
    npMantenimiento: 'Mantenimiento',
  },
  {
    id: idModuloIds.idModuloDepartamentos,
    nombre: 'Departamentos',
    npPrincipal: 'GestionDepartamentos',
    npNoAutorizado: 'NoAutorizado',
    npMantenimiento: 'Mantenimiento',
  },
  {
    id: idModuloIds.idModuloTareas,
    nombre: 'Tareas',
    npPrincipal: 'MisTareas',
    npNoAutorizado: 'NoAutorizado',
    npMantenimiento: 'Mantenimiento',
  },
  {
    id: idModuloIds.idModuloKpis,
    nombre: 'Kpis',
    npPrincipal: 'DashboardGlobal',
    npNoAutorizado: 'NoAutorizado',
    npMantenimiento: 'Mantenimiento',
  },
  {
    id: idModuloIds.idModuloReportes,
    nombre: 'Reportes',
    npPrincipal: 'ReportesListado',
    npNoAutorizado: 'NoAutorizado',
    npMantenimiento: 'Mantenimiento',
  },
  {
    id: idModuloIds.idModuloNotificaciones,
    nombre: 'Notificaciones',
    npPrincipal: 'NotificacionesInbox',
    npNoAutorizado: 'NoAutorizado',
    npMantenimiento: 'Mantenimiento',
  },
  {
    id: idModuloIds.idModuloAuditoria,
    nombre: 'Auditoria',
    npPrincipal: 'AuditoriaRegistros',
    npNoAutorizado: 'NoAutorizado',
    npMantenimiento: 'Mantenimiento',
  },
]

export const modulosItems = [
  {
    mostrar: false,
    idModulo: idModuloAdministracion,
    icon: 'pi pi-shield',
    text: 'Módulo de Administración',
    ruta: '/Administracion/Usuarios',
    llave: 'Administracion',
    llaveAbreviacion: 'AdministracionAb',
  },
  {
    mostrar: false,
    idModulo: idModuloDepartamentos,
    icon: 'pi pi-sitemap',
    text: 'Módulo de Departamentos',
    ruta: '/Departamentos/GestionDepartamentos',
    llave: 'Departamentos',
    llaveAbreviacion: 'DepartamentosAb',
  },
  {
    mostrar: false,
    idModulo: idModuloTareas,
    icon: 'pi pi-check-square',
    text: 'Módulo de Tareas',
    ruta: '/Tareas/MisTareas',
    llave: 'Tareas',
    llaveAbreviacion: 'TareasAb',
  },
  {
    mostrar: false,
    idModulo: idModuloKpis,
    icon: 'pi pi-chart-line',
    text: 'Módulo de KPIs',
    ruta: '/Kpis/DashboardGlobal',
    llave: 'Kpis',
    llaveAbreviacion: 'KpisAb',
  },
  {
    mostrar: false,
    idModulo: idModuloReportes,
    icon: 'pi pi-file',
    text: 'Módulo de Reportes',
    ruta: '/Reportes/Reportes',
    llave: 'Reportes',
    llaveAbreviacion: 'ReportesAb',
  },
  {
    mostrar: false,
    idModulo: idModuloNotificaciones,
    icon: 'pi pi-bell',
    text: 'Módulo de Notificaciones',
    ruta: '/Notificaciones/Inbox',
    llave: 'Notificaciones',
    llaveAbreviacion: 'NotificacionesAb',
  },
  {
    mostrar: false,
    idModulo: idModuloAuditoria,
    icon: 'pi pi-history',
    text: 'Módulo de Auditoría',
    ruta: '/Auditoria/Registros',
    llave: 'Auditoria',
    llaveAbreviacion: 'AuditoriaAb',
  },
]

export default {
  idModuloAdministracion,
  idModuloDepartamentos,
  idModuloTareas,
  idModuloKpis,
  idModuloReportes,
  idModuloNotificaciones,
  idModuloAuditoria,
  idModuloIds,
  nombresModulos,
  Todos,
  detallesModulos,
  modulosItems,
}
