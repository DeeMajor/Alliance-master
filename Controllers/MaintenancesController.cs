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
    public class MaintenancesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Maintenances
        public ActionResult Index()
        {
            var userName = User.Identity.GetUserName();
            return View(db.Maintenances.ToList().Where(x=>x.TenantEmail == userName));
        }
        public ActionResult AdminIndex()
        {
            return View(db.Maintenances.ToList());
        }

        // GET: Maintenances/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Maintenance maintenance = db.Maintenances.Find(id);
            if (maintenance == null)
            {
                return HttpNotFound();
            }
            return View(maintenance);
        }

        public ActionResult Seen(int? id)
        {
            Maintenance maintainance = db.Maintenances.Find(id);
            maintainance.Status = "Seen";
            db.Entry(maintainance).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("AdminIndex");
        }
        public ActionResult Done(int? id)
        {
            Maintenance maintainance = db.Maintenances.Find(id);
            maintainance.Status = "Fixed";
            maintainance.FixedDate = DateTime.Now.Date;
            db.Entry(maintainance).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        // GET: Maintenances/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Maintenances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaintainanceId,TenantName,TenantEmail,ReportDate,FixedDate,Comments,propertyName,Status,Image")] Maintenance maintenance, HttpPostedFileBase file)
        {
            var userName = User.Identity.GetUserName();
            var cleanerName = db.Tenants.Where(x=>x.Email == userName).Select(x=>x.FullName).FirstOrDefault();
            var buildingName = db.RoomBookings.Where(x=>x.TenantEmail == userName).Select(x=>x.BuildingAddress).FirstOrDefault();

            if (file != null && file.ContentLength > 0)
            {
                maintenance.Image = ConvertToBytes(file);
            }
            if (ModelState.IsValid)
            {
                maintenance.ReportDate = DateTime.Now;
                maintenance.TenantName = cleanerName;
                maintenance.propertyName = buildingName;
                maintenance.Status = "Awaiting";
                maintenance.TenantEmail = userName;
                db.Maintenances.Add(maintenance);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(maintenance);
        }
        public byte[] ConvertToBytes(HttpPostedFileBase file)
        {
            BinaryReader reader = new BinaryReader(file.InputStream);
            return reader.ReadBytes((int)file.ContentLength);
        }
        // GET: Maintenances/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Maintenance maintenance = db.Maintenances.Find(id);
            if (maintenance == null)
            {
                return HttpNotFound();
            }
            return View(maintenance);
        }

        // POST: Maintenances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaintainanceId,TenantName,TenantEmail,ReportDate,FixedDate,Comments,propertyName,Status,Image")] Maintenance maintenance)
        {
            if (ModelState.IsValid)
            {
                db.Entry(maintenance).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(maintenance);
        }

        // GET: Maintenances/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Maintenance maintenance = db.Maintenances.Find(id);
            if (maintenance == null)
            {
                return HttpNotFound();
            }
            return View(maintenance);
        }

        // POST: Maintenances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Maintenance maintenance = db.Maintenances.Find(id);
            db.Maintenances.Remove(maintenance);
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
