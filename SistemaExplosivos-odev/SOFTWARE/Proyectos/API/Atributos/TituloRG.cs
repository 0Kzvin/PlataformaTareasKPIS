using System;

namespace API.Atributos
{
    [AttributeUsage(validOn: AttributeTargets.Property)]
    public class TituloRG : Attribute
    {
        private string nombre;
        private string nombrePropiedad;
        private Type tipoVariable;
        private int posicion;
        private bool ocultar;

        public TituloRG(string nombre)
        {
            this.nombre = nombre;
            this.ocultar = false;
            this.posicion = 0;
        }

        public virtual string Nombre

        {
            get { return nombre; }
        }

        public virtual string NombrePropiedad

        {
            get { return nombrePropiedad; }
            set { nombrePropiedad = value; }
        }

        public virtual Type TipoVariable

        {
            get { return tipoVariable; }
            set { tipoVariable = value; }

        }

        public virtual int Posicion
        {
            get { return posicion; }
            set { posicion = value; }

        }
        public virtual bool Ocultar
        {
            get { return ocultar; }
            set { ocultar = value; }

        }

    }
}
