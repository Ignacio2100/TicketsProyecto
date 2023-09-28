using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ticket.Models
{
    public class TipoGeneroCLS
    {
        [Display(Name = "Codigo")]
        public int Id   { get; set; }

        [Display(Name = "Genero")]
        public string Genero { get; set; }
    }
}