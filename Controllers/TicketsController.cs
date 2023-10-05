using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Ticket.Models;
using Tickets = Ticket.Models.Ticket;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Ajax.Utilities;
using System;

namespace Ticket.Controllers
{

	public class TicketsController : Controller
	{
		private yanill_ticketsEntities db = new yanill_ticketsEntities();

		public async Task<ActionResult> Index()
		{
			var tickets = db.Tickets.Include(t => t.Cliente).Include(t => t.Usuario);
			return View(await tickets.ToListAsync());
		}

		public async Task<ActionResult> Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Tickets ticket = await db.Tickets.FindAsync(id);
			if (ticket == null)
			{
				return HttpNotFound();
			}
			return View(ticket);
		}

		public ActionResult Create()
		{
			ViewBag.ClienteId = new SelectList(db.Clientes, "Id", "Nombre");
			ViewBag.UsuarioId = new SelectList(db.Usuarios, "Id", "Nombre");
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create([Bind(Include = "Id,UsuarioId,ClienteId,Fecha,Nota")] Tickets ticket)
		{
			if (ModelState.IsValid)
			{
				db.Tickets.Add(ticket);
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}

			ViewBag.ClienteId = new SelectList(db.Clientes, "Id", "Nombre", ticket.ClienteId);
			ViewBag.UsuarioId = new SelectList(db.Usuarios, "Id", "Nombre", ticket.UsuarioId);
			return View(ticket);
		}

		public async Task<ActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Tickets ticket = await db.Tickets.FindAsync(id);
			if (ticket == null)
			{
				return HttpNotFound();
			}
			ViewBag.ClienteId = new SelectList(db.Clientes, "Id", "Nombre", ticket.ClienteId);
			ViewBag.UsuarioId = new SelectList(db.Usuarios, "Id", "Nombre", ticket.UsuarioId);
			return View(ticket);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit([Bind(Include = "Id,UsuarioId,ClienteId,Fecha,Nota")] Tickets ticket)
		{
			if (ModelState.IsValid)
			{
				db.Entry(ticket).State = EntityState.Modified;
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}
			ViewBag.ClienteId = new SelectList(db.Clientes, "Id", "Nombre", ticket.ClienteId);
			ViewBag.UsuarioId = new SelectList(db.Usuarios, "Id", "Nombre", ticket.UsuarioId);
			return View(ticket);
		}

		public async Task<ActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Tickets ticket = await db.Tickets.FindAsync(id);
			if (ticket == null)
			{
				return HttpNotFound();
			}
			return View(ticket);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(int id)
		{
			Tickets ticket = await db.Tickets.FindAsync(id);
			db.Tickets.Remove(ticket);
			await db.SaveChangesAsync();
			return RedirectToAction("Index");
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}

		public ActionResult Buscar()
		{
			return View();
		}

		public ActionResult BuscarDPI()
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
			return RedirectToAction("Buscar");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Crear(Tickets model)
		{
			if (ModelState.IsValid)
			{
				model.Fecha = DateTime.Now;
				var newTicket = db.Tickets.Add(model);
				db.SaveChanges();
				var cliente = db.Clientes.First(c => c.Id == newTicket.ClienteId);
				newTicket.Cliente = cliente;
				return View("Comprobante", newTicket);
			}
			ViewBag.Procesos = new SelectList(db.Procesoes, "Id", "Descripcion");
			return View("Crear", model);
		}

		public ActionResult Comprobante()
		{
			return View();
		}
	}
}
