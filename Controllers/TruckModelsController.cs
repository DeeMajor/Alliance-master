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
    public class TruckModelsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TruckModels
        public ActionResult Index()
        {
            return View(db.TruckModels.ToList());
        }

        // GET: TruckModels/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TruckModel truckModel = db.TruckModels.Find(id);
            if (truckModel == null)
            {
                return HttpNotFound();
            }
            return View(truckModel);
        }

        // GET: TruckModels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TruckModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TruckModelID,ModelName,CreatedBy")] TruckModel truckModel)
        {
            if (ModelState.IsValid)
            {
                truckModel.CreatedBy = User.Identity.GetUserName();

                db.TruckModels.Add(truckModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(truckModel);
        }

        // GET: TruckModels/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TruckModel truckModel = db.TruckModels.Find(id);
            if (truckModel == null)
            {
                return HttpNotFound();
            }
            return View(truckModel);
        }

        // POST: TruckModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TruckModelID,ModelName,CreatedBy")] TruckModel truckModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(truckModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(truckModel);
        }

        // GET: TruckModels/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TruckModel truckModel = db.TruckModels.Find(id);
            if (truckModel == null)
            {
                return HttpNotFound();
            }
            return View(truckModel);
        }

        // POST: TruckModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            TruckModel truckModel = db.TruckModels.Find(id);
            db.TruckModels.Remove(truckModel);
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
