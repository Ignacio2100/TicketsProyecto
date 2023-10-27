using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ticket.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Drawing.Printing;
using System.Web;
using System.Xml.Linq;
using System;

namespace Ticket.Controllers
{
	public class ReporteController : Controller
    {
        // GET: Reporte
        public ActionResult Index(DateTime? fechaInicio, DateTime? fechaFin)
        {
            List<ReporteCLS> ListaReporte = null;
            using (var bd = new yanill_ticketsEntities())
            {
                var query = from T in bd.Ticket
                            join P in bd.Proceso on T.ProcesoId equals P.Id
                            join C in bd.Cliente on T.ClienteId equals C.Id
                            where (!fechaInicio.HasValue || (T.FechaCreacion >= fechaInicio)) &&
                                 (!fechaFin.HasValue || (T.FechaCreacion <= fechaFin))
                            orderby T.FechaCreacion.Month
                            select new ReporteCLS
                            {
                                Mes = (DateTime)T.FechaCreacion,
                                Proceso = P.Descripcion,
                                Cliente = C.Nombre
                            };

                ListaReporte = query.ToList();
            }

            ViewBag.FechaInicio = fechaInicio; // Esto se utiliza para mantener la fecha de inicio seleccionada en la vista
            ViewBag.FechaFin = fechaFin; // Esto se utiliza para mantener la fecha de fin seleccionada en la vista

            Session["ListaReporte"] = ListaReporte;
            return View(ListaReporte);
        }

        public ActionResult DescargarPDF()
        {
            List<ReporteCLS> ListaReporte = Session["ListaReporte"] as List<ReporteCLS>;

            using (MemoryStream ms = new MemoryStream())
            {
                Document document = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();

                PdfPTable table = new PdfPTable(3); // 3 columnas para tus datos
                table.WidthPercentage = 100;
                float[] widths = new float[] { 40, 40, 40}; // Ancho de las columnas
                table.SetWidths(widths);

                // Encabezados de la tabla
                table.AddCell("Nombre del Cliente");
                table.AddCell("Proceso");
                table.AddCell("Mes");
             

                // Datos de la tabla
                foreach (var item in ListaReporte)
                {
                    table.AddCell(item.Cliente);
                    table.AddCell(item.Proceso);                   
                    table.AddCell(item.Mes.ToString("dd/MM/yyyy"));
                }

                document.Add(table);
                document.Close();

                // Configurar la respuesta para descargar el PDF
                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=Reporte.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.BinaryWrite(ms.ToArray());
                Response.End();
            }

            return View("Index", ListaReporte);
        }
    }
}