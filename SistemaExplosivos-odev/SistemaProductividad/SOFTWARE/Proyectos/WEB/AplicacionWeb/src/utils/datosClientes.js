// ! Es muy importante que todos tengan el mismo numero de variables y con el mismo nombre el cual se debe de definir una sola vez y repetir igual
export const datosClientes = [
  {
    cliente: 'CANANEA',
    URLs: [
      'http://localhost:8082'
    ],
  },
  {
    cliente: 'NACOZARI',
    URLs: [
      'http://10.86.0.13',
    ],
  },
  {
    cliente: 'TEST',
    URLs: [
      'http://localhost:8082'
    ],
  },
]

export const obtenerDatosCliente = (nombreCliente) => {
  return datosClientes.find((x) => x.cliente == nombreCliente)
}

export default {
  datosClientes,
  obtenerDatosCliente,
}
