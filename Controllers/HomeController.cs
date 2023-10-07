using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Ticket.Models;

namespace Ticket.Controllers
{
	[HandleError]
	public class HomeController : Controller
	{
		private readonly yanill_ticketsEntities db = new yanill_ticketsEntities();
		public ActionResult Index()
		{
			var tickets = db.Tickets
				.OrderBy(t => t.Id)
				.Where(t => t.Nota == null)
				.Include(t => t.Cliente)
				.Include(t => t.Proceso)
				.Take(5);
			var primerosCincoTickets = tickets.AsEnumerable();
			return View(primerosCincoTickets);
		}
	}
}