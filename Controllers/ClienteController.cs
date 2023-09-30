using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ticket.Models;


namespace Ticket.Controllers
{
    public class ClienteController : Controller
    {
        // GET: Clientes
        public ActionResult Index() //Este proceso se usa para poder en listar tablas o hacer Select de la base de datos
        {
            List<ClienteCLS> listaCliente = null;
            using (var bd = new yanill_ticketsEntities())
            {
                listaCliente = (from Clientes in bd.Clientes
                                join Tipo in bd.TipoGeneroes
                                on Clientes.Genero equals Tipo.Id
                                join Proceso in bd.Procesoes
                                on Clientes.ProcesoId equals Proceso.Id
                                select new ClienteCLS
                                {
                                    Id = Clientes.Id,
                                    Nombre = Clientes.Nombre,
                                    Apellido = Clientes.Apellido,
                                    Dpi = Clientes.Dpi,
                                    Telefono = Clientes.Telefono,
                                    NombreGenero = Tipo.Genero,
                                    TipoProblema = Proceso.Nombre 
                                }).ToList();
            }

           
            return View(listaCliente);
        }

        public void listarComboGenero()//Este proceso se usa para poder en listar los elementos de una tabla
        {
            //agregar
            List<SelectListItem> listarGenero;
            using (var bd = new yanill_ticketsEntities())
            {
                listarGenero = (from TipoGenero in bd.TipoGeneroes
                                select new SelectListItem
                                {
                                    Text = TipoGenero.Genero,
                                    Value = TipoGenero.Id.ToString(),

                                }).ToList();
                listarGenero.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                ViewBag.lista = listarGenero;
            }
        }

        public void listarProceso() //Este proceso se usa para poder en listar los elementos de una tabla
        {
            //agregar
            List<SelectListItem> listarProceso;
            using (var bd = new yanill_ticketsEntities())
            {
                listarProceso = (from Proceso in bd.Procesoes
                                select new SelectListItem
                                {
                                    Text = Proceso.Nombre,
                                    Value = Proceso.Id.ToString(),

                                }).ToList();
                listarProceso.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                ViewBag.lista2 = listarProceso;
            }
        }

        public void ListarCombos()
        {
            listarComboGenero();
            listarProceso();

        }
        public ActionResult Agregar()
        {
            ListarCombos(); // Llama al método para cargar la lista de géneros en ViewBag
            return View();
        }


        [HttpPost]
        public ActionResult Agregar(ClienteCLS oClientesCLS) // Este proceso se usa para ingresar Datos a la Tabla Clientes
        {
            if (!ModelState.IsValid)
            {
                ListarCombos();
                return View(oClientesCLS);
            }
            else
            {
                using (var bd = new yanill_ticketsEntities())
                {
                    // Verificar si ya existe un cliente con el mismo DPI
                    bool existeDPI = bd.Clientes.Any(c => c.Dpi == oClientesCLS.Dpi);

                    if (existeDPI)
                    {
                        ModelState.AddModelError("Dpi", "Ya existe un cliente con este DPI.");
                        ListarCombos();
                        return View(oClientesCLS);
                    }   

                    Cliente oCliente = new Cliente();
                    oCliente.Nombre = oClientesCLS.Nombre;
                    oCliente.Apellido = oClientesCLS.Apellido;
                    oCliente.Dpi = oClientesCLS.Dpi;
                    oCliente.Telefono = oClientesCLS.Telefono;
                    oCliente.Genero = oClientesCLS.Genero;
                    oCliente.ProcesoId = oClientesCLS.Id;
                    bd.Clientes.Add(oCliente);
                    bd.SaveChanges();
                }
                return RedirectToAction("Index", "Home");
            }

        }

       
    }
}