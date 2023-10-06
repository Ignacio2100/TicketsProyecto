namespace Ticket.Models
{
	using System.Collections.Generic;
	using System.ComponentModel;

	public partial class Cliente
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cliente()
        {
            this.Tickets = new HashSet<Ticket>();
        }
    
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Dpi { get; set; }
        [DisplayName("Teléfono")]
        public string Telefono { get; set; }
		[DisplayName("Género")]
		public int Genero { get; set; }
    
        public virtual TipoGenero TipoGenero { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
