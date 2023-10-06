namespace Ticket.Models
{
	using System;

	public partial class Ticket
	{
		public int Id { get; set; }
		public int? UsuarioId { get; set; }
		public int? ClienteId { get; set; }
		public DateTime Fecha { get; set; }
		public string Nota { get; set; }
		public int ProcesoId { get; set; }

		public virtual Cliente Cliente { get; set; }
		public virtual Usuario Usuario { get; set; }
		public virtual Proceso Proceso { get; set; }
	}
}
