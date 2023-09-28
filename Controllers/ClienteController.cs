using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ticket.Models;


namespace Ticket.Controllers
{
    public class ClienteController : Controller
    {
        // GET: Clientes
        public ActionResult Index()
        {
            List<ClienteCLS> listaCliente = null;
            using (var bd = new TICKETSEntities())
            {
                listaCliente = (from Clientes in bd.Cliente
                                join Tipo in bd.TipoGenero
                                on Clientes.Genero equals Tipo.Id
                                select new ClienteCLS
                                {
                                    Id = Clientes.Id,
                                    Nombre = Clientes.Nombre,
                                    Apellido = Clientes.Apellido,
                                    Dpi = Clientes.Dpi,
                                    Telefono = Clientes.Telefono,
                                    NombreGenero = Tipo.Genero
                                }).ToList();
            }

            int numeroPersonasRegistradas = ObtenerNumeroPersonasRegistradas();
            ViewBag.NumeroPersonasRegistradas = numeroPersonasRegistradas;

            return View(listaCliente);
        }

        public void listarComboGenero()
        {
            //agregar
            List<SelectListItem> listarGenero;
            using (var bd = new TICKETSEntities())
            {
                listarGenero = (from TipoGenero in bd.TipoGenero
                                select new SelectListItem
                                {
                                    Text = TipoGenero.Genero,
                                    Value = TipoGenero.Id.ToString(),

                                }).ToList();
                listarGenero.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                ViewBag.lista = listarGenero;
            }
        }

        public void ListarCombos()
        {
            listarComboGenero();
        }
        public ActionResult Agregar()
        {
            ListarCombos(); // Llama al método para cargar la lista de géneros en ViewBag
            return View();
        }


        [HttpPost]
        public ActionResult Agregar(ClienteCLS oClientesCLS)
        {
            if (!ModelState.IsValid)
            {
                ListarCombos();
                return View(oClientesCLS);
            }
            else
            {
                using (var bd = new TICKETSEntities())
                {
                    Cliente oCliente = new Cliente();
                    oCliente.Nombre = oClientesCLS.Nombre;
                    oCliente.Apellido = oClientesCLS.Apellido;
                    oCliente.Dpi = oClientesCLS.Dpi;
                    oCliente.Telefono = oClientesCLS.Telefono;
                    oCliente.Genero = oClientesCLS.Genero;
                    bd.Cliente.Add(oCliente);
                    bd.SaveChanges();
                }
                return RedirectToAction("Index", "Home");
            }

        }


        public int ObtenerNumeroPersonasRegistradas()
        {
            int numeroPersonasRegistradas = 0;

            using (var bd = new TICKETSEntities())
            {
                numeroPersonasRegistradas = bd.Cliente.Count(); // Cuenta los registros en la tabla Cliente
            }

            return numeroPersonasRegistradas;
        }
    }
}