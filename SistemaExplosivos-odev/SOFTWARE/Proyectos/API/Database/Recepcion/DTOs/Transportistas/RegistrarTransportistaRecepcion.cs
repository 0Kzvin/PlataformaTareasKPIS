using API.Database.Recepcion.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Database.Recepcion.DTOs.Transportistas
{
    public class RegistrarTransportistaRecepcion
    {
        public int IdProveedor { get; set; }

        public string Nombre { get; set; }

        public string Apodo { get; set; }
    }
}
