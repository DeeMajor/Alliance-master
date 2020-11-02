using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Accommodation.Models
{
    public class timeslot
    {
        [Key]
        public int TimeSlotID { get; set; }
        [DisplayName("Time")]
        [DataType(DataType.Time)]
        public string TimeS { get; set; }
    }
}