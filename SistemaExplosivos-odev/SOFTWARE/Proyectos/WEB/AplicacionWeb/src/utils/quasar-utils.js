import { Notify, Dialog, Loading, QSpinnerGears, getCssVar } from 'quasar'
import { traducir } from '../services/TranslationService.js'
import Swal from 'sweetalert2'
import CustomAvisoDialog from 'src/components/Globales/Dialogs/CustomAvisoDialog.vue'

// Helper para obtener el color del tema actual
const obtenerColorTema = (nombreColor) => {
  return getCssVar(nombreColor) || nombreColor
}

export const mostrarSnackbar = ({
  mensaje = '',
  icono = 'report_problem',
  posicion = 'center',
  timeout = 2000,
  bgColor = 'secondary',
  textColor = 'white',
}) => {
  Notify.create({
    color: bgColor,
    textColor: textColor,
    icon: icono,
    message: traducir(mensaje),
    position: posicion,
    timeout,
  })
}

export const aviso = async ({
  exito = false,
  error = false,
  icono = '',
  titulo = '',
  mensaje = '',
}) => {
  if (error && !icono) {
    icono = 'error'
  }
  if (exito && !icono) {
    icono = 'success'
  }
  await Swal.fire({
    title: titulo,
    text: mensaje,
    icon: icono,
    allowEnterKey: true,
    keydownListenerCapture: true,
    showConfirmButton: true,
    confirmButtonText: traducir('Enterado').toUpperCase(),
    customClass: {
      confirmButton: 'bg-primary text-bold text-primary-contrast',
      popup: 'bg-fondo2 text-textsecondary',
    },
    didOpen: () => {
      const popup = Swal.getPopup()
      if (popup) {
        popup.focus()
      }
    },
  })
  return
}

export const decision = async ({
  icono = 'question',
  iconoColor = null,
  titulo = '',
  html = null,
  mensaje = '',
  checkbox = false,
  checkBoxLabel = '',
  checkBoxErr = '',
  mostrarCancelar = true,
}) => {
  // Resolver el color del tema si es un nombre de color
  const colorResuelto = iconoColor ? obtenerColorTema(iconoColor) : undefined

  const resultado = await Swal.fire({
    title: titulo,
    text: mensaje,
    html: html,
    icon: icono,
    iconColor: colorResuelto,
    showConfirmButton: true,
    reverseButtons: true,
    allowEnterKey: true,
    confirmButtonText: `<i class="fa-solid fa-check"></i> ${traducir('Confirmar').toUpperCase()}`,
    cancelButtonText: `<i class="fa-solid fa-xmark"></i> ${traducir('Cancelar').toUpperCase()}`,
    keydownListenerCapture: true,
    showCancelButton: mostrarCancelar,
    customClass: {
      confirmButton: 'bg-primary text-bold text-primary-contrast',
      cancelButton: 'bg-negative text-bold text-negative-contrast',
      popup: 'bg-fondo3 text-textsecondary',
    },
    input: checkbox ? 'checkbox' : '',
    inputValue: false,
    inputPlaceholder: checkBoxLabel,
    inputValidator: (result) => {
      return !result && checkBoxErr
    },
    didOpen: () => {
      var doc = document.getElementsByClassName('swal2-container')[0]
      if (doc) {
        doc.focus()
        doc.addEventListener('keypress', function (e) {
          if (e.key === 'Enter') {
            Swal.clickConfirm()
          }
        })
      }
    },
  })
  return resultado.isConfirmed
}

export const cargandoSimple = ({
  mensaje = `${traducir('Cargando')}...`,
  colorMensaje = 'textlight',
  spinnerColor = 'primary',
  fondoColor = 'bg-light',
} = {}) => {
  Loading.show({
    message: mensaje,
    messageColor: colorMensaje,
    spinnerColor: spinnerColor,
    spinner: QSpinnerGears,
    boxClass: fondoColor,
  })
}

export const ocultarCargandoSimple = () => Loading.hide()

export const mostrarAvisoCustomDialog = ({
  operacionExitosa = true,
  mensaje = '',
  titulo = '',
}) => {
  var icono = 'check'
  if (!operacionExitosa) {
    icono = 'trash'
  }
  Dialog.create({
    component: CustomAvisoDialog,
    componentProps: {
      mensaje,
      titulo,
      icono,
    },
  })
}
