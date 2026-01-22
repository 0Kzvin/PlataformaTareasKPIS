import { useSesionStore } from 'src/stores/sesion'

export const estaModuloOtorgado = (idModulo = 0) => {
  const modulosOtorgados = useSesionStore().modulosOtorgados
  return modulosOtorgados.filter((x) => x.id == idModulo).length > 0
}
