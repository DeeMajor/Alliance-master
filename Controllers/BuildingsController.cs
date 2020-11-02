using Accommodation.Models;
using Accommodation.Services.Interface;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Accommodation.Controllers
{
    public class BuildingsController : Controller
    {
        private IBuildingService _buildingService;
        private ApplicationDbContext _context;
        public BuildingsController(IBuildingService buildingService, ApplicationDbContext context)
        {
            _context = context;
            _buildingService = buildingService;
        }
        // GET: Buildings
        public ActionResult Index()
        {
            var userName = User.Identity.GetUserName();
            if (User.IsInRole("Landlord"))
            {
                return View(_buildingService.GetBuildings().Where(x=>x.OwnerEmail==userName));

            }
            else
            {
                return View(_buildingService.GetBuildings());

            }
        }

        public ActionResult Index2(string search)
        {
                return View(_buildingService.GetBuildings().Where(x => x.locality.Contains(search)).ToList());
        }
        public ActionResult Index3()
        {
            return View(_buildingService.GetBuildings());
        }
        public ActionResult Index1()
        {
            return View(_buildingService.GetBuildings());

        }

        // GET: Buildings/Details/5
        public ActionResult Details(int id)
        {
            return View(_buildingService.GetBuildings(id));
        }

        // GET: Buildings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Buildings/Create
        [HttpPost]
        public ActionResult Create(Building building, HttpPostedFileBase photoUpload)
        {
            var email = User.Identity.GetUserName();

            var status = _context.ApprovedOwners.Where(p => p.Email == email).Select(p => p.Status).FirstOrDefault();
            try
            {
                if(status == "Paid")
                {
                    var userName = User.Identity.GetUserName();
                    byte[] photo = null;
                    photo = new byte[photoUpload.ContentLength];
                    photoUpload.InputStream.Read(photo, 0, photoUpload.ContentLength);
                    building.BuildingPic = photo;
                    building.OwnerEmail = userName;
                    building.Address = ($"{building.street_number}, {building.route}, {building.locality}, {building.administrative_area_level_1}, {building.country}");
                    if (_buildingService.Insert(building))
                    {
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    TempData["AlertMessage"] = "Please pay subscription first before adding a building";
                }


            }
            catch
            {
                return View();
            }
            return View();
        }

        // GET: Buildings/Edit/5
        public ActionResult Edit(int id)
        {
            if (!String.IsNullOrEmpty(id.ToString()))
            {
                try
                {
                    return View(_buildingService.GetBuildings(id));
                }
                catch { }
            }
            return View();
        }

        // POST: Buildings/Edit/5
        [HttpPost]
        public ActionResult Edit(Building building)
        {
            try
            {
                if (_buildingService.Update(building))
                {
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return View();
            }
            return View();
        }

        // GET: Buildings/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Building building = _buildingService.GetBuildings().Find(p => p.BuildingId == id);
            if (building == null)
            {
                return HttpNotFound();
            }
            return View(building);
        }

        // POST: SubscriptionPrices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Building building = _buildingService.GetBuildings().Find(p => p.BuildingId == id);
                _buildingService.Delete(building);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }

        }
    }
}
