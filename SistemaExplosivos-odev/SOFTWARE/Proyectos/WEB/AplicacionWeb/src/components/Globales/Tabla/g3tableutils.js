export const tieneLaPropiedadYValor = (lista, propiedad) => {
  // Verificar si la propiedad existe en la lista
  if (Object.prototype.hasOwnProperty.call(lista, propiedad)) {
    // Devolver el valor booleano de la propiedad
    return lista[propiedad]
  } else {
    // Devolver un valor predeterminado (por ejemplo, false) si la propiedad no existe
    return false
  }
}

export const obtenerTipoVariable = (valor, tipoVariable) => {
  const tipo = tipoVariable ? tipoVariable : typeof valor

  if (tipo === 'object') {
    if (valor === null) {
      return 'null'
    }

    if (valor instanceof Date || esFechaISO8601(valor)) {
      return 'Date'
    }

    if (Array.isArray(valor)) {
      return 'Array'
    }

    return 'Object'
  }

  if (tipo === 'string' && esFechaISO8601(valor)) {
    return 'Date'
  }

  return tipo
}

export const esFechaISO8601 = (valor) => {
  const formatoFechaISO8601 =
    /^\d{4}-(?:0[1-9]|1[0-2])-(?:[0-2][1-9]|[1-3]0|3[01])T(?:[0-1][0-9]|2[0-3])(?::[0-6]\d)(?::[0-6]\d)?(?:\.\d{3})?(?:[+-][0-2]\d:[0-5]\d|Z)?$/
  return formatoFechaISO8601.test(valor) && !isNaN(Date.parse(valor))
}

export const formatearValor = (valor, lenguaje, tipoVariable) => {
  const tipo = tipoVariable ? tipoVariable : obtenerTipoVariable(valor)

  switch (tipo) {
    case 'number':
      return formatearNumero(valor)
    case 'dinero':
      return formatearDinero(valor)
    case 'porcentaje':
      return formatearPorcentaje(valor)
    case 'color':
      return valor
    case 'string':
      return valor
    case 'imagen':
      return valor
    case 'boolean':
      return valor.toString()
    case 'Date':
      return formatearFecha(valor, lenguaje)
    case 'Array':
      return valor
    case 'Object':
      if (Array.isArray(valor)) {
        return JSON.stringify(valor)
      }
      return formatearObjeto(valor)
    case 'null':
      return 'null'
    case 'undefined':
      return 'undefined'
    case 'symbol':
      return valor.toString()
    case 'function':
      return 'function'
    default:
      return 'Tipo no reconocido'
  }
}

export const formatearNumero = (numero, decimales) => {
  const cantidadDecimales = decimales ? decimales : contarDecimales(numero)
  const numeroFormateado = numero.toLocaleString(undefined, {
    minimumFractionDigits: cantidadDecimales,
    maximumFractionDigits: cantidadDecimales,
  })
  return numeroFormateado
}

export const formatearDinero = (numero, decimales) => {
  const cantidadDecimales = decimales ? decimales : contarDecimales(numero)

  const USDollar = new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: 'USD',
    minimumFractionDigits: cantidadDecimales,
    maximumFractionDigits: cantidadDecimales,
  })

  const numeroFormateado = USDollar.format(numero)

  return numeroFormateado
}

export const formatearPorcentaje = (numero, decimales) => {
  const nuevoNumero = numero / 100
  const cantidadDecimales = decimales ? decimales : contarDecimales(numero)

  const numeroFormateado = Number(nuevoNumero).toLocaleString(undefined, {
    style: 'percent',
    minimumFractionDigits: cantidadDecimales,
    maximumFractionDigits: cantidadDecimales,
  })

  return numeroFormateado
}

export const contarDecimales = (numero) => {
  const partes = numero.toString().split('.')
  if (partes.length > 1) {
    return partes[1].length
  }
  return 0
}

export const formatearFecha = (fecha, lenguaje) => {
  if (typeof fecha === 'string') {
    fecha = new Date(fecha)
  }

  return fecha.toLocaleString(lenguaje, {
    day: 'numeric',
    month: 'numeric',
    year: 'numeric',
    hour: 'numeric',
    minute: 'numeric',
    second: 'numeric',
    hour12: true,
  })
}

export const formatearObjeto = (objeto) => {
  return JSON.stringify(objeto)
}

// Función para obtener la suma de todos los números en la lista
export const obtenerSumaTotales = (listaValores) => {
  return listaValores.reduce((acc, num) => acc + num, 0)
}

// Función para obtener el promedio de los números en la lista
export const obtenerPromedio = (listaValores) => {
  const suma = obtenerSumaTotales(listaValores)
  return suma / listaValores.length
}

// Función para obtener el mayor número en la lista
export const obtenerNumeroMayor = (listaValores) => {
  return Math.max(...listaValores)
}

// Función para obtener el menor número en la lista
export const obtenerNumeroMenor = (listaValores) => {
  return Math.min(...listaValores)
}

// Función para obtener la mediana de la lista
export const obtenerMediana = (listaValores) => {
  const listaOrdenada = listaValores.slice().sort((a, b) => a - b)
  const mitad = Math.floor(listaOrdenada.length / 2)

  if (listaOrdenada.length % 2 === 0) {
    return (listaOrdenada[mitad - 1] + listaOrdenada[mitad]) / 2
  } else {
    return listaOrdenada[mitad]
  }
}

// Función para obtener la moda de la lista
export const obtenerModa = (listaValores) => {
  const frecuencias = {}

  listaValores.forEach((valor) => {
    frecuencias[valor] = (frecuencias[valor] || 0) + 1
  })

  let moda = []
  let maxFrecuencia = 0

  for (const valor in frecuencias) {
    if (frecuencias[valor] > maxFrecuencia) {
      moda = [valor]
      maxFrecuencia = frecuencias[valor]
    } else if (frecuencias[valor] === maxFrecuencia) {
      moda.push(valor)
    }
  }

  return moda
}

// Función para obtener el rango (diferencia entre el número mayor y menor) de la lista
export const obtenerRango = (listaValores) => {
  const mayor = obtenerNumeroMayor(listaValores)
  const menor = obtenerNumeroMenor(listaValores)
  return mayor - menor
}

// Función para obtener la varianza de la lista
export const obtenerVarianza = (listaValores) => {
  const promedio = obtenerPromedio(listaValores)
  const sumaCuadradosDiferenciaPromedio = listaValores.reduce(
    (acc, num) => acc + Math.pow(num - promedio, 2),
    0,
  )
  return sumaCuadradosDiferenciaPromedio / listaValores.length
}

// Función para obtener la desviación estándar de la lista
export const obtenerDesviacionEstandar = (listaValores) => {
  const varianza = obtenerVarianza(listaValores)
  return Math.sqrt(varianza)
}

// Función para obtener la productoria de todos los números en la lista
export const obtenerProductoria = (listaValores) => {
  return listaValores.reduce((acc, num) => acc * num, 1)
}

// Función para obtener la suma de los cuadrados de todos los números en la lista
export const obtenerSumaCuadrados = (listaValores) => {
  return listaValores.reduce((acc, num) => acc + Math.pow(num, 2), 0)
}
