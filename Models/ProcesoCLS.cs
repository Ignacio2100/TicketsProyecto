using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ticket.Models
{
    public class ProcesoCLS
    {
        [Display(Name = "Codigo")]
        public int Id { get; set; }

        [Display(Name = "Tipo de Problema")]
        public string Descripcion { get; set; }
    }
}