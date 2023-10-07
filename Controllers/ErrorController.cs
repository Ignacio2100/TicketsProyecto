using System.Web.Mvc;

namespace Ticket.Controllers
{
	public class ErrorController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PageNotFound()
        {
            return View();
        }
    }
}