import { HubConnectionBuilder } from '@microsoft/signalr'
import { obtenerDireccionAPI } from 'src/services/AxiosService.js'

// Get base URL without /api suffix for SignalR
const baseUrl = obtenerDireccionAPI().replace('/api', '')

export const connection = new HubConnectionBuilder()
    .withUrl(`${baseUrl}/hubs/notificaciones`, {
        accessTokenFactory: () => {
            // Get token from localStorage
            const tokenInfo = localStorage.getItem('tokenInfo')
            if (tokenInfo) {
                const parsed = JSON.parse(tokenInfo)
                return parsed.token || ''
            }
            return ''
        }
    })
    .withAutomaticReconnect()
    .build()

export async function iniciarSignalR() {
    try {
        await connection.start()
        console.log('SignalR Conectado')
    } catch (err) {
        console.error('Error SignalR', err)
        setTimeout(iniciarSignalR, 5000)
    }
}
