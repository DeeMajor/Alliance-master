using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Accommodation.Models
{
    public class TruckModel
    {
        [Key]
        public long TruckModelID { get; set; }
        [Required]
        [StringLength(maximumLength: 35, ErrorMessage = "Model Name must be at least 2 characters long", MinimumLength = 2)]
        [Display(Name = "Model Name")]
        public string ModelName { get; set; }

        [DataType(DataType.EmailAddress)]
        [StringLength(maximumLength: 50, MinimumLength = 3)]
        [Display(Name = "Manager's Email")]
        public string CreatedBy { get; set; }
    }
}