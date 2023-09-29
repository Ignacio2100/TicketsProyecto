using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ticket.Models
{
    public class ClienteCLS
    {
        [Display(Name = "Codigo Cliente")]
        public int Id { get; set; }

        [Display(Name = "Nombres")]
        [StringLength(250, ErrorMessage = "La Longitud Maxima es de 250")]
        [Required]
        public string Nombre { get; set; }

        [Display(Name = "Apellidos")]
        [StringLength(250, ErrorMessage = "La Longitud Maxima es de 250")]
        [Required]
        public string Apellido { get; set; }

        [Display(Name = "Documento de Identificación")]
        [StringLength(13, ErrorMessage = "La Longitud Maxima es de 13")]
        [Required]
        public string Dpi { get; set; }


        [Display(Name = "Numero de Telefono")]
        [StringLength(8, ErrorMessage = "La Longitud Maxima es de 8")]
        [Required]
        public string Telefono { get; set; }

        [Display(Name = "Genero")]
        [Required]
        public int Genero { get; set; }

        [Display(Name = "Tipo de Problema")]
        [Required]
        public int ProcesoId { get; set; }

        //Adiccion para en listar
        public string NombreGenero { get; set; }

        //Adiccion para en listar
        public string TipoProblema { get; set; }
    }
}