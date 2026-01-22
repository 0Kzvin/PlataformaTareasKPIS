import * as quasarUtils from 'src/utils/quasar-utils.js'

export const usePwaStore = defineStore('pwa', () => {
    const deferredPrompt = ref(null)
    const canInstall = ref(process.env.DEV || false)

    const captureEvent = (e) => {
        e.preventDefault()
        deferredPrompt.value = e
        canInstall.value = true
        console.log('PWA Install Event Captured')
    }

    const installApp = async () => {
        if (deferredPrompt.value) {
            deferredPrompt.value.prompt()
            const { outcome } = await deferredPrompt.value.userChoice
            console.log(`User response: ${outcome}`)
            deferredPrompt.value = null
            canInstall.value = false
        } else if (process.env.DEV) {
            const confirmed = await quasarUtils.decision({
                titulo: 'Instalar Aplicación',
                mensaje: '¿Estás seguro que deseas instalar la aplicación? (Simulación Dev)',
                icono: 'download'
            })

            if (confirmed) {
                quasarUtils.exito({
                    titulo: 'Instalación Iniciada',
                    mensaje: 'La aplicación se está instalando en segundo plano (Simulada).'
                })
            }
        }
    }

    return {
        deferredPrompt,
        canInstall,
        captureEvent,
        installApp
    }
})
