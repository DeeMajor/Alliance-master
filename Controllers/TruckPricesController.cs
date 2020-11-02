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
    public class TruckPricesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TruckPrices
        public ActionResult Index()
        {
            return View(db.TruckPrices.ToList());
        }

        // GET: TruckPrices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TruckPrice truckPrice = db.TruckPrices.Find(id);
            if (truckPrice == null)
            {
                return HttpNotFound();
            }
            return View(truckPrice);
        }

        // GET: TruckPrices/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TruckPrices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TruckPriceId,Price")] TruckPrice truckPrice)
        {
            if (ModelState.IsValid)
            {
                db.TruckPrices.Add(truckPrice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(truckPrice);
        }

        // GET: TruckPrices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TruckPrice truckPrice = db.TruckPrices.Find(id);
            if (truckPrice == null)
            {
                return HttpNotFound();
            }
            return View(truckPrice);
        }

        // POST: TruckPrices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TruckPriceId,Price")] TruckPrice truckPrice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(truckPrice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(truckPrice);
        }

        // GET: TruckPrices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TruckPrice truckPrice = db.TruckPrices.Find(id);
            if (truckPrice == null)
            {
                return HttpNotFound();
            }
            return View(truckPrice);
        }

        // POST: TruckPrices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TruckPrice truckPrice = db.TruckPrices.Find(id);
            db.TruckPrices.Remove(truckPrice);
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
