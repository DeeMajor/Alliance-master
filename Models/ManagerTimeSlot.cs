using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Accommodation.Models
{
    public class ManagerTimeSlot
    {
        [Key]
        public int ManagerTimeSlotId { get; set; }
        public int TimeSlotId { get; set; }

        public string ManagerEmail { get; set; }

        public virtual timeslot Timeslot { get; set; }
    }
}