using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ticket.Models;


namespace Ticket.Controllers
{
    public class ProcesoController : Controller
    {
        // GET: Proceso
        public ActionResult Index()
        {
            List<ProcesoCLS> ListaProceso = null;
            using (var bd = new yanill_ticketsEntities())
            {
                ListaProceso = (from Tipo in bd.Procesoes
                                   select new ProcesoCLS
                                   {
                                       Id = Tipo.Id,
                                       Descripcion = Tipo.Descripcion
                                   }
                                ).ToList();
            }

            return View(ListaProceso);
        }

        public ActionResult Agregar()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Agregar(ProcesoCLS oProcesoCLS)
        {
            if (!ModelState.IsValid)
            {
                return View(oProcesoCLS);
            }
            else
            {
                using (var bd = new yanill_ticketsEntities())
                {
                    Proceso oProceso = new Proceso();
                    oProceso.Descripcion = oProcesoCLS.Descripcion;
                    bd.Procesoes.Add(oProceso);
                    bd.SaveChanges();
                }

            }
            return RedirectToAction("Index");
        }
    }
}