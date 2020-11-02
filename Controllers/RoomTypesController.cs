using Accommodation.Models;
using Accommodation.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;

namespace Accommodation.Controllers
{
    public class RoomTypesController : Controller
    {
        private IRoomTypeService _roomTypeService;
        public RoomTypesController(IRoomTypeService roomTypeService)
        {
            _roomTypeService = roomTypeService;
        }
        // GET: RoomType
        public ActionResult Index()
        {
            return View(_roomTypeService.GetRoomTypes());
        }

        // GET: RoomType/Details/5
        public ActionResult Details(int id)
        {
            return View(_roomTypeService.GetRoomTypes(id));
        }

        // GET: RoomType/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RoomType/Create
        [HttpPost]
        public ActionResult Create(RoomType roomType)
        {
            try
            {
                roomType.RoomAvailable = roomType.NumOfRooms;
                if (_roomTypeService.Insert(roomType))
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

        // GET: RoomType/Edit/5
        public ActionResult Edit(int id)
        {
            if (!String.IsNullOrEmpty(id.ToString()))
            {
                try
                {
                    return View(_roomTypeService.GetRoomTypes(id));
                }
                catch { }
            }
            return View();
        }

        // POST: RoomType/Edit/5
        [HttpPost]
        public ActionResult Edit(RoomType roomType)
        {
            try
            {
                if (_roomTypeService.Update(roomType))
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

        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomType roomtype = _roomTypeService.GetRoomTypes().Find(p => p.roomtypeId == id);
            if (roomtype == null)
            {
                return HttpNotFound();
            }
            return View(roomtype);
        }

        // POST: SubscriptionPrices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                RoomType roomtype = _roomTypeService.GetRoomTypes().Find(p => p.roomtypeId == id);
                _roomTypeService.Delete(roomtype);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }

        }
    }
}
