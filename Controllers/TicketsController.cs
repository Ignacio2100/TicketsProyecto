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
			var tickets = db.Ticket.Include(t => t.Cliente).Include(t => t.Usuario);
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
			try
			{
				if (!dpi.IsNullOrWhiteSpace() && Regex.IsMatch(dpi, @"^\d+$") && dpi.Length == 13)
				{
					var cliente = db.Cliente.Include(c => c.TipoGenero).FirstOrDefault(c => c.Dpi == dpi);

					if (cliente != null)
					{
						var ticket = new Tickets
						{
							ClienteId = cliente.Id,
							Cliente = cliente,
						};
						ViewBag.Procesos = new SelectList(db.Proceso, "Id", "Descripcion");
						return View("Crear", ticket);
					}
				}
			}
			catch (Exception e)
			{

                TempData["ErrorMessage"] = $"{e.Message}";
            }
			
			
			return RedirectToAction("Buscar");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Crear(Tickets model)
		{
			try
			{
                var uid = Session["uid"];
				var id = Convert.ToInt32(uid.ToString());
                if (ModelState.IsValid)
				{
					model.FechaCreacion = DateTime.Now;
					model.EstadoTicketId = 1;
					model.UsuarioId = id;
					var newTicket = db.Ticket.Add(model);
					db.SaveChanges();
					TempData["SuccessMessage"] = "Ticket creado";
					var cliente = db.Cliente.First(c => c.Id == newTicket.ClienteId);
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
			ViewBag.Procesos = new SelectList(db.Proceso, "Id", "Descripcion");
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
