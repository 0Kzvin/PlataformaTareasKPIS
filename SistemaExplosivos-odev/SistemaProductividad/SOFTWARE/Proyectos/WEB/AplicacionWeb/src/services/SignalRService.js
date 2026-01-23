import { HubConnectionBuilder } from '@microsoft/signalr'
import { date } from 'quasar'
import { obtenerDireccionAPI } from 'src/services/AxiosService.js'
import { useSesionStore } from 'src/stores/sesion'

const baseUrl = obtenerDireccionAPI()

const obtenerTokenInfo = () => {
  const sesionStore = useSesionStore()
  if (sesionStore?.tokenInfo?.token) {
    return sesionStore.tokenInfo
  }
  const tokenPersistido = localStorage.getItem('tokenInfo')
  if (!tokenPersistido) {
    return null
  }
  try {
    return JSON.parse(tokenPersistido)
  } catch {
    return null
  }
}

const tokenExpirado = (tokenInfo) => {
  if (!tokenInfo?.expiracion) {
    return false
  }
  const segundosRestantes = date.getDateDiff(tokenInfo.expiracion, new Date(), 'seconds')
  return segundosRestantes <= 0
}

const obtenerTokenSesion = () => {
  return obtenerTokenInfo()?.token || ''
}

export const connection = new HubConnectionBuilder()
  .withUrl(`${baseUrl}/hubs/notificaciones`, {
    accessTokenFactory: () => obtenerTokenSesion(),
  })
  .withAutomaticReconnect()
  .build()

export async function iniciarSignalR() {
  try {
    const tokenInfo = obtenerTokenInfo()
    if (!tokenInfo?.token) {
      console.warn('SignalR no iniciado: token inexistente')
      return
    }
    if (tokenExpirado(tokenInfo)) {
      console.warn('SignalR no iniciado: token expirado')
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
