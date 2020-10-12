using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GestorInfracciones.Models
{
    public class Conductor
    {
        public Conductor()
        {
           
        }
        [Required]
        public string DNI { get; set; }
        [Required]
        public string NombreApellidos { get; set; }
        [Required]
        public int Puntos { get; set; }

    }
}