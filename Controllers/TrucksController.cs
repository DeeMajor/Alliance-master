using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Accommodation.Models;
using Microsoft.AspNet.Identity;

namespace Accommodation.Controllers
{
    public class TrucksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Trucks
        public ActionResult Index()
        {
            var trucks = db.Trucks.Include(t => t.TruckMake).Include(t => t.TruckModel);
            return View(trucks.ToList());
        }
        public ActionResult TruckRequest()
        {
            var trucks = db.Trucks.Include(t => t.TruckMake).Include(t => t.TruckModel);
            return View(trucks.ToList());
        }
        // GET: Trucks/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Truck truck = db.Trucks.Find(id);
            if (truck == null)
            {
                return HttpNotFound();
            }
            return View(truck);
        }

        // GET: Trucks/Create
        public ActionResult Create()
        {
            ViewBag.TruckMakeID = new SelectList(db.TruckMakes, "TruckMakeID", "MakeName");
            ViewBag.TruckModelID = new SelectList(db.TruckModels, "TruckModelID", "ModelName");
            return View();
        }

        // POST: Trucks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TaxiID,Image,TruckMakeID,TruckModelID,CreatedBy,landlordEmail,PropertyAddress")] Truck truck, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                truck.Image = ConvertToBytes(file);
            }
            var userName = User.Identity.GetUserName();
            var id = db.Managers.Where(x => x.Email == userName).Select(x => x.ManagerId).FirstOrDefault();
            var bId = db.ManagerBuildings.Where(x => x.ManagerId == id).Select(x => x.BuildingId).FirstOrDefault();
            var address = db.buildings.Where(x => x.BuildingId == bId).Select(x => x.Address).FirstOrDefault();
            if (ModelState.IsValid)
            {
                truck.PropertyAddress = address;
                truck.CreatedBy = userName;
                db.Trucks.Add(truck);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TruckMakeID = new SelectList(db.TruckMakes, "TruckMakeID", "MakeName", truck.TruckMakeID);
            ViewBag.TruckModelID = new SelectList(db.TruckModels, "TruckModelID", "ModelName", truck.TruckModelID);
            return View(truck);
        }
        public byte[] ConvertToBytes(HttpPostedFileBase file)
        {
            BinaryReader reader = new BinaryReader(file.InputStream);
            return reader.ReadBytes((int)file.ContentLength);
        }
        // GET: Trucks/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Truck truck = db.Trucks.Find(id);
            if (truck == null)
            {
                return HttpNotFound();
            }
            ViewBag.TruckMakeID = new SelectList(db.TruckMakes, "TruckMakeID", "MakeName", truck.TruckMakeID);
            ViewBag.TruckModelID = new SelectList(db.TruckModels, "TruckModelID", "ModelName", truck.TruckModelID);
            return View(truck);
        }

        // POST: Trucks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TaxiID,Image,TruckMakeID,TruckModelID,CreatedBy,landlordEmail,PropertyAddress")] Truck truck)
        {
            if (ModelState.IsValid)
            {
                db.Entry(truck).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TruckMakeID = new SelectList(db.TruckMakes, "TruckMakeID", "MakeName", truck.TruckMakeID);
            ViewBag.TruckModelID = new SelectList(db.TruckModels, "TruckModelID", "ModelName", truck.TruckModelID);
            return View(truck);
        }

        // GET: Trucks/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Truck truck = db.Trucks.Find(id);
            if (truck == null)
            {
                return HttpNotFound();
            }
            return View(truck);
        }

        // POST: Trucks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Truck truck = db.Trucks.Find(id);
            db.Trucks.Remove(truck);
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
