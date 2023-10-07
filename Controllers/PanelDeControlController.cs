using Microsoft.Ajax.Utilities;
using System.Linq;
using System.Web.Mvc;
using System.Diagnostics;
using System;
using System.Data.Entity;
using Ticket.Models;
using Tickets = Ticket.Models.Ticket;

namespace Ticket.Controllers
{
	public class PanelDeControlController : Controller
	{
		private readonly yanill_ticketsEntities db = new yanill_ticketsEntities();

		public ActionResult Index()
		{
			var tickets = db.Tickets.AsEnumerable();
			return View(tickets);
		}

		public ActionResult TicketsEnEsperaPartialView()
		{
			var ticketsEnEspera = db.Tickets.Where(t => t.Nota == null).AsEnumerable();
			return PartialView("_TicketsEnEsperaPartialView", ticketsEnEspera);
		}

		public ActionResult TicketsAtendidosPartialView()
		{
			var ticketsAtendidos = db.Tickets.Where(t => t.Nota != null).AsEnumerable();
			return PartialView("_TicketsAtendidosPartialView", ticketsAtendidos);
		}

		public ActionResult AtenderTicket(int id = 0)
		{
			if (id == 0)
			{
				return RedirectToAction("Index");
			}
			var ticket = db.Tickets.FirstOrDefault(t => t.Id == id);
			var cliente = db.Clientes.FirstOrDefault(c => c.Id == ticket.ClienteId);
			ticket.Cliente = cliente;
			return View(ticket);
		}

		[HttpPost]
		public ActionResult AtenderTicket(Tickets model)
		{
			if (model.Nota.IsNullOrWhiteSpace())
			{
				ModelState.AddModelError("Nota", "Nota vacía.");
				var client = db.Clientes.First(c => c.Id == model.ClienteId);
				var proceso = db.Procesoes.First(p => p.Id == model.ProcesoId);
				model.Cliente = client;
				model.Proceso = proceso;
				return View(model);
			}
			var uid = Session["uid"];
			if (uid == null)
			{
				return RedirectToAction("Index", "Login");
			}
			else
			{
				try
				{
					var userId = Convert.ToInt32(uid.ToString());
					model.UsuarioId = userId;
					model.Fecha = DateTime.Now;
					db.Entry(model).State = EntityState.Modified;
					db.SaveChanges();
					TempData["SuccessMessage"] = $"Se guardó el comentario";
					return RedirectToAction("Index");
				}
				catch (Exception e)
				{
					if (e.InnerException == null)
					{
						TempData["ErrorMessage"] = $"Error al registrar el ticket: {e.Message}";
					}
					else
					{
						TempData["ErrorMessage"] = $"Error al registrar el ticket: {e.InnerException.Message}";
					}
					var client = db.Clientes.First(c => c.Id == model.ClienteId);
					var proceso = db.Procesoes.First(p => p.Id == model.ProcesoId);
					model.Cliente = client;
					model.Proceso = proceso;
					return View(model);
				}
			}
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