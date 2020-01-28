using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EscuelaWeb.Models
{
    public class Curso:ObjetoEscuelaBase
    {
        [Required(ErrorMessage = "El Nombre del curso es requerido")]
        [StringLength(8)]
        public override string Nombre { get; set; }
        public TiposJornada Jornada { get; set; }
        public List<Asignatura> Asignaturas{ get; set; }
        public List<Alumno> Alumnos{ get; set; }
        
        [Display(Prompt = "Direccion Correspondiente", Name = "Address")]
        [Required(ErrorMessage = "La Direccion es Requerida")]
        [MinLength(5, ErrorMessage = "La longitud requerida minima es de 5")]
        public string Dirección { get; set; }
        public string EscuelaId { get; set; }
        public Escuela Escuela { get; set; }
    }
}