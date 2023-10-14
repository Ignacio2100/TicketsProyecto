using System.Web.Mvc;
using Ticket.Models;

namespace Ticket.Controllers
{
	public class RegistrarseController : Controller
    {
		private yanill_ticketsEntities db = new yanill_ticketsEntities();

		public ActionResult Index()
        {
            return View();
        }
    }
}