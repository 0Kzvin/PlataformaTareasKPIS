export const idModuloAdmin = 1
export const idModuloAlmacenamiento = 2
export const idModuloRecepcion = 3
export const idModuloAccesorios = 4
export const idModuloGerencia = 5

export const idModuloIds = {
  idModuloAdmin: 1,
  idModuloAlmacenamiento: 2,
  idModuloRecepcion: 3,
  idModuloAccesorios: 4,
  idModuloGerencia: 5,
}

export const nombresModulos = {
  moduloAdmin: 'moduloAdmin',
  moduloAlmacenamiento: 'moduloAlmacenamiento',
  moduloRecepcion: 'moduloRecepcion',
  moduloAccesorios: 'moduloAccesorios',
  moduloGerencia: 'moduloGerencia',
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
    id: idModuloIds.idModuloAlmacenamiento,
    nombre: 'Almacenamiento',
    npPrincipal: 'TableroAlmacenamiento',
    npNoAutorizado: 'NoAutorizadoAlmacenamiento',
    npMantenimiento: 'MantenimientoAlmacenamiento',
  },
  {
    id: idModuloIds.idModuloRecepcion,
    nombre: 'Recepcion',
    npPrincipal: 'TableroRecepcion',
    npNoAutorizado: 'NoAutorizadoRecepcion',
    npMantenimiento: 'MantenimientoRecepcion',
  },
  {
    id: idModuloIds.idModuloAccesorios,
    nombre: 'Accesorios',
    npPrincipal: 'TableroAccesorios',
    npNoAutorizado: 'NoAutorizadoAccesorios',
    npMantenimiento: 'MantenimientoAccesorios',
  },
  {
    id: idModuloIds.idModuloGerencia,
    nombre: 'Gerencia',
    npPrincipal: 'AlmacenamientoGerencia',
    npNoAutorizado: 'NoAutorizadoGerencia',
    npMantenimiento: 'MantenimientoGerencia',
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
    idModulo: idModuloAlmacenamiento,
    icon: 'pi pi-database',
    text: 'Módulo de Almacenamiento',
    ruta: '/Almacenamiento/TableroAlmacenamiento',
    llave: 'Almacenamiento',
    llaveAbreviacion: 'AlmacenamientoAb',
  },
  {
    mostrar: false,
    idModulo: idModuloRecepcion,
    icon: 'pi pi-truck',
    text: 'Módulo de Recepción',
    ruta: '/Recepcion/TableroRecepcion',
    llave: 'Recepcion',
    llaveAbreviacion: 'RecepcionAb',
  },
  {
    mostrar: false,
    idModulo: idModuloAccesorios,
    icon: 'pi pi-box',
    text: 'Módulo de Accesorios',
    ruta: '/Accesorios/TableroAccesorios',
    llave: 'Accesorios',
    llaveAbreviacion: 'AccesariosAb',
  },
  {
    mostrar: false,
    idModulo: idModuloGerencia,
    icon: 'pi pi-lock',
    text: 'Módulo de Gerencia',
    ruta: '/Gerencia/AlmacenamientoGerencia',
    llave: 'Gerencia',
    llaveAbreviacion: 'GerenciaAb',
  },
]

export default {
  idModuloAdmin,
  idModuloAlmacenamiento,
  idModuloRecepcion,
  idModuloAccesorios,
  idModuloGerencia,
  idModuloIds,
  nombresModulos,
  Todos,
  detallesModulos,
  modulosItems,
}
