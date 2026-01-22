import { computed } from 'vue'
import { useRouter } from 'vue-router'
import { useSesionStore } from 'src/stores/sesion'

export function useNavigation() {
  const router = useRouter()
  const store = useSesionStore()

  const permisosOtorgados = computed(() => store.permisosOtorgados)

  const obtenerMenuPorModulo = (ordenModuloId) => {
    // Buscar la ruta padre que corresponda al módulo
    const rutas = router.options.routes

    // Estrategia: Buscar la ruta cuyo 'children' tenga rutas con el idModulo especificado
    // OJO: Ahora con anidación, el 'children' directo es el Grupo, y sus hijos tienen el idModulo.
    // Buscamos recursivamente o asumimos estructura Nivel 1 (Modulo) -> Nivel 2 (Grupo) -> Nivel 3 (Pagina con idModulo)
    const rutaModulo = rutas.find((route) => {
      // Caso 1: Estructura plana antigua (por si acaso) o mixta
      const tieneHijoDirecto = route.children?.some(
        (child) => child.meta?.idModulo === ordenModuloId,
      )
      if (tieneHijoDirecto) return true

      // Caso 2: Estructura anidada (Nivel 2 es grupo, Nivel 3 es pagina)
      const tieneNieto = route.children?.some((group) =>
        group.children?.some((page) => page.meta?.idModulo === ordenModuloId),
      )
      return tieneNieto
    })

    if (!rutaModulo || !rutaModulo.children) return []

    const groups = []

    const itemsSueltos = []

    rutaModulo.children.forEach((childRoute) => {
      // Validar si es una ruta oculta
      if (childRoute.meta?.sidebar?.visible === false) return

      // Caso A: Es un GRUPO Anidado (tiene children)
      if (childRoute.children && childRoute.children.length > 0) {
        const groupObj = {
          seccionOculta: false,
          icono:
            childRoute.meta?.sidebar?.icon || childRoute.meta?.sidebar?.icono || 'pi pi-folder',
          active: childRoute.meta?.sidebar?.active ?? true,
          titulo: childRoute.meta?.sidebar?.title || childRoute.meta?.sidebar?.titulo || 'Grupo',
          llave: childRoute.name || childRoute.meta?.sidebar?.title,
          items: [],
        }

        childRoute.children.forEach((pageRoute) => {
          if (pageRoute.meta?.sidebar?.visible === false) return

          if (pageRoute.meta?.permisos) {
            const permisosRequeridos = pageRoute.meta.permisos
            const tienePermiso = permisosOtorgados.value.some((p) =>
              permisosRequeridos.includes(p.nombre),
            )
            if (!tienePermiso) return
          }

          groupObj.items.push({
            icono: pageRoute.meta?.sidebar?.icon || pageRoute.meta?.sidebar?.icono,
            titulo: pageRoute.meta?.sidebar?.title || pageRoute.meta?.sidebar?.titulo,
            ruta: pageRoute.path,
            llave: pageRoute.name,
            permisos: pageRoute.meta?.permisos,
            ...pageRoute.meta?.sidebar,
          })
        })

        if (groupObj.items.length > 0) {
          groups.push(groupObj)
        }
      }
      // Caso B: Es una Página Suelta (Hijo directo del módulo)
      else {
        // Chequeo de permisos para el item suelto
        if (childRoute.meta?.permisos) {
          const permisosRequeridos = childRoute.meta.permisos
          const tienePermiso = permisosOtorgados.value.some((p) =>
            permisosRequeridos.includes(p.nombre),
          )
          if (!tienePermiso) return
        }

        // Agregar a itemsSueltos
        itemsSueltos.push({
          icono: childRoute.meta?.sidebar?.icon || childRoute.meta?.sidebar?.icono,
          titulo: childRoute.meta?.sidebar?.title || childRoute.meta?.sidebar?.titulo,
          ruta: childRoute.path,
          llave: childRoute.name,
          permisos: childRoute.meta?.permisos,
          ...childRoute.meta?.sidebar,
        })
      }
    })

    // Si hubo items sueltos, los agrupamos bajo la metadata del Módulo Padre
    if (itemsSueltos.length > 0) {
      const grupoPadre = {
        seccionOculta: false,
        icono:
          rutaModulo.meta?.sidebar?.icon ||
          rutaModulo.meta?.sidebar?.icono ||
          rutaModulo.meta?.sidebar?.groupIcon || // Fallback a groupIcon si existe
          'pi pi-folder',
        active: rutaModulo.meta?.sidebar?.active ?? true,
        titulo:
          rutaModulo.meta?.sidebar?.title ||
          rutaModulo.meta?.sidebar?.titulo ||
          rutaModulo.meta?.sidebar?.groupTitle || // Fallback a groupTitle si existe
          'General',
        llave: rutaModulo.name,
        items: itemsSueltos,
      }
      groups.push(grupoPadre)
    }

    return groups
  }

  return {
    obtenerMenuPorModulo,
  }
}
