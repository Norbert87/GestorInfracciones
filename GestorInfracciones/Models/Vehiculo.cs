using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GestorInfracciones.Models
{
    public class Vehiculo
    {
        [Required]
        public string Matricula { get; set; }
        [Required]
        public string Marca { get; set; }
        [Required]
        public string Modelo { get; set; }
    }
}