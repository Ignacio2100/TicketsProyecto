using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;
using Ticket.Models;
using Tickets = Ticket.Models.Ticket;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Ajax.Utilities;
using System;

namespace Ticket.Controllers
{
	public class TicketsController : Controller
	{
		private readonly yanill_ticketsEntities db = new yanill_ticketsEntities();

		public async Task<ActionResult> Index()
		{
			var tickets = db.Tickets.Include(t => t.Cliente).Include(t => t.Usuario);
			return View(await tickets.ToListAsync());
		}

		public ActionResult Buscar()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult BuscarDPI(string dpi)
		{
			if (Regex.IsMatch(dpi, @"^\d+$") && dpi.Length == 13)
			{
				return RedirectToAction("Crear", new { dpi });
			}
			TempData["ErrorMessage"] = $"No existe un cliente con este DPI";
			return View("Buscar");
		}

		public ActionResult Crear(string dpi)
		{
			if (!dpi.IsNullOrWhiteSpace() && Regex.IsMatch(dpi, @"^\d+$") && dpi.Length == 13)
			{
				var cliente = db.Clientes.FirstOrDefault(c => c.Dpi == dpi);

				if (cliente != null)
				{
					var ticket = new Tickets
					{
						ClienteId = cliente.Id,
						Cliente = cliente,
					};
					ViewBag.Procesos = new SelectList(db.Procesoes, "Id", "Descripcion");
					return View("Crear", ticket);
				}
			}
			TempData["ErrorMessage"] = $"No existe un cliente con este DPI";
			return RedirectToAction("Buscar");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Crear(Tickets model)
		{
			try
			{
				if (ModelState.IsValid)
				{
					model.Fecha = DateTime.Now;
					var newTicket = db.Tickets.Add(model);
					db.SaveChanges();
					TempData["SuccessMessage"] = "Ticket creado";
					var cliente = db.Clientes.First(c => c.Id == newTicket.ClienteId);
					newTicket.Cliente = cliente;
					return View("Comprobante", newTicket);
				}
			}
			catch (Exception e)
			{
				if (e.InnerException == null)
				{
					TempData["ErrorMessage"] = $"Error al crear el ticket: {e.Message}";
				}
				else
				{
					TempData["ErrorMessage"] = $"Error al crear el ticket: {e.InnerException.Message}";
				}
			}
			ViewBag.Procesos = new SelectList(db.Procesoes, "Id", "Descripcion");
			return View("Crear", model);
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
