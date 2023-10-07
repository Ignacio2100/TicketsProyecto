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
			Debug.WriteLine("estamos en TicketsEnEsperaPartialView");
			var ticketsEnEspera = db.Tickets.Where(t => t.Nota == null).AsEnumerable();
			Debug.WriteLine(ticketsEnEspera.ToString());
			return PartialView("_TicketsEnEsperaPartialView", ticketsEnEspera);
		}

		public ActionResult TicketsAtendidosPartialView()
		{
			Debug.WriteLine("estamos en TicketsAtendidosPartialView");
			var ticketsAtendidos = db.Tickets.Where(t => t.Nota != null).AsEnumerable();
			Debug.WriteLine(ticketsAtendidos.ToString());
			return PartialView("_TicketsAtendidosPartialView", ticketsAtendidos);
		}

		public ActionResult AtenderTicket(int id)
		{
			var ticket = db.Tickets.FirstOrDefault(t => t.Id == id);
			var cliente = db.Clientes.FirstOrDefault(c => c.Id == ticket.ClienteId);
			ticket.Cliente = cliente;
			return View(ticket);
		}

		[HttpPost]
		public ActionResult AtenderTicket(Tickets model)
		{
			Debug.Write("es valido: ");
			Debug.WriteLine(ModelState.IsValid);
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
				var userId = Convert.ToInt32(uid.ToString());
				model.UsuarioId = userId;
				model.Fecha = DateTime.Now;
				db.Entry(model).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
		}
	}
}