const TAMANIO_DEFAULT = "md"

const opcionesTextos = [
  { llave: "TextoChico", label: "Pequeño", value: "sm", size: "0.8" },
  {
    llave: "TextoMediano",
    label: "Medio (Preterminado)",
    value: "md",
    size: "1.0",
  },
  { llave: "TextoLargo", label: "Largo", value: "lg", size: "1.2" },
  {
    llave: "TextoExtraLargo",
    label: "Extra Largo",
    value: "xl",
    size: "1.4",
  },
];

const obtenerTextSize = () => {
  return localStorage.getItem("userTextSize") ?? TAMANIO_DEFAULT;
}

const obtenerOpcionTextSize = () => {
  return opcionesTextos.find((opcion) => opcion.value === obtenerTextSize());
}

const cambiarTextSize = () => {
  document.body.setAttribute("datos-tamano-texto", obtenerTextSize());
}

const guardarTextSize = (textSize = TAMANIO_DEFAULT) => {
  localStorage.setItem("userTextSize", textSize);
  cambiarTextSize();
}

export {
  cambiarTextSize,
  guardarTextSize,
  opcionesTextos,
  obtenerOpcionTextSize,
  obtenerTextSize
}
