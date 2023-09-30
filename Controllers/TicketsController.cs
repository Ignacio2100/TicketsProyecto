using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Ticket.Models;
using Tickets = Ticket.Models.Ticket;

namespace Ticket.Controllers
{
	public class TicketsController : Controller
    {
        private yanill_ticketsEntities db = new yanill_ticketsEntities();

        // GET: Tickets
        public async Task<ActionResult> Index()
        {
            var tickets = db.Tickets.Include(t => t.Cliente).Include(t => t.Usuario);
            return View(await tickets.ToListAsync());
        }

        // GET: Tickets/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tickets ticket = await db.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Tickets/Create
        public ActionResult Create()
        {
            ViewBag.ClienteId = new SelectList(db.Clientes, "Id", "Nombre");
            ViewBag.UsuarioId = new SelectList(db.Usuarios, "Id", "Nombre");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,UsuarioId,ClienteId,Fecha,Nota")] Tickets ticket)
        {
            if (ModelState.IsValid)
            {
                db.Tickets.Add(ticket);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ClienteId = new SelectList(db.Clientes, "Id", "Nombre", ticket.ClienteId);
            ViewBag.UsuarioId = new SelectList(db.Usuarios, "Id", "Nombre", ticket.UsuarioId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tickets ticket = await db.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClienteId = new SelectList(db.Clientes, "Id", "Nombre", ticket.ClienteId);
            ViewBag.UsuarioId = new SelectList(db.Usuarios, "Id", "Nombre", ticket.UsuarioId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,UsuarioId,ClienteId,Fecha,Nota")] Tickets ticket)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticket).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ClienteId = new SelectList(db.Clientes, "Id", "Nombre", ticket.ClienteId);
            ViewBag.UsuarioId = new SelectList(db.Usuarios, "Id", "Nombre", ticket.UsuarioId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tickets ticket = await db.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Tickets ticket = await db.Tickets.FindAsync(id);
            db.Tickets.Remove(ticket);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
