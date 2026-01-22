
import { traducir } from "src/services/TranslationService.js";
import { date } from "quasar";
import axios from 'axios';

export let source = null;

export const refreshSource = () => {
  source = axios.CancelToken.source();
}

export const getFileAsByteArray = async (file) => {
  return new Uint8Array(await readFile(file));
};

export const getFileAsNumberArray = async (file) => {
  const byteArray = new Uint8Array(await readFile(file));
  const numberArray = Array.from(byteArray);
  return numberArray;
};

const readFile = async (file) => {
  return new Promise((resolve, reject) => {
    // Create file reader
    let reader = new FileReader();

    // Register event listeners
    reader.addEventListener("loadend", (e) => resolve(e.target.result));
    reader.addEventListener("error", reject);

    // Read file
    reader.readAsArrayBuffer(file);
  });
};

//Separa un string de numeros por commas
export function stringNumerosConCommas(string, modo = 1) {
  if (modo == 1) {
    var cadena =  string.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    var cadenas = cadena.split('.');
    if(cadenas[1] != null && cadenas[1] != undefined){

      // var decimalSinComas = cadenas[1].replace(',','');
      var decimalSinComas = ((cadenas[1][0]) ? cadenas[1][0] : "") + ((cadenas[1][1]) ? cadenas[1][1] : "");
      return cadenas[0]+'.'+decimalSinComas;
    }
    else return cadena;
  }

  if (modo == 2) {
    return string.toString().replace(/B(?=(\d{3})+(?!d))/g, ",");
  }
}

//Retorna el numero de semana actual en formato ISO
export const obtenerSemanaISO = async function () {
  var date = new Date();
  date.setHours(0, 0, 0, 0);
  // Thursday in current week decides the year.
  date.setDate(date.getDate() + 3 - ((date.getDay() + 6) % 7));
  // January 4 is always in week 1.
  var week1 = new Date(date.getFullYear(), 0, 4);
  // Adjust to Thursday in week 1 and count number of weeks from date to week1.
  return (
    1 +
    Math.round(
      ((date.getTime() - week1.getTime()) / 86400000 -
        3 +
        ((week1.getDay() + 6) % 7)) /
      7
    )
  );
};

export const obtenerSemanaAnioISO = async function () {
  const date = new Date();
  date.setHours(0, 0, 0, 0);

  // Ajusta el día al primer jueves del año, siempre es en la semana 1
  const dayOfWeek = date.getDay() || 7;
  date.setDate(date.getDate() + 4 - dayOfWeek);

  // Obtiene el año de la fecha ajustada (si es una semana partida, toma la del año nuevo)
  let year = date.getFullYear();

  // Obtiene el primer dia del año
  const yearStart = new Date(year, 0, 1);

  // Calcula el número de semana
  const weekNumber = Math.ceil((((date - yearStart) / 86400000) + 1) / 7);

  // Maneja los casos donde las semanas pertenecen al siguiente o al año anterior
  if (weekNumber === 1 && date.getMonth() === 11) {
    // Si el numero de semana es 1 pero el mes es diciembre, entonces debe de tomar el año nuevo
    year += 1;
  } else if (weekNumber >= 52 && date.getMonth() === 0) {
    //Si el numero de semana es 52 o mayor, entonces el año debe de ser el anterior
    year -= 1;
  }

  return { weekNumber, year };
};

export const obtenerSemanaISONoAsync = function () {
  var date = new Date();
  date.setHours(0, 0, 0, 0);
  // Thursday in current week decides the year.
  date.setDate(date.getDate() + 3 - ((date.getDay() + 6) % 7));
  // January 4 is always in week 1.
  var week1 = new Date(date.getFullYear(), 0, 4);
  // Adjust to Thursday in week 1 and count number of weeks from date to week1.
  return (
    1 +
    Math.round(
      ((date.getTime() - week1.getTime()) / 86400000 -
        3 +
        ((week1.getDay() + 6) % 7)) /
      7
    )
  );
};

//Retorna la Fecha ISO en donde la semana comienza en Domingo
export function obtenerFechaDeSemana(semana, anio) {
  let dia = 1 + (semana - 1) * 7;
  // var dia = 1 + (semana) * 7;
  // let dia = (semana) * 7;
  return new Date(anio, 0, dia);

  //Si quieres obtener el lunes
  // var lunes = new Date(
  //     diaSemana.setDate(diaSemana.getDate() - diaSemana.getDay() + 1)
  //   );
}

//Se utiliza para saber si las # de Semanas que tiene un anio
//e identificar las que tienen 52 o 53
export function obtenerNumeroSemanasEnAnio(anio) {
  const d = new Date(anio, 11, 31);
  const week = obtenerNumeroSemanaAPartirDeFecha(d);
  const numeroSemanasEnAnio = week == 1 ? 52 : week;

  return numeroSemanasEnAnio;
}

//Se utiliza para la funcion obtenerNumeroSemanasEnAnio de Utils
export function obtenerNumeroSemanaAPartirDeFecha(d) {
  d = new Date(+d);
  d.setHours(0, 0, 0, 0);
  d.setDate(d.getDate() + 4 - (d.getDay() || 7));
  var yearStart = new Date(d.getFullYear(), 0, 1);
  var weekNo = Math.ceil((((d - yearStart) / 86400000) + 1) / 7)
  return weekNo;
}

// Toma un array y el string de un objeto para buscar duplicados
// En caso de no especificar el nombre de la propiedad, busca por variables primitivas
// Si encuentra un duplicado regreso el primer index duplicado
// No encuentra un duplicado, regresa -1
export function obtenerIndexDuplicadoEnArray(array, nombrePropiedad) {
  if(nombrePropiedad){
    var tmpArr = [];
    for(var obj in array) {
      if(tmpArr.indexOf(array[obj][nombrePropiedad]) < 0){
        tmpArr.push(array[obj][nombrePropiedad]);
      } else {
        return array.findIndex(e => e[nombrePropiedad] == tmpArr[tmpArr.length]);
      }
    }
    return -1;
  }
}

export const esStringVacio = async (string) => {
  return !string || 0 === string.length;
}

export const groupBy = (arr, key) => {
  const initialValue = {};
  return arr.reduce((acc, cval) => {
    const myAttribute = cval[key];
    acc[myAttribute] = [...(acc[myAttribute] || []), cval]
    return acc;
  }, initialValue);
};
export const horasEnElDia = [
  { horaVisualizar: "12:00 AM", hora:  0, },
  { horaVisualizar: "01:00 AM", hora:  1, },
  { horaVisualizar: "02:00 AM", hora:  2, },
  { horaVisualizar: "03:00 AM", hora:  3, },
  { horaVisualizar: "04:00 AM", hora:  4, },
  { horaVisualizar: "05:00 AM", hora:  5, },
  { horaVisualizar: "06:00 AM", hora:  6, },
  { horaVisualizar: "07:00 AM", hora:  7, },
  { horaVisualizar: "08:00 AM", hora:  8, },
  { horaVisualizar: "09:00 AM", hora:  9, },
  { horaVisualizar: "10:00 AM", hora:  10, },
  { horaVisualizar: "11:00 AM", hora:  11, },
  { horaVisualizar: "12:00 PM", hora:  12, },
  { horaVisualizar: "01:00 PM", hora:  13, },
  { horaVisualizar: "02:00 PM", hora:  14, },
  { horaVisualizar: "03:00 PM", hora:  15, },
  { horaVisualizar: "04:00 PM", hora:  16, },
  { horaVisualizar: "05:00 PM", hora:  17, },
  { horaVisualizar: "06:00 PM", hora:  18, },
  { horaVisualizar: "07:00 PM", hora:  19, },
  { horaVisualizar: "08:00 PM", hora:  20, },
  { horaVisualizar: "09:00 PM", hora:  21, },
  { horaVisualizar: "10:00 PM", hora:  22, },
  { horaVisualizar: "11:00 PM", hora:  23, },
];

export function obtenerNombreMesAnio(mes) {
  const nombreMeses = [
    traducir("Enero"),
    traducir("Febrero"),
    traducir("Marzo"),
    traducir("Abril"),
    traducir("Mayo"),
    traducir("Junio"),
    traducir("Julio"),
    traducir("Agosto"),
    traducir("Septiembre"),
    traducir("Octubre"),
    traducir("Noviembre"),
    traducir("Diciembre"),
  ];
  return nombreMeses[mes];
}
export function obtenerDiaSemana(dia) {
  const nombreDia = [
    traducir("Domingo"),
  traducir("Lunes"),
  traducir("Martes"),
  traducir("Miercoles"),
  traducir("Jueves"),
  traducir("Viernes"),
  traducir("Sabado"),
  ];
  return nombreDia[dia];
}

export function obtenerFechaNormalizada(fecha) {
  return date.formatDate(fecha, "DD MMMM YYYY", {
    days: [
      traducir("Domingo"),
      traducir("Lunes"),
      traducir("Martes"),
      traducir("Miercoles"),
      traducir("Jueves"),
      traducir("Viernes"),
      traducir("Sabado"),
    ],
    daysShort: [
      traducir("DomingoCorto"),
      traducir("LunesCorto"),
      traducir("MartesCorto"),
      traducir("MiercolesCorto"),
      traducir("JuevesCorto"),
      traducir("ViernesCorto"),
      traducir("SabadoCorto"),
    ],
    months: [
      traducir("Enero"),
      traducir("Febrero"),
      traducir("Marzo"),
      traducir("Abril"),
      traducir("Mayo"),
      traducir("Junio"),
      traducir("Julio"),
      traducir("Agosto"),
      traducir("Septiembre"),
      traducir("Octubre"),
      traducir("Noviembre"),
      traducir("Diciembre"),
    ],
    monthsShort: [
      traducir("EneroCortos"),
      traducir("FebreroCortos"),
      traducir("MarzoCortos"),
      traducir("AbrilCortos"),
      traducir("MayoCortos"),
      traducir("JunioCortos"),
      traducir("JulioCortos"),
      traducir("AgostoCortos"),
      traducir("SeptiembreCortos"),
      traducir("OctubreCortos"),
      traducir("NoviembreCortos"),
      traducir("DiciembreCortos"),
    ],
  });
}

export const objectToFormData = (object) => {
  const form = new FormData();
  for (const key in object) {
    form.append(key, object[key]);
  }
  return form;
}

export function esFechaMayorALaActual(dia, mes, anio) {

  const ahora = new Date();
  const diaActual = ahora.getDate();
  const mesActual = ahora.getMonth() + 1;
  const anioActual = ahora.getFullYear();

  let laFechaEsMayor = false;

  if(anioActual < anio){
   laFechaEsMayor = true;
  }else if(anioActual == anio){
    if(mesActual < mes){
      laFechaEsMayor = true;
    }else if(mesActual == mes){
      if(diaActual <= dia){
        laFechaEsMayor = true;
      }
    }
  }
  return laFechaEsMayor;
}

/// Obtiene la diferencia de horas entre dos DateTime
// MODO 1: HORAS
// MODO 2: MINUTOS
export function obtenerDifEntreDates(dateTimeMayor, dateTimeMenor, modo = 1)
 {
  var diff = 0;
  if(modo == 1) {
    diff =(dateTimeMayor.getTime() - dateTimeMenor.getTime()) / 1000;
    diff /= (60 * 60);
  }
  if(modo == 2) {
    diff =(dateTimeMayor.getTime() - dateTimeMenor.getTime()) / 1000;
    diff /= 60;
  }
  return Math.abs(Math.round(diff));

 }

export function formatNumberWithDecimals(number, minimumIntegerDigits = 2, useGrouping = false, minimumFractionDigits = 2, maximumFractionDigits = 2 ) {
  return number.toLocaleString('en-US', { minimumIntegerDigits: minimumIntegerDigits, useGrouping: useGrouping, minimumFractionDigits: minimumFractionDigits, maximumFractionDigits: maximumFractionDigits });
}

export function debounce (fn, delay){
  let timeout

  return (...args) => {
    if (timeout) {
      clearTimeout(timeout)
    }

    timeout = setTimeout(() => {
      fn(...args)
    }, delay)
  }
}

//Unidades que utilizan los Tanques según el cliente
export const unidadesClienteTanque = {
  milimetros: "mm",
  metros: "Mts"
}

/**
* Función que formatea un número agregando o recortando dígitos decimales según un tamaño específico. formatearNumeroLongitudDecimal
*
* @param {number} valor - El número a formatear.
* @param {number} tamañoDecimales - La cantidad de dígitos decimales deseados.
* @returns {string|null} El número formateado con la cantidad de decimales especificada, o null si el valor es falso.
*/
export function formatNumberDecimalLength(valor, tamañoDecimales) {
  // Si el valor es falso, devuelve null
  if (!valor) return null;

  // Divide el número en parte entera y parte decimal
  const arregloSeparado = valor.toString().split('.');
  const parteEntera = arregloSeparado[0];
  const parteDecimal = arregloSeparado[1] || '';

  // Si la parte decimal es más corta que el tamaño deseado, agrega ceros al final
  if (parteDecimal.length < tamañoDecimales) {
    return `${parteEntera}.${parteDecimal}${'0'.repeat(tamañoDecimales - parteDecimal.length)}`;
  }
  // Si la parte decimal es más larga que el tamaño deseado, recorta los dígitos sobrantes
  else if (parteDecimal.length > tamañoDecimales) {
    return `${parteEntera}.${parteDecimal.substring(0, tamañoDecimales)}`;
  }
  // Si la parte decimal tiene la longitud deseada, devuelve el número sin cambios
  else {
    return valor.toString();
  }
 }

export function obtenerObjetoHora(datosCliente) {
  return horasEnElDia.find(x => x.hora == datosCliente.horaCorte);
}

export function obtenerColorAceite(productoAceite) {
  switch(productoAceite){
    case "ACEITE USADO":
      return '#8B4513';
    case "REFRIGERANTE NUEVO":
      return '#32CD32';
    case "REFRIGERANTE USADO":
      return '#32CD32';
    case "TRANSMISION":
      return '#FF7F00';
    case "HIDRAULICO":
      return '#FFA500';
    case "MOTOR":
      return '#B22222';
    case "PARA EJES":
      return '#FF6347';
    default:
      return '#b3dd1c';
  }
}

export default {
  groupBy,
  horasEnElDia,
  stringNumerosConCommas,
  obtenerDifEntreDates,
  formatNumberWithDecimals,
  source,
  refreshSource,
  debounce,
  obtenerObjetoHora,
  unidadesClienteTanque,
}

