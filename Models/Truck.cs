using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accommodation.Models
{
    public class Truck
    {
        [Key]
        [Required]
        [Display(Name = "Registration Number")]
        public string TaxiID { get; set; }
        public byte[] Image { get; set; }
        [Required]
        [ForeignKey("TruckMake")]
        [Display(Name = "Make Name")]
        public long TruckMakeID { get; set; }
        [Required]
        [ForeignKey("TruckModel")]
        [Display(Name = "Model Name")]
        public long TruckModelID { get; set; }    
        [DataType(DataType.EmailAddress)]
        [StringLength(maximumLength: 50, MinimumLength = 3)]
        [Display(Name = "Manager's Email")]
        public string CreatedBy { get; set; }
        public string landlordEmail { get; set; }
        public string PropertyAddress { get; set; }
        public virtual TruckMake TruckMake { get; set; }
        public virtual TruckModel TruckModel { get; set; }
    }
}