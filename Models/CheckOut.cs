using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Accommodation.Models
{
    public class CheckOut
    {
        [Key]
        public int CheckOutId { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Check Out Date")]
        public string checkOutDate { get; set; }
        [DataType(DataType.Time)]
        [Display(Name = "Check Out Time")]
        public string checkOutTime { get; set; }
        public string tenantEmail { get; set; }
    }
}