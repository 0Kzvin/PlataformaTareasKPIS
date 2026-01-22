import { HubConnectionBuilder, HttpTransportType } from '@microsoft/signalr'
import { useSesionStore } from '../../stores/sesion'

const sesionStore = useSesionStore()
const token = sesionStore.tokenInfo.token

class ConexionSignalR {
  /**
   * Constructor de la clase ConexionSignalR.
   * @param {string} ipEstacion - La dirección IP de la estación a la que se conectará.
   * @param {string} nombreHub - El nombre del hub de SignalR al que se conectará.
   * @param {string} nombreMetodoRecibirDatos - El nombre del método que se usará para recibir datos del hub.
   */
  constructor(ipEstacion, nombreHub, nombreMetodoRecibirDatos) {
    this.ipEstacion = ipEstacion
    this.nombreHub = nombreHub
    this.nombreMetodoRecibirDatos = nombreMetodoRecibirDatos
    this.conexionSignalR = null
    this.manejadorCambioEstatusConexion = () => {}
    this.manejadorConexionExitosa = () => {}
    this.idTemporizadorReconexion = null
    this.reconexionActiva = false // Bandera para controlar la reconexión automática
  }

  /**
   * Configura la conexión con el hub de SignalR.
   * @param {Function} manejadorMensajeRecibido - Función que se ejecutará cuando se reciba un mensaje del hub.
   * @param {Function} manejadorCambioEstatusConexion - Función que se ejecutará cuando cambie el estatus de la conexión.
   * @param {Function} manejadorConexionExitosa - Función que se ejecutará cuando se conecte al hub.
   */
  configurarConexionHub(
    manejadorMensajeRecibido,
    manejadorCambioEstatusConexion,
    manejadorConexionExitosa,
  ) {
    this.conexionSignalR = new HubConnectionBuilder()
      .withUrl(`${this.ipEstacion}/${this.nombreHub}`, {
        accessTokenFactory: () => token,
        transport: HttpTransportType.WebSockets | HttpTransportType.LongPolling,
      })
      .withAutomaticReconnect()
      .build()

    this.manejadorCambioEstatusConexion = manejadorCambioEstatusConexion
    this.manejadorConexionExitosa = manejadorConexionExitosa

    // Configura el manejador para recibir mensajes del hub
    this.conexionSignalR.on(this.nombreMetodoRecibirDatos, manejadorMensajeRecibido)

    // Configura los manejadores para los eventos de reconexión y cierre
    this.conexionSignalR.onreconnecting(() => {
      if (this.manejadorCambioEstatusConexion) this.manejadorCambioEstatusConexion(false)
    })

    this.conexionSignalR.onreconnected(() => {
      if (this.manejadorCambioEstatusConexion) this.manejadorCambioEstatusConexion(true)
      if (this.manejadorConexionExitosa) this.manejadorConexionExitosa()
    })

    this.conexionSignalR.onclose(() => {
      if (this.manejadorCambioEstatusConexion) this.manejadorCambioEstatusConexion(false)
      if (this.reconexionActiva) {
        this.idTemporizadorReconexion = setTimeout(() => this.iniciarConexionHub(), 5000)
      }
    })

    this.reconexionActiva = true // Activa la reconexión automática
    this.iniciarConexionHub()
  }

  /**
   * Inicia la conexión con el hub de SignalR.
   * Si la conexión falla, se intentará reconectar después de 5 segundos.
   */
  async iniciarConexionHub() {
    try {
      if (this.conexionSignalR && this.conexionSignalR.state === 'Disconnected') {
        await this.conexionSignalR.start()
        if (this.manejadorCambioEstatusConexion) this.manejadorCambioEstatusConexion(true)
        if (this.manejadorConexionExitosa) this.manejadorConexionExitosa()
      }
    } catch {
      if (this.manejadorCambioEstatusConexion) this.manejadorCambioEstatusConexion(false)
      if (this.reconexionActiva) {
        this.idTemporizadorReconexion = setTimeout(() => this.iniciarConexionHub(), 5000)
      }
    }
  }

  /**
   * Desconecta la conexión con el hub de SignalR.
   * También detiene cualquier intento de reconexión automática.
   */
  async desconectarConexionHub() {
    try {
      this.reconexionActiva = false // Desactiva la reconexión automática
      clearTimeout(this.idTemporizadorReconexion) // Cancela el temporizador de reconexión
      if (this.conexionSignalR) {
        this.conexionSignalR.off(this.nombreMetodoRecibirDatos) // Elimina el manejador de mensajes
        await this.conexionSignalR.stop() // Detiene la conexión

        if (this.manejadorCambioEstatusConexion) this.manejadorCambioEstatusConexion(false)
      }
    } catch {
      if (this.manejadorCambioEstatusConexion) this.manejadorCambioEstatusConexion(false)
    }
  }
}

export default ConexionSignalR
