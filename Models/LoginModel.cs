using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Ticket.Models
{
	public class LoginModel
	{
		[DisplayName("Correo")]
		[Required(ErrorMessage = "El correo es requerido")]
		[EmailAddress(ErrorMessage = "Dirección de correo no válida")]
		public string Correo { get; set; }

		[Required(ErrorMessage = "La contraseña es requerida")]
		[DisplayName("Contraseña")]
		[StringLength(8, ErrorMessage = "La contraseña debe contener por lo menos ocho caracteres")]
		public string Contraseña { get; set; }
	}
}