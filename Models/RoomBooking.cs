using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Accommodation.Models
{
    public class RoomBooking
    {
        [Key]
        public int BookingId { get; set; }
        [DisplayName("Building Name")]
        public string BuildingId { get; set; }
        public int RoomId { get; set; }
        [DisplayName("Tenant Email")]
        public string TenantEmail { get; set; }
        public string TenantId { get; set; }
        [DisplayName("Room Price"), DataType(DataType.Currency)]
        public decimal RoomPrice { get; set; }
        public string Status { get; set; }
        [DisplayName("Start Date"),DataType(DataType.Date)]
        public string DateOFBooking { get; set; }
        [DisplayName("CheckOut Date"), DataType(DataType.Date)]
        public string CheckOutDate { get; set; }
        public string CheckOutTime { get; set; }
        public virtual Room Room { get; set; }
        //public virtual Building Building { get; set; }
        [DisplayName("Building Address")]
        public string BuildingAddress { get; set; }
        public byte[] QrCodeImage { get; set; }

        ApplicationDbContext db = new ApplicationDbContext();
        public string GetBuildingName()
        {
            var roomBuilding = (from rb in db.Rooms
                                where rb.RoomId == RoomId
                                select rb.Buildings.BuildingName).FirstOrDefault();
            return roomBuilding;
        }

        public string GetBuildingAddress()
        {
            var buildingAddress = (from rb in db.Rooms
                                   where rb.RoomId == RoomId
                                   select rb.Buildings.Address).FirstOrDefault();
            return buildingAddress;
        }
    }
}

