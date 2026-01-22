import * as cargas from './cargas'
import * as tanques from './tanques'
import * as equipos from './equipos'
import * as estatus from './estatus'
import * as movimientos from './movimientos'
import * as analisis from './analisis'
import * as descargas from './descargas'

export const Todos = () => {
  return [
    ...cargas.Todos(),
    ...tanques.Todos(),
    ...equipos.Todos(),
    ...estatus.Todos(),
    ...movimientos.Todos(),
    ...analisis.Todos(),
    ...descargas.Todos(),
  ]
}

export { cargas, tanques, equipos, estatus, movimientos, analisis, descargas }

export default {
  cargas,
  tanques,
  equipos,
  estatus,
  movimientos,
  analisis,
  descargas,
}
