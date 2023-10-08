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


        [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
        [StringLength(50, ErrorMessage = "El campo Nombre debe tener como máximo 50 caracteres.")]
        [Display(Name = "Nombres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo Apellido es obligatorio.")]
        [StringLength(50, ErrorMessage = "El campo Apellido debe tener como máximo 50 caracteres.")]
        [Display(Name = "Apellidos")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El campo DPI es obligatorio.")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "El campo DPI debe tener exactamente 13 dígitos.")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "El campo DPI debe contener solo números.")]
        [Display(Name = "Documento de Identificación")]
        public string Dpi { get; set; }

        [Required(ErrorMessage = "El campo Teléfono es obligatorio.")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "El campo Teléfono debe contener solo números.")]
        [Display(Name = "Numero de Telefono")]
        [StringLength(8, ErrorMessage = "La Longitud Maxima es de 8")]
        public string Telefono { get; set; }

        [Display(Name = "Genero")]
        [Required]
        public int Genero { get; set; }

        

        //Adiccion para en listar
        public string NombreGenero { get; set; }

        
    }
}