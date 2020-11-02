using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Accommodation.Models;
using Microsoft.AspNet.Identity;

namespace Accommodation.Controllers
{
    public class TruckMakesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TruckMakes
        public ActionResult Index()
        {
            return View(db.TruckMakes.ToList());
        }

        // GET: TruckMakes/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TruckMake truckMake = db.TruckMakes.Find(id);
            if (truckMake == null)
            {
                return HttpNotFound();
            }
            return View(truckMake);
        }

        // GET: TruckMakes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TruckMakes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TruckMakeID,MakeName,CreatedBy")] TruckMake truckMake)
        {
            if (ModelState.IsValid)
            {
                truckMake.CreatedBy = User.Identity.GetUserName();
                db.TruckMakes.Add(truckMake);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(truckMake);
        }

        // GET: TruckMakes/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TruckMake truckMake = db.TruckMakes.Find(id);
            if (truckMake == null)
            {
                return HttpNotFound();
            }
            return View(truckMake);
        }

        // POST: TruckMakes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TruckMakeID,MakeName,CreatedBy")] TruckMake truckMake)
        {
            if (ModelState.IsValid)
            {
                db.Entry(truckMake).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(truckMake);
        }

        // GET: TruckMakes/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TruckMake truckMake = db.TruckMakes.Find(id);
            if (truckMake == null)
            {
                return HttpNotFound();
            }
            return View(truckMake);
        }

        // POST: TruckMakes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            TruckMake truckMake = db.TruckMakes.Find(id);
            db.TruckMakes.Remove(truckMake);
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
