using API.Utilidades.Constantes;
using System;

namespace API.Atributos
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class OperacionTotalizacionAttribute : Attribute
    {
        public TipoTotalizacion Tipo { get; }

        public OperacionTotalizacionAttribute(TipoTotalizacion tipo)
        {
            Tipo = tipo;
        }
    }
}
