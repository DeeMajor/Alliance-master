using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace Accommodation.Models
{
    public class TruckMake
    {
        [Key]
        public long TruckMakeID { get; set; }
        [Required]
        [StringLength(maximumLength: 35, ErrorMessage = "Make Name must be at least 2 characters long", MinimumLength = 2)]
        [Display(Name = "Make Name")]
        public string MakeName { get; set; }

        [DataType(DataType.EmailAddress)]
        [StringLength(maximumLength: 50, MinimumLength = 3)]
        [Display(Name = "Manager's Email")]        
        public string CreatedBy { get; set; }
    }
}