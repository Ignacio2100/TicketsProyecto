using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Ticket.Models;

namespace Ticket.Controllers
{
	public class LoginController : Controller
	{
		private yanill_ticketsEntities db = new yanill_ticketsEntities();

		// GET: Login
		public ActionResult Index()
		{
			if (Session["uid"] != null)
			{
				return RedirectToAction("Index", "Home");
			}
			return View();
		}

		public ActionResult Logout()
		{
			Session["uid"] = null;
			Session["rol"] = null;
			return RedirectToAction("Index");
		}

		[HttpPost]
		public ActionResult LogoutConfirmed()
		{
			Session["uid"] = null;
			Session["rol"] = null;
			return RedirectToAction("Index");
		}

		// GET: Login/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		// GET: Login/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: Login/Create
		[HttpPost]
		public ActionResult Create(FormCollection collection)
		{
			try
			{
				// TODO: Add insert logic here

				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}

		// GET: Login/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: Login/Edit/5
		[HttpPost]
		public ActionResult Edit(int id, FormCollection collection)
		{
			try
			{
				// TODO: Add update logic here

				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}

		// GET: Login/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: Login/Delete/5
		[HttpPost]
		public ActionResult Delete(int id, FormCollection collection)
		{
			try
			{
				// TODO: Add delete logic here

				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}

		public ActionResult Login()
		{
			return View("Login");
		}

		[HttpPost]
		public ActionResult Login(LoginModel model)
		{
			if (ModelState.IsValid)
			{
				var usuario = db.Usuarios.Include(u => u.TipoUsuario1)
					.FirstOrDefault(u => u.Nombre == model.Correo && u.Password == model.Contraseña);
				if (usuario == null)
				{
					System.Diagnostics.Debug.WriteLine("Usuario o contraseña incorrecta");
					return View(model);
				}
				else
				{
					Session["uid"] = usuario.Id;
					Session["rol"] = usuario.TipoUsuario;

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
						return RedirectToAction("Index", "Cliente");
					}
					else
					{
						ModelState.AddModelError("Error", "Tipo de usuario invalido");
						return View(model);
					}

				}
			}
			System.Diagnostics.Debug.WriteLine("Modelo no valido");
			return View(model);
		}
	}
}
