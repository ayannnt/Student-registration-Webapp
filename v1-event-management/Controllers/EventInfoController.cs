using Microsoft.AspNetCore.Mvc;
using EventManagementWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace EventManagementWebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EventInfoController : Controller
    {
        EventDbContext dbContext;

        // Constructor Dependency Injection to create the DbContext object
        public EventInfoController(EventDbContext context)
        {
            dbContext = context;
        }

        // ═══ Anyone (including anonymous) can view the Event list ═══
        [AllowAnonymous]
        public IActionResult Index()
        {
            var eventList = dbContext.Event.ToList();
            return View(eventList);
        }

        // ═══ Anyone (including anonymous) can view Event details ═══
        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            var e = dbContext.Event.Find(id);
            if (e == null) return NotFound();

            // Store selected Event in TempData for Participant registration
            TempData["EventId"] = e.EventId;

            return View(e);
        }

        // ═══ Admin only — Create a new Event ═══
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EventCreateEditViewModel ev)
        {
            if (ModelState.IsValid)
            {
                EventInfo e = new EventInfo();
                e.EventName = ev.EventName;
                e.EventLocation = ev.EventLocation;
                dbContext.Event.Add(e);
                dbContext.SaveChanges();
                TempData["SuccessMessage"] = $"Event '{e.EventName}' created successfully!";
                return RedirectToAction("Index");
            }
            return View();
        }

        // ═══ Admin only — Edit an Event ═══
        public IActionResult Edit(int id)
        {
            var e = dbContext.Event.Find(id);
            if (e == null) return NotFound();

            EventCreateEditViewModel ev = new EventCreateEditViewModel();
            ev.EventId = e.EventId;
            ev.EventName = e.EventName;
            ev.EventLocation = e.EventLocation;
            return View(ev);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, EventCreateEditViewModel evModified)
        {
            var e = dbContext.Event.Find(id);
            if (e == null) return NotFound();

            if (ModelState.IsValid)
            {
                e.EventName = evModified.EventName;
                e.EventLocation = evModified.EventLocation;
                dbContext.SaveChanges();
                TempData["SuccessMessage"] = $"Event '{e.EventName}' updated successfully!";
                return RedirectToAction("Index");
            }
            return View(evModified);
        }

        // ═══ Admin only — Delete an Event (with confirmation page) ═══
        public IActionResult Delete(int id)
        {
            var e = dbContext.Event.Include(ev => ev.Participants)
                        .Where(ev => ev.EventId == id).SingleOrDefault();
            if (e == null) return NotFound();

            return View(e);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var e = dbContext.Event.Find(id);
            if (e != null)
            {
                dbContext.Event.Remove(e);
                dbContext.SaveChanges();
                TempData["SuccessMessage"] = $"Event '{e.EventName}' deleted successfully!";
            }
            return RedirectToAction("Index");
        }
    }
}
