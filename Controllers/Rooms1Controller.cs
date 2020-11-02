using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Accommodation.Models;

namespace Accommodation.Controllers
{
    public class Rooms1Controller : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // POST: api/Rooms1
        [ResponseType(typeof(RoomBooking))]
        [HttpPost]
        public IHttpActionResult PostRoom(int? id)
        {
            RoomBooking roomBooking = db.RoomBookings.Find(id);
            roomBooking.Status = "Its Working";
            db.Entry(roomBooking).State = EntityState.Modified;
            db.SaveChanges();
            return StatusCode(HttpStatusCode.NoContent);
            //return CreatedAtRoute("DefaultApi", new { id = roomBooking.BookingId }, roomBooking);
        }

        
    }
}