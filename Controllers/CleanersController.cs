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
    public class CleanersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Cleaners
        public ActionResult Index()
        {
            return View(db.cleaners.ToList());
        }

        // GET: Cleaners/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cleaners cleaners = db.cleaners.Find(id);
            if (cleaners == null)
            {
                return HttpNotFound();
            }
            return View(cleaners);
        }

        // GET: Cleaners/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cleaners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CleanersID,FullName,LastName,Email,Phone,Gender,OwnerEmail")] Cleaners cleaners)
        {
            if (ModelState.IsValid)
            {
                db.cleaners.Add(cleaners);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cleaners);
        }

        // GET: Cleaners/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cleaners cleaners = db.cleaners.Find(id);
            if (cleaners == null)
            {
                return HttpNotFound();
            }
            return View(cleaners);
        }

        // POST: Cleaners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CleanersID,FullName,LastName,Email,Phone,Gender,OwnerEmail,buildingName")] Cleaners cleaners)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cleaners).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cleaners);
        }

        // GET: Cleaners/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cleaners cleaners = db.cleaners.Find(id);
            if (cleaners == null)
            {
                return HttpNotFound();
            }
            return View(cleaners);
        }

        // POST: Cleaners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cleaners cleaners = db.cleaners.Find(id);
            db.cleaners.Remove(cleaners);
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
