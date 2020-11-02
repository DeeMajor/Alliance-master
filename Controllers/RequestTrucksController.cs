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
    public class RequestTrucksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: RequestTrucks
        public ActionResult Index()
        {
            return View(db.RequestTrucks.ToList());
        }
        
        // GET: RequestTrucks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestTruck requestTruck = db.RequestTrucks.Find(id);
            if (requestTruck == null)
            {
                return HttpNotFound();
            }
            return View(requestTruck);
        }

        // GET: RequestTrucks/Create
        public ActionResult Create(string id)
        {
            Session["id"] = id.ToString();
            var username = User.Identity.GetUserName();
            var address = db.RoomBookings.Where(x => x.TenantEmail == username).Select(x=>x.BuildingAddress).FirstOrDefault();
            ViewBag.propertyName = address;
            return View();
        }

        // POST: RequestTrucks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RequestTruckId,pickUpDate,pickUpTime,TenantEmail,propertyAddress,myLocation,Distance,Duration,Latitude,Longitude,TotalPrice")] RequestTruck requestTruck)
        {
            var id = Session["id"].ToString();
            Truck truck = db.Trucks.Find(id);

            var userName = User.Identity.GetUserName();

            if (ModelState.IsValid)
            {
                requestTruck.TruckModel = truck.TruckModel.ModelName;
                requestTruck.TruckMake = truck.TruckMake.MakeName;
                requestTruck.TruckId = Convert.ToString(id);
                requestTruck.TenantEmail = userName;
                requestTruck.TotalPrice = requestTruck.calcTotal();
                db.RequestTrucks.Add(requestTruck);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(requestTruck);
        }

        // GET: RequestTrucks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestTruck requestTruck = db.RequestTrucks.Find(id);
            if (requestTruck == null)
            {
                return HttpNotFound();
            }
            return View(requestTruck);
        }

        // POST: RequestTrucks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RequestTruckId,pickUpDate,pickUpTime,TenantEmail,propertyAddress,myLocation,Distance,Duration,Latitude,Longitude")] RequestTruck requestTruck)
        {
            if (ModelState.IsValid)
            {
                db.Entry(requestTruck).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(requestTruck);
        }

        // GET: RequestTrucks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestTruck requestTruck = db.RequestTrucks.Find(id);
            if (requestTruck == null)
            {
                return HttpNotFound();
            }
            return View(requestTruck);
        }

        // POST: RequestTrucks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RequestTruck requestTruck = db.RequestTrucks.Find(id);
            db.RequestTrucks.Remove(requestTruck);
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
