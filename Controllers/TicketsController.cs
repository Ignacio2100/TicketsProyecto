using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;
using Ticket.Models;
using Tickets = Ticket.Models.Ticket;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Ajax.Utilities;
using System;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Collections.Generic;


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
				// var uid = Session["uid"];
				// var id = Convert.ToInt32(uid.ToString()); le puse comentario porque no deja crear ticket si no esta logeado
				if (ModelState.IsValid)
				{
					model.FechaCreacion = DateTime.Now;
					model.EstadoTicketId = 1;
					// model.UsuarioId = id;
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
		public ActionResult DescargarComprobantePDF(int ticketId)
		{
			var ticket = db.Ticket.Find(ticketId);
			if (ticket == null)
			{
				return HttpNotFound();
			}

			// Creamos un nuevo documento PDF
			Document doc = new Document();
			MemoryStream memoryStream = new MemoryStream();
			PdfWriter writer = PdfWriter.GetInstance(doc, memoryStream);

			doc.Open();

			// Agregamos el contenido al documento PDF
			doc.Add(new Paragraph("Número de ticket: " + ticket.Id));
			doc.Add(new Paragraph("Cliente: " + ticket.Cliente.Nombre + " " + ticket.Cliente.Apellido));
			doc.Add(new Paragraph("Fecha de emisión: " + ticket.FechaCreacion.ToString("d 'de' MMMM 'de' yyyy, h:mm tt")));

			// Cerrar el documento PDF
			doc.Close();

			// Configuramos la respuesta para descargar el PDF
			Response.ContentType = "application/pdf";
			Response.AddHeader("Content-Disposition", "attachment; filename=Comprobante.pdf");
			Response.BinaryWrite(memoryStream.ToArray());

			//return new EmptyResult();
			// return RedirectToAction(nameof(Buscar));

			// Generate and save the PDF to a temporary location
			string pdfFilePath = "path_to_temporary_location/Comprobante.pdf"; // Adjust the path

			// ... (PDF generation code)

			// Redirect to the 'Buscar' action with a parameter that specifies the PDF path
			return RedirectToAction(nameof(Buscar), new { pdfPath = pdfFilePath });

		}




	}
}
