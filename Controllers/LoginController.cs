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
            return View();
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
            if(!ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("NO ES VALIDO");
				return View(model);
			}
            var usuario = db.Usuarios.FirstOrDefault(u => u.Nombre == model.Correo && u.Password == model.Contraseña);
            if (usuario == null)
            {
				ModelState.AddModelError("Error", "Usuario o contraseña incorrecta");
				System.Diagnostics.Debug.WriteLine("Usuario o contraseña incorrecta");
				return View(model);
            }
            return RedirectToAction("Index", "Cliente");
        }
    }
}
