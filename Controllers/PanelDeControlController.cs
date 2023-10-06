using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ticket.Models;
using Tickets = Ticket.Models.Ticket;
using System.Diagnostics;
using Microsoft.Ajax.Utilities;

namespace Ticket.Controllers
{
	public class PanelDeControlController : Controller
	{
		private yanill_ticketsEntities db = new yanill_ticketsEntities();

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
	}
}