using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GestorInfracciones.Models
{
    public class Infraccion
    {
        /// <summary>
        /// Identificador único. Se genera automáticamente
        /// </summary>
        public string Identificador { get; set; }
        [Required]
        public System.DateTime Fecha { get; set; }
        [Required]
        public string TipoInfraccionIdentificador { get; set; }
        [Required]
        public string Matricula { get; set; }
        public string DNI { get; set; } = null;
    }
}