import { useSesionStore } from 'src/stores/sesion'

export const estaPermisoOtorgado = (nombrePermiso = '') => {
  const permisosOtorgados = useSesionStore().permisosOtorgados
  return permisosOtorgados.filter((x) => x.nombre == nombrePermiso).length > 0
}

export const estaAlgunPermisoOtorgado = async (permisos = []) => {
  const permisosOtorgados = useSesionStore().permisosOtorgados
  return permisosOtorgados.some((x) => permisos.includes(x))
}

export const estaTodosPermisosOtorgados = async (permisos = []) => {
  const permisosOtorgados = useSesionStore().permisosOtorgados
  return permisosOtorgados.every((x) => permisos.includes(x))
}
