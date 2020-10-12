using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GestorInfracciones.Models
{
    public class TipoInfraccion
    {
        [Required]
        public string Identificador { get; set; }
        [Required]
        public string Descripcion { get; set; }
        [Required]
        public int Puntos { get; set; }
    }
}