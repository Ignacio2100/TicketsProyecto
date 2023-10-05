namespace Ticket.Models
{
	using System;

	public partial class Ticket
    {
        public int Id { get; set; }
        public Nullable<int> UsuarioId { get; set; }
        public Nullable<int> ClienteId { get; set; }
        public System.DateTime Fecha { get; set; }
        public string Nota { get; set; }
    
        public virtual Cliente Cliente { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
