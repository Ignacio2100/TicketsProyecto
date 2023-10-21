using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ticket.Models;

namespace Ticket.Controllers
{
	public class ReporteController : Controller
    {
        // GET: Reporte
        public ActionResult Index(int? mes)
        {
            List<ReporteCLS> ListaReporte = null;
            using (var bd = new yanill_ticketsEntities())
            {
                var query = from T in bd.Ticket
                            join P in bd.Proceso on T.ProcesoId equals P.Id
                            join C in bd.Cliente on T.ClienteId equals C.Id
                            where T.FechaCreacion.Year == 2023 && (!mes.HasValue || T.FechaCreacion.Month == mes.Value)
                            orderby T.FechaCreacion.Month
                            select new ReporteCLS
                            {
                                Mes = T.FechaCreacion.Month,
                                Proceso = P.Descripcion,
                                Cliente = C.Nombre
                            };

                ListaReporte = query.ToList();
            }

            ViewBag.MesSeleccionado = mes; // Esto se utiliza para mantener el mes seleccionado en la vista

            return View(ListaReporte);
        }
    }
}