using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class universitarios
    {
        [Required]
        public int IdEstudiante { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellidos { get; set; }
        [Required]
        
        public string Direccion { get; set; }
        [Required]
        public System.DateTime FechaIngreso { get; set; }
        
        [Required]
        public int Telefono { get; set; }
    }
}
