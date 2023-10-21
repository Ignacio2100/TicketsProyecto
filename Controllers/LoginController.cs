using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using Ticket.Models;

namespace Ticket.Controllers
{
	public class LoginController : Controller
	{
		private yanill_ticketsEntities db = new yanill_ticketsEntities();

		public ActionResult Index(string from = null)
		{
			if (Session["uid"] != null)
			{
				return RedirectToAction("Index", "Home");
			}
			ViewBag.From = from;
			return View();
		}

		public ActionResult Logout()
		{
			Session["uid"] = null;
			Session["rol"] = null;
			return RedirectToAction("Index");
		}

		[HttpPost]
		public ActionResult Login(LoginModel model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var usuario = db.Usuario.Include(u => u.TipoUsuario1)
					.FirstOrDefault(u => u.Nombre == model.Correo && u.Password == model.Contraseña);
					if (usuario == null)
					{
						ModelState.AddModelError("Error", "Usuario o contraseña incorrecta");
						Debug.WriteLine("Usuario o contraseña incorrecta");
						return View(model);
					}
					else
					{
						TempData["SuccessMessage"] = $"Se ha iniciado sesión como: {usuario.Nombre}";
						Session["uid"] = usuario.Id;
						Session["usuario"] = usuario.Nombre;
						Session["rol"] = usuario.TipoUsuario;
						if (ViewBag.From != null)
						{
							string input = ViewBag.From.ToString();
							string[] parts = input.Split('-');

							string controllerName = parts[0];
							string actionName = parts[1];

							return RedirectToAction(actionName, controllerName);
						}
						// Si el usuario es administrador
						if (usuario.TipoUsuario1.Id == 1)
						{
							// Enviar a la ruta correcta si es administrador
							return RedirectToAction("Index", "Home");
						}
						// Si el usuario es empleado
						else if (usuario.TipoUsuario1.Id == 2)
						{
							// Enviar a la ruta correcta si es empleado
							return RedirectToAction("Index", "PanelDeControl");
						}
						else
						{
							ModelState.AddModelError("Error", "Tipo de usuario invalido");
							return View(model);
						}
					}
				}
				catch (System.Exception e)
				{
					if (e.InnerException == null)
					{
						TempData["ErrorMessage"] = $"Error: {e.Message}";
					}
					else
					{
						TempData["ErrorMessage"] = $"Error: {e.InnerException.Message}";
					}
					return View(model);
				}
			}
			Debug.WriteLine("Modelo no valido");
			return View(model);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
