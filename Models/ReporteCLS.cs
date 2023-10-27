using System;

namespace Ticket.Models
{
	public class ReporteCLS
    {
        public int Id { get; set; } 
        public DateTime Mes { get; set; }

        public string Proceso { get; set; }

        public string Cliente { get; set; }

        public string TiempoEnAtencion { get; set; } = string.Empty;

        public string Usuario { get; set; }

    }
}