<template>
  <div class="row full-height">
    <!-- LOADING -->
    <div v-if="cargandoTanques" class="col-12 flex flex-center">
      <BuscandoDataApi />
    </div>

    <!-- CONTENIDO -->
    <div v-else class="col-12">
      <draggable
        :move="movimientoTanque"
        @end="terminarMovimiento"
        class="row q-col-gutter-md"
        :list="listaTanques"
        :animation="300"
      >
        <template #item="{ element }">
          <transition-group
            name="fade-move"
            tag="div"
            class="col-lg-4 col-md-6 col-sm-12 col-xs-12"
          >
            <DepositoAlmacenamientoCard
              :deposito="element"
              :productos="productos"
              :dato-remoto="obtenerDatoRemotoDeposito(element)"
              :tiene-conexion="datosEstacion.tieneConexion"
            />
          </transition-group>
        </template>
      </draggable>
    </div>
  </div>
</template>

<script setup>
import { onBeforeUnmount, onMounted, ref, toRefs } from 'vue'
import draggable from 'vuedraggable'
import datosClientes from 'src/utils/datosClientes'
import SignalREstacionNetCore from 'src/utils/signalR/estacionNetCore'
import { estatusAlmacenamiento } from 'src/api/moduloAlmacenamiento'
import DepositoAlmacenamientoCard from './DepositoAlmacenamientoCard.vue'
import BuscandoDataApi from 'src/components/Globales/BuscandoDataApi.vue'

const datosCliente = datosClientes.obtenerDatosCliente(process.env.clienteActual)

const userHostApp = window.location.host

const apiLocal = userHostApp.includes('192.')
  ? 'http://192.168.1.250:8082'
  : 'http://localhost:8082'

const apiUrl = process.env.DEV ? apiLocal : datosCliente?.apiUrl || apiLocal

const estacionExplosivos = 'EstacionHub'
const nombreMetodoRecibirDatos = 'RecibirDatosAlmacenamiento'

//VARIABLES PARA CONTROLAR EL ORDENAMIENTO DE LOS TANQUES, INDICAN QUE TIPO DE
//ORDENAMIENTO SIGUEN EN ESTE MOMENTO
const estaOrdenadoTanquesMayorMenor = ref(false)
const estaOrdenadoTanquesMenorMayor = ref(false)
const estaOrdenadoEstacionesAaZ = ref(false)
const estaOrdenadoEstacionesZaA = ref(false)
const estaOrdenadoEstadosConectado = ref(false)
const estaOrdenadoEstadosDesconectado = ref(false)

const almacenamientoHub = ref({})

const props = defineProps({
  estacion: {
    type: Object,
    default: () => {},
  },
  productos: {
    type: Array,
    default: () => [],
  },
})

const { estacion, productos } = toRefs(props)

const cargandoTanques = ref(false)

const listaTanques = ref([])

const datosEstacion = ref({
  idEstacion: '',
  ipEstacion: estacion.value.ip,
  nombreEstacion: estacion.value.nombre,
  tieneConexion: false,
  datosRemotos: [],
})

let timeoutId = null

function iniciarTimeout() {
  limpiarTimeout()

  timeoutId = setTimeout(() => {}, 10000)
}

function limpiarTimeout() {
  if (timeoutId) {
    clearTimeout(timeoutId)
    timeoutId = null
  }
}

onMounted(async () => {
  await refrescarTodo()
})

onBeforeUnmount(async () => {
  await almacenamientoHub.value.desconectarConexionHub()
  limpiarTimeout()
})

const refrescarTodo = async () => {
  await refrescarTanques()
  configurarHub()
  iniciarTimeout()
}

const configurarHub = () => {
  almacenamientoHub.value = new SignalREstacionNetCore(
    apiUrl,
    estacionExplosivos,
    nombreMetodoRecibirDatos,
  )

  const recibirDatosHub = (datosRemotos) => {
    if (!datosRemotos) return
    if (Object.keys(datosRemotos.datos).length === 0) {
      if (!datosEstacion.value.idEstacion) return
      if (datosEstacion.value.idEstacion === datosRemotos.userId) {
        if (Object.keys(datosRemotos.datos).length === 0) {
          datosEstacion.value.tieneConexion = false
          datosEstacion.value.datosRemotos = []
        } else {
          datosEstacion.value.tieneConexion = true
          datosEstacion.value.datosRemotos = datosRemotos.datos
        }
      }
      return
    }
    if (!datosRemotos.userId) return
    if (datosEstacion.value.idEstacion === datosRemotos.userId) {
      datosEstacion.value.datosRemotos = datosRemotos.datos
      datosEstacion.value.tieneConexion = true
      return
    }
    if (!datosRemotos.userId.includes('Optix-')) return
    let ipEstacion = datosRemotos.userId.replace('Optix-', '')
    if (datosEstacion.value.ipEstacion === ipEstacion) {
      datosEstacion.value.tieneConexion = true
      datosEstacion.value.idEstacion = datosRemotos.userId
      datosEstacion.value.datosRemotos = datosRemotos.datos
    }
  }

  const cambiarEstadoEstacion = (estado) => {
    console.log(estado)
  }

  almacenamientoHub.value.configurarConexionHub(recibirDatosHub, cambiarEstadoEstacion)
}

const refrescarTanques = async () => {
  const filtro = {
    estacion: estacion.value.nombre,
  }

  cargandoTanques.value = true
  const resp = await estatusAlmacenamiento.listarTanquesEstatus(filtro)

  if (!resp.exito) {
    listaTanques.value = []
    cargandoTanques.value = false
    return
  }

  listaTanques.value = resp.payload.respuesta

  let ordenTanquesLocalStorage = JSON.parse(
    localStorage.getItem(`ordenTanquesAlmacenamiento${estacion.value.nombre}`),
  )

  if (ordenTanquesLocalStorage) {
    listaTanques.value = ordenarTanquesConRespectoA(ordenTanquesLocalStorage, listaTanques.value)
  }
  cargandoTanques.value = false
}

const obtenerDatoRemotoDeposito = (deposito) => {
  const datoDepositoRemoto = datosEstacion.value.datosRemotos.find((d) => {
    if (!d.datoDeposito) return null

    const resultadoComparacion =
      d.datoDeposito.nombre.toUpperCase() === deposito.nombre.toUpperCase()

    return resultadoComparacion
  })

  return datoDepositoRemoto
}

const movimientoTanque = () => {
  negarOrdenamientos()
}

const terminarMovimiento = () => {
  guardarOrdenTanques()
}

const ordenarTanquesConRespectoA = (arrayOrden, arrayAOrdenar) => {
  let indexMap = new Map()
  arrayOrden.forEach((element, index) => {
    indexMap.set(element, index)
  })

  arrayAOrdenar.sort((a, b) => {
    let indexA = indexMap.get(a.idUnico)
    let indexB = indexMap.get(b.idUnico)

    return indexA - indexB
  })

  return arrayAOrdenar
}

const guardarOrdenTanques = () => {
  let ordenTanquesAlmacenamiento = listaTanques.value.map((x) => x.idUnico)
  localStorage.setItem(
    `ordenTanquesAlmacenamiento${estacion.value.nombre}`,
    JSON.stringify(ordenTanquesAlmacenamiento),
  )
}

const negarOrdenamientos = () => {
  estaOrdenadoTanquesMayorMenor.value = false
  estaOrdenadoTanquesMenorMayor.value = false
  estaOrdenadoEstacionesAaZ.value = false
  estaOrdenadoEstacionesZaA.value = false
  estaOrdenadoEstadosConectado.value = false
  estaOrdenadoEstadosDesconectado.value = false
  localStorage.removeItem('tipoOrdenamientoAlmacenamiento')
}
</script>

<style lang="scss" scoped></style>
