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
			if (Session["uid"] == null)
			{
				return RedirectToAction("Index", "Login", new { from = "PanelDeControl-Index" });
			}
			var tickets = db.Ticket.AsEnumerable();
			return View(tickets);
		}

		public ActionResult TicketsEnEsperaPartialView()
		{
			var ticketsEnEspera = db.Ticket.Where(t => t.Nota == null).AsEnumerable();
			return PartialView("_TicketsEnEsperaPartialView", ticketsEnEspera);
		}

		public ActionResult TicketsAtendidosPartialView()
		{
			var ticketsAtendidos = db.Ticket.Where(t => t.Nota != null).AsEnumerable();
			return PartialView("_TicketsAtendidosPartialView", ticketsAtendidos);
		}

		public ActionResult AtenderTicket(int id = 0)
		{
			if (Session["uid"] == null)
			{
				return RedirectToAction("Index", "Login", new { from = $"PanelDeControl-Index" });
			}
			if (id == 0)
			{
				return RedirectToAction("Index");
			}
			try
			{
				var ticket = db.Ticket.FirstOrDefault(t => t.Id == id);
				if (ticket != null)
				{
					var cliente = db.Cliente.FirstOrDefault(c => c.Id == ticket.ClienteId);
					ticket.Cliente = cliente;
					return View(ticket);
				}
				else
				{
					TempData["ErrorMessage"] = $"Ticket con ID {id} no existe";
				}
			}
			catch (Exception e)
			{
				if (e.InnerException == null)
				{
					TempData["ErrorMessage"] = $"Error: {e.Message}";
				}
				else
				{
					TempData["ErrorMessage"] = $"Error: {e.InnerException.Message}";
				}
			}
			return RedirectToAction("Index");
		}

		[HttpPost]
		public ActionResult AtenderTicket(Tickets model)
		{
			if (model.Nota.IsNullOrWhiteSpace())
			{
				ModelState.AddModelError("Nota", "Nota vacía.");
				var client = db.Cliente.First(c => c.Id == model.ClienteId);
				var proceso = db.Proceso.First(p => p.Id == model.ProcesoId);
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
					model.FechaCreacion = DateTime.Now;
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
					var client = db.Cliente.First(c => c.Id == model.ClienteId);
					var proceso = db.Proceso.First(p => p.Id == model.ProcesoId);
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