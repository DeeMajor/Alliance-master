using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Accommodation.Models;

namespace Accommodation.Controllers
{
    public class CleanerRoastersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CleanerRoasters
        public ActionResult Index()
        {
            var cleanerRoasters = db.CleanerRoasters.Include(c => c.GetCleaners).Include(c => c.GetService);
            return View(cleanerRoasters.ToList());
        }
        public ActionResult ServiceRequest(int? id)
        {
            var cleanerRoasters = db.CleanerRoasters.Include(c => c.GetCleaners).Include(c => c.GetService);
            return View(cleanerRoasters.ToList().Where(x => x.ServiceTypeId == id));
        }
        // GET: CleanerRoasters/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CleanerRoaster cleanerRoaster = db.CleanerRoasters.Find(id);
            if (cleanerRoaster == null)
            {
                return HttpNotFound();
            }
            return View(cleanerRoaster);
        }

        // GET: CleanerRoasters/Create
        public ActionResult Create()
        {
            ViewBag.CleanersID = new SelectList(db.cleaners, "CleanersID", "FullName");
            ViewBag.ServiceId = new SelectList(db.Services, "ServiceId", "serviceName");
            return View();
        }

        // POST: CleanerRoasters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CleanerRoasterId,ServiceId,CleanersID,ServiceTypeId")] CleanerRoaster cleanerRoaster)
        {
            if (ModelState.IsValid)
            {
                cleanerRoaster.ServiceTypeId = cleanerRoaster.serviceTypeId();
                db.CleanerRoasters.Add(cleanerRoaster);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CleanersID = new SelectList(db.cleaners, "CleanersID", "FullName", cleanerRoaster.CleanersID);
            ViewBag.ServiceId = new SelectList(db.Services, "ServiceId", "serviceName", cleanerRoaster.ServiceId);
            return View(cleanerRoaster);
        }

        // GET: CleanerRoasters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CleanerRoaster cleanerRoaster = db.CleanerRoasters.Find(id);
            if (cleanerRoaster == null)
            {
                return HttpNotFound();
            }
            ViewBag.CleanersID = new SelectList(db.cleaners, "CleanersID", "FullName", cleanerRoaster.CleanersID);
            ViewBag.ServiceId = new SelectList(db.Services, "ServiceId", "serviceName", cleanerRoaster.ServiceId);
            return View(cleanerRoaster);
        }

        // POST: CleanerRoasters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CleanerRoasterId,ServiceId,CleanersID,ServiceTypeId")] CleanerRoaster cleanerRoaster)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cleanerRoaster).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CleanersID = new SelectList(db.cleaners, "CleanersID", "FullName", cleanerRoaster.CleanersID);
            ViewBag.ServiceId = new SelectList(db.Services, "ServiceId", "serviceName", cleanerRoaster.ServiceId);
            return View(cleanerRoaster);
        }

        // GET: CleanerRoasters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CleanerRoaster cleanerRoaster = db.CleanerRoasters.Find(id);
            if (cleanerRoaster == null)
            {
                return HttpNotFound();
            }
            return View(cleanerRoaster);
        }

        // POST: CleanerRoasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CleanerRoaster cleanerRoaster = db.CleanerRoasters.Find(id);
            db.CleanerRoasters.Remove(cleanerRoaster);
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
