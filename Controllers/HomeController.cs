using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Ticket.Models;
using Tickets = Ticket.Models.Ticket;


namespace Ticket.Controllers
{
	[HandleError]
	public class HomeController : Controller
	{
		private readonly yanill_ticketsEntities db = new yanill_ticketsEntities();
		public ActionResult Index()
		{
			var tickets = db.Ticket
				.OrderBy(t => t.Id)
				.Where(t => t.Nota == null)
				.Include(t => t.Cliente)
				.Include(t => t.Proceso)
				.Take(5);
			var primerosCincoTickets = tickets.AsEnumerable();
			return View(primerosCincoTickets);
		}

		public ActionResult Test(string dpi = "1234567891234")
		{
			if (true)
			{
				var cliente = db.Cliente.FirstOrDefault(c => c.Dpi == dpi);

				if (cliente != null)
				{
					var ticket = new Tickets
					{
						ClienteId = cliente.Id,
						Cliente = cliente,
					};
					ViewBag.Procesos = new SelectList(db.Proceso, "Id", "Descripcion");
					return View("Test", ticket);
				}
			}
			TempData["ErrorMessage"] = $"No existe un cliente con este DPI";
			return RedirectToAction("Buscar");
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