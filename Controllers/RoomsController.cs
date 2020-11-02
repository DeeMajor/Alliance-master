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
    public class RoomsController : Controller
    {
        private IRoomService _roomService;
        private IRoomTypeService _roomTypeService;
        private IBuildingService _buildingService;
        //private string userName;
        
        public RoomsController()
        {
            //if (User.Identity.IsAuthenticated == true)
            //{
            //    userName = User.Identity.GetUserName();

            //}
        }
        public RoomsController (IRoomService roomService,IRoomTypeService roomTypeService ,IBuildingService buildingService)
        {
            _roomService = roomService;
            _roomTypeService = roomTypeService;
            _buildingService = buildingService;
        }
        // GET: Rooms
        public ActionResult Index()
        {
            var userName = User.Identity.GetUserName();
            if (User.IsInRole("Landlord"))
            {
                int buildingId = _buildingService.getBuildingId(userName);
                return View(_roomService.GetRooms().Where(x=>x.BuildingId==buildingId));

            }
            else
            {
                return View(_roomService.GetRooms());

            }
        }

        public ActionResult RoomsBooking(int? id)
        {
            return View(_roomService.GetRooms().Where(p=>p.BuildingId ==id && p.Status==null || String.IsNullOrEmpty(p.Status)));
        }
        public ActionResult RoomsBooking2()
        {
            return View(_roomService.GetRooms());
        }

        // GET: Rooms/Details/5
        public ActionResult Details(int id)
        {
            return View(_roomService.GetRooms(id));
        }

        // GET: Rooms/Create
        public ActionResult Create()
        {
            var userName = User.Identity.GetUserName();
            ViewBag.roomtypeId = new SelectList(_roomTypeService.GetRoomTypes(), "roomtypeId", "Type");
            ViewBag.BuildingId = new SelectList(_buildingService.GetBuildings().Where(x=>x.OwnerEmail== userName), "BuildingId", "BuildingName");
            return View();
        }

        // POST: Rooms/Create
        [HttpPost]
        public ActionResult Create (Room room, HttpPostedFileBase photoUpload)
        {
            var userName = User.Identity.GetUserName();

            try
            {
                
                byte[] photo = null;
                photo = new byte[photoUpload.ContentLength];
                photoUpload.InputStream.Read(photo, 0, photoUpload.ContentLength);
                room.RoomPicture = photo;

                var result = _buildingService.GetBuildings().Where(p => p.BuildingId == room.BuildingId).Select(p => p.BuildingName).FirstOrDefault();
                var result1 = _buildingService.GetBuildings().Where(p => p.BuildingId == room.BuildingId).Select(p => p.TotalNumberOfRooms).FirstOrDefault();

                if (_roomService.ChceckPoeple(room.roomtypeId, room.NoOfPeople))
                {
                    Random a = new Random();

                    room.RoomNumber = result.Substring(0, 3) + a.Next(0, result1);
                    room.NoOfPeople = _roomService.getNumberOfTenants(room.roomtypeId);
                    room.Status = null;
                    if (_roomService.validateNumbers(room))  // here i  am validating the numbers ukuthi they not negative
                    {
                        if (_roomService.Insert(room))
                        {

                            ViewBag.roomtypeId = new SelectList(_roomTypeService.GetRoomTypes(), "roomtypeId", "Type");
                            ViewBag.BuildingId = new SelectList(_buildingService.GetBuildings().Where(x => x.OwnerEmail == userName), "BuildingId", "BuildingName");
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "One or more values were less tha 0 , please make sure all values are greater than 0");
                        ViewBag.roomtypeId = new SelectList(_roomTypeService.GetRoomTypes(), "roomtypeId", "Type");
                        ViewBag.BuildingId = new SelectList(_buildingService.GetBuildings().Where(x => x.OwnerEmail == userName), "BuildingId", "BuildingName");
                        return View(room);
                    }
               
                }
                else
                {
                     ModelState.AddModelError("", "Number of poeple exceeded room type quantity");
                    ViewBag.roomtypeId = new SelectList(_roomTypeService.GetRoomTypes(), "roomtypeId", "Type");
                    ViewBag.BuildingId = new SelectList(_buildingService.GetBuildings().Where(x => x.OwnerEmail == userName), "BuildingId", "BuildingName");
                    return View(room);
                }

               
            }
            catch
            {
                ViewBag.roomtypeId = new SelectList(_roomTypeService.GetRoomTypes(), "roomtypeId", "Type");
                ViewBag.BuildingId = new SelectList(_buildingService.GetBuildings().Where(x => x.OwnerEmail == userName), "BuildingId", "BuildingName");
                return View(room);
            }
            ViewBag.roomtypeId = new SelectList(_roomTypeService.GetRoomTypes(), "roomtypeId", "Type");
            ViewBag.BuildingId = new SelectList(_buildingService.GetBuildings().Where(x => x.OwnerEmail == userName), "BuildingId", "BuildingName");
            return View(room);
        }

        // GET: Rooms/Edit/5
        public ActionResult Edit(int id)
        {
            if (!String.IsNullOrEmpty(id.ToString()))
            {
                try
                {
                    return View(_roomService.GetRooms(id));
                }
                catch { }
            }
            return View();
        }

        // POST: Rooms/Edit/5
        [HttpPost]
        public ActionResult Edit(Room room)
        {
            try
            {
                if (_roomService.Update(room))
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

        // GET: Rooms/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room rooms = _roomService.GetRooms().Find(p => p.RoomId == id);
            if (rooms == null)
            {
                return HttpNotFound();
            }
            return View(rooms);
        }

        // POST: SubscriptionPrices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Room rooms = _roomService.GetRooms().Find(p => p.RoomId == id);
                _roomService.Delete(rooms);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }

        }
    }
}
