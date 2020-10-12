using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GestorInfracciones.Models
{
    public class ConductorHabitual
    {
        [Required]
        public string DNI { get; set; }
        [Required]
        public string Matricula { get; set; }
    }
}