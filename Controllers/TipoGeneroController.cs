using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ticket.Models;

namespace Ticket.Controllers
{
    public class TipoGeneroController : Controller
    {
        // GET: Tipo Genero
        public ActionResult Index()
        {
            List<TipoGeneroCLS> ListaTipoGenero = null;
            using (var bd = new TICKETSEntities())
            {
                ListaTipoGenero = (from Tipo in bd.TipoGenero
                                         select new TipoGeneroCLS
                                         {
                                             Id = Tipo.Id,
                                             Genero = Tipo.Genero
                                         }
                                                       ).ToList();
            }

            return View(ListaTipoGenero);
        }

        public ActionResult Agregar()
        {
            return View(); 
        }


        [HttpPost]
        public ActionResult Agregar(TipoGeneroCLS oTipoGeneroCLS)
        {
            if (!ModelState.IsValid)
            {
                return View(oTipoGeneroCLS);
            }
            else
            {
                using (var bd = new TICKETSEntities())
                {
                    TipoGenero oTipoGenero = new TipoGenero();
                    oTipoGenero.Genero = oTipoGeneroCLS.Genero;
                    bd.TipoGenero.Add(oTipoGenero);
                    bd.SaveChanges();
                }

            }
            return RedirectToAction("Index");
        }
    }
}