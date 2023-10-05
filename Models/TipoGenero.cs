namespace Ticket.Models
{
	using System.Collections.Generic;

	public partial class TipoGenero
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TipoGenero()
        {
            this.Clientes = new HashSet<Cliente>();
        }
    
        public int Id { get; set; }
        public string Genero { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cliente> Clientes { get; set; }
    }
}
