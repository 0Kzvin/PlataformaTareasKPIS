export const idModuloAdmin = 1
export const idModuloDepartamentos = 2
export const idModuloTareas = 3
export const idModuloKpis = 4
export const idModuloReportes = 5
export const idModuloNotificaciones = 6
export const idModuloAuditoria = 7

export const idModuloIds = {
  idModuloAdmin: 1,
  idModuloDepartamentos: 2,
  idModuloTareas: 3,
  idModuloKpis: 4,
  idModuloReportes: 5,
  idModuloNotificaciones: 6,
  idModuloAuditoria: 7,
}

export const nombresModulos = {
  moduloAdmin: 'moduloAdmin',
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
    id: idModuloIds.idModuloAdmin,
    nombre: 'Administracion',
    npPrincipal: 'Usuarios',
    npNoAutorizado: 'NoAutorizadoAdmin',
    npMantenimiento: 'MantenimientoAdmin',
  },
  {
    id: idModuloIds.idModuloDepartamentos,
    nombre: 'Departamentos',
    npPrincipal: 'GestionDepartamentos',
    npNoAutorizado: 'NoAutorizadoDepartamentos',
    npMantenimiento: 'MantenimientoDepartamentos',
  },
  {
    id: idModuloIds.idModuloTareas,
    nombre: 'Tareas',
    npPrincipal: 'ListadoTareas',
    npNoAutorizado: 'NoAutorizadoTareas',
    npMantenimiento: 'MantenimientoTareas',
  },
  {
    id: idModuloIds.idModuloKpis,
    nombre: 'Kpis',
    npPrincipal: 'DashboardGlobal',
    npNoAutorizado: 'NoAutorizadoKpis',
    npMantenimiento: 'MantenimientoKpis',
  },
  {
    id: idModuloIds.idModuloReportes,
    nombre: 'Reportes',
    npPrincipal: 'ReportesPrincipal',
    npNoAutorizado: 'NoAutorizadoReportes',
    npMantenimiento: 'MantenimientoReportes',
  },
  {
    id: idModuloIds.idModuloNotificaciones,
    nombre: 'Notificaciones',
    npPrincipal: 'NotificacionesPrincipal',
    npNoAutorizado: 'NoAutorizadoNotificaciones',
    npMantenimiento: 'MantenimientoNotificaciones',
  },
  {
    id: idModuloIds.idModuloAuditoria,
    nombre: 'Auditoria',
    npPrincipal: 'AuditoriaTrazabilidad',
    npNoAutorizado: 'NoAutorizadoAuditoria',
    npMantenimiento: 'MantenimientoAuditoria',
  },
]

export const modulosItems = [
  {
    mostrar: false,
    idModulo: idModuloAdmin,
    icon: 'pi pi-shield',
    text: 'Módulo de Administracion',
    ruta: '/Administracion/Usuarios',
    llave: 'Administracion',
    llaveAbreviacion: 'AdministracionAb',
  },
  {
    mostrar: false,
    idModulo: idModuloDepartamentos,
    icon: 'pi pi-sitemap',
    text: 'Módulo de Departamentos',
    ruta: '/Departamentos/Gestion',
    llave: 'Departamentos',
    llaveAbreviacion: 'DepartamentosAb',
  },
  {
    mostrar: false,
    idModulo: idModuloTareas,
    icon: 'pi pi-check-square',
    text: 'Módulo de Tareas',
    ruta: '/Tareas/Listado',
    llave: 'Tareas',
    llaveAbreviacion: 'TareasAb',
  },
  {
    mostrar: false,
    idModulo: idModuloKpis,
    icon: 'pi pi-chart-line',
    text: 'Módulo de KPIs y Analítica',
    ruta: '/Kpis/Global',
    llave: 'KpisAnalitica',
    llaveAbreviacion: 'KpisAb',
  },
  {
    mostrar: false,
    idModulo: idModuloReportes,
    icon: 'pi pi-file',
    text: 'Módulo de Reportes',
    ruta: '/Reportes/Resumen',
    llave: 'Reportes',
    llaveAbreviacion: 'ReportesAb',
  },
  {
    mostrar: false,
    idModulo: idModuloNotificaciones,
    icon: 'pi pi-bell',
    text: 'Módulo de Notificaciones',
    ruta: '/Notificaciones/Resumen',
    llave: 'Notificaciones',
    llaveAbreviacion: 'NotificacionesAb',
  },
  {
    mostrar: false,
    idModulo: idModuloAuditoria,
    icon: 'pi pi-eye',
    text: 'Módulo de Auditoría y Trazabilidad',
    ruta: '/Auditoria/Trazabilidad',
    llave: 'Auditoria',
    llaveAbreviacion: 'AuditoriaAb',
  },
]

export default {
  idModuloAdmin,
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
