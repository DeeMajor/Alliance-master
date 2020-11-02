using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accommodation.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public int ManagerId { get; set; }
        [DisplayName("Tenant Email")]
        public string email { get; set; }
        [DisplayName("Appointment Date")]
        [DataType(DataType.Date)]
        public DateTime ADate { get; set; }
        [DisplayName("Date Booked")]
        public DateTime DateBooked { get; set; }
        public int TimeSlotID { get; set; }
        public string Status { get; set; }
        [DisplayName("Building Address")]
        public string BuildingAddress { get; set; }
        public virtual Manager Managers { get; set; }
        [DisplayName("Time Slot")]
        public virtual timeslot Timeslot { get; set; }

        ApplicationDbContext db = new ApplicationDbContext();

       public string getBuildingAddress()
        {
            var buildingAddress = (from b in db.ManagerBuildings
                              where b.ManagerId == ManagerId
                              select b.Building.Address).FirstOrDefault();
            return buildingAddress;
        }
    }
}