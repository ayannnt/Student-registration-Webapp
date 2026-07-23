using Microsoft.AspNetCore.Mvc;
using EventManagementWebApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace EventManagementWebApp.Controllers
{
    public class ParticipantController : Controller
    {
        EventDbContext dbContext;

        public ParticipantController(EventDbContext context)
        {
            dbContext = context;
        }

        // ═══ Admin only — List of all Participants ═══
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var participants = dbContext.Participants.Include("Event").ToList();
            return View(participants);
        }

        // ═══ User only — Register for an Event ═══
        [Authorize(Roles = "User")]
        [HttpGet]
        public IActionResult Create()
        {
            // A user can register only once — redirect to profile if already registered
            var existing = dbContext.Participants
                    .Where(pr => pr.Email == User.Identity.Name).FirstOrDefault();
            if (existing != null)
            {
                TempData["InfoMessage"] = "You have already registered for an event. You can edit or delete your registration below.";
                return RedirectToAction("MyProfile");
            }

            ViewBag.EventList = GetEventData();
            ViewBag.Email = User.Identity.Name;

            // Pre-select the event chosen from the Details page (if any)
            var pv = new ParticipantCreateEditViewModel();
            if (TempData["EventId"] != null)
            {
                pv.EventId = (int)TempData["EventId"];
            }
            pv.Email = User.Identity.Name;
            return View(pv);
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ParticipantCreateEditViewModel pv)
        {
            // Guard again on POST — one registration per user
            var existing = dbContext.Participants
                    .Where(pr => pr.Email == User.Identity.Name).FirstOrDefault();
            if (existing != null)
            {
                TempData["InfoMessage"] = "You have already registered for an event.";
                return RedirectToAction("MyProfile");
            }

            if (ModelState.IsValid)
            {
                Participant p = new Participant();
                p.FullName = pv.FullName;
                p.Email = User.Identity.Name; // always the logged-in user's email
                p.Age = pv.Age;
                p.EventId = pv.EventId;
                dbContext.Participants.Add(p);
                dbContext.SaveChanges();

                var ev = dbContext.Event.Find(pv.EventId);
                TempData["SuccessMessage"] = $"Successfully registered for the event '{ev?.EventName}'!";
                return RedirectToAction("MyProfile");
            }

            ViewBag.EventList = GetEventData();
            ViewBag.Email = User.Identity.Name;
            return View(pv);
        }

        // ═══ User only — Delete own registration ═══
        [Authorize(Roles = "User")]
        public IActionResult Delete(int id)
        {
            var p = dbContext.Participants.Find(id);
            if (p == null || p.Email != User.Identity.Name) return NotFound();

            dbContext.Participants.Remove(p);
            dbContext.SaveChanges();
            TempData["SuccessMessage"] = "Your event registration has been deleted.";
            return RedirectToAction("MyProfile");
        }

        // ═══ User only — Edit own registration ═══
        [Authorize(Roles = "User")]
        public IActionResult Edit(int id)
        {
            var p = dbContext.Participants.Include("Event")
                         .Where(pr => pr.Id == id).SingleOrDefault();
            if (p == null || p.Email != User.Identity.Name) return NotFound();

            ParticipantCreateEditViewModel pv = new ParticipantCreateEditViewModel();
            pv.Id = p.Id;
            pv.FullName = p.FullName;
            pv.Age = p.Age;
            pv.Email = p.Email;
            pv.EventId = p.EventId;

            ViewBag.EventList = GetEventData();
            return View(pv);
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, ParticipantCreateEditViewModel pvModified)
        {
            var p = dbContext.Participants.Include("Event")
                         .Where(pr => pr.Id == id).SingleOrDefault();
            if (p == null || p.Email != User.Identity.Name) return NotFound();

            if (ModelState.IsValid)
            {
                p.FullName = pvModified.FullName;
                p.Age = pvModified.Age;
                p.EventId = pvModified.EventId;
                dbContext.SaveChanges();
                TempData["SuccessMessage"] = "Your registration details have been updated.";
                return RedirectToAction("MyProfile");
            }

            ViewBag.EventList = GetEventData();
            return View(pvModified);
        }

        // ═══ User only — View own profile ═══
        [Authorize(Roles = "User")]
        public IActionResult MyProfile()
        {
            var p = dbContext.Participants.Include("Event")
                         .Where(pr => pr.Email == User.Identity.Name).FirstOrDefault();
            return View(p);
        }

        // ═══ Admin only — View details of any Participant ═══
        [Authorize(Roles = "Admin")]
        public IActionResult Details(int id)
        {
            var p = dbContext.Participants.Include("Event")
                         .Where(pr => pr.Id == id).SingleOrDefault();
            if (p == null) return NotFound();
            return View(p);
        }

        // Method to populate dropdown with event details
        private SelectList GetEventData()
        {
            var events = dbContext.Event.Select(e => new
            {
                EventId = e.EventId,
                DisplayName = e.EventName + " (" + e.EventLocation + ")"
            }).ToList();

            return new SelectList(events, "EventId", "DisplayName");
        }
    }
}
