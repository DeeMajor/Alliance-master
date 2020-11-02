using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Accommodation.DAL.Interface;
using Accommodation.Models;
using Microsoft.AspNet.Identity;

namespace Accommodation.Controllers
{
    public class ManagerTimeSlotsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private IManagerTimeSlotRepository _managerTimeSlotRepository;

        public ManagerTimeSlotsController(IManagerTimeSlotRepository managerTimeSlotRepository)
        {
            _managerTimeSlotRepository = managerTimeSlotRepository;
        }
        // GET: ManagerTimeSlots
        public ActionResult Index()
        {
            var userName = User.Identity.GetUserName();
            if (User.IsInRole("Manager"))
            {
                var managerTimeSlots = _managerTimeSlotRepository.GetManagerTimeSlots().ToList().Where(x => x.ManagerEmail == userName);
                return View(managerTimeSlots.ToList());

            }
            else
            {
                var managerTimeSlots = db.managerTimeSlots.Include(m => m.Timeslot);
                return View(managerTimeSlots.ToList());

            }

        }

        // GET: ManagerTimeSlots/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ManagerTimeSlot managerTimeSlot = db.managerTimeSlots.Find(id);
            if (managerTimeSlot == null)
            {
                return HttpNotFound();
            }
            return View(managerTimeSlot);
        }

        // GET: ManagerTimeSlots/Create
        public ActionResult Create()
        {
            ViewBag.TimeSlotId = new SelectList(db.timeslots, "TimeSlotID", "TimeS");
            return View();
        }

        // POST: ManagerTimeSlots/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ManagerTimeSlotId,TimeSlotId,ManagerEmail")] ManagerTimeSlot managerTimeSlot)
        {
            if (ModelState.IsValid)
            {
                var userName = User.Identity.GetUserName();
                managerTimeSlot.ManagerEmail = userName;
                db.managerTimeSlots.Add(managerTimeSlot);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TimeSlotId = new SelectList(db.timeslots, "TimeSlotID", "TimeS", managerTimeSlot.TimeSlotId);
            return View(managerTimeSlot);
        }

        // GET: ManagerTimeSlots/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ManagerTimeSlot managerTimeSlot = db.managerTimeSlots.Find(id);
            if (managerTimeSlot == null)
            {
                return HttpNotFound();
            }
            ViewBag.TimeSlotId = new SelectList(db.timeslots, "TimeSlotID", "TimeS", managerTimeSlot.TimeSlotId);
            return View(managerTimeSlot);
        }

        // POST: ManagerTimeSlots/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ManagerTimeSlotId,TimeSlotId,ManagerEmail")] ManagerTimeSlot managerTimeSlot)
        {
            if (ModelState.IsValid)
            {
                db.Entry(managerTimeSlot).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TimeSlotId = new SelectList(db.timeslots, "TimeSlotID", "TimeS", managerTimeSlot.TimeSlotId);
            return View(managerTimeSlot);
        }

        // GET: ManagerTimeSlots/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ManagerTimeSlot managerTimeSlot = db.managerTimeSlots.Find(id);
            if (managerTimeSlot == null)
            {
                return HttpNotFound();
            }
            return View(managerTimeSlot);
        }

        // POST: ManagerTimeSlots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ManagerTimeSlot managerTimeSlot = db.managerTimeSlots.Find(id);
            db.managerTimeSlots.Remove(managerTimeSlot);
            db.SaveChanges();
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
