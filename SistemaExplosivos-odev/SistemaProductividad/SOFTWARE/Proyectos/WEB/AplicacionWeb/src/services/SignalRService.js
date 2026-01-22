import { HubConnectionBuilder } from '@microsoft/signalr'
import { obtenerDireccionAPI } from 'src/services/AxiosService.js'
import { useSesionStore } from 'src/stores/sesion'

const baseUrl = obtenerDireccionAPI()

const obtenerTokenSesion = () => {
  const sesionStore = useSesionStore()
  return sesionStore?.tokenInfo?.token || ''
}

export const connection = new HubConnectionBuilder()
  .withUrl(`${baseUrl}/hubs/notificaciones`, {
    accessTokenFactory: () => obtenerTokenSesion(),
  })
  .withAutomaticReconnect()
  .build()

export async function iniciarSignalR() {
  try {
    const token = obtenerTokenSesion()
    if (!token) {
      console.warn('SignalR no iniciado: token inexistente')
      return
    }
    if (connection.state === 'Disconnected') {
      await connection.start()
      console.log('SignalR Conectado')
    }
  } catch (err) {
    console.error('Error SignalR', err)
    setTimeout(iniciarSignalR, 5000)
  }
}

export async function detenerSignalR() {
  try {
    if (connection.state === 'Connected') {
      await connection.stop()
      console.log('SignalR Desconectado')
    }
  } catch (err) {
    console.error('Error al detener SignalR', err)
  }
}
