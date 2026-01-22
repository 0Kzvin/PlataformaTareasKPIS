import { setCssVar, Dark, colors } from 'quasar'

// Colores por defecto (G3 / Aceites como base inicial)
const DEFAULT_COLORS = {
  primary: '#e41321',
  secondary: '#7e7e7e',
  positive: '#149849',
  negative: '#d24b59',
  warning: '#f2c037',
  info: '#9de0f6',
}

const STORAGE_KEY_COLORS = 'app_custom_colors'
const STORAGE_KEY_DARK = 'app_dark_mode'

// Obtener colores guardados o defaults
const obtenerColoresGuardados = () => {
  const saved = localStorage.getItem(STORAGE_KEY_COLORS)
  if (saved) {
    return { ...DEFAULT_COLORS, ...JSON.parse(saved) }
  }
  return { ...DEFAULT_COLORS }
}

// Guardar colores
const guardarColores = (colores) => {
  localStorage.setItem(STORAGE_KEY_COLORS, JSON.stringify(colores))
}

// Aplicar colores al DOM usando Quasar utils
const aplicarColores = (colores) => {
  Object.keys(colores).forEach((key) => {
    setCssVar(key, colores[key])
    // Calcular y establecer el color de contraste
    const colorHex = colores[key]
    const contrastColor = colors.luminosity(colorHex) > 0.5 ? '#000000' : '#ffffff'

    setCssVar(`${key}-contrast`, contrastColor)
  })
}

// Obtener estado dark mode
const obtenerEstadoDark = () => {
  return localStorage.getItem(STORAGE_KEY_DARK) === 'true'
}

// Alternar y guardar Dark Mode
const toggleDarkMode = (status) => {
  Dark.set(status)
  localStorage.setItem(STORAGE_KEY_DARK, status)
}

// Inicializar tema (llamado al cargar la app)
const inicializarTema = () => {
  const colores = obtenerColoresGuardados()
  const isDark = obtenerEstadoDark()

  aplicarColores(colores)
  toggleDarkMode(isDark)

  return { colores, isDark }
}

export {
  DEFAULT_COLORS,
  obtenerColoresGuardados,
  guardarColores,
  aplicarColores,
  toggleDarkMode,
  obtenerEstadoDark,
  inicializarTema,
}
