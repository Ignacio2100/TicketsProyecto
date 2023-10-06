namespace Ticket.Models
{
	using System.Collections.Generic;
	using System.ComponentModel;

	public partial class TipoGenero
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TipoGenero()
        {
            this.Clientes = new HashSet<Cliente>();
        }
    
        public int Id { get; set; }
        [DisplayName("Género")]
        public string Genero { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cliente> Clientes { get; set; }
    }
}
