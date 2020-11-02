using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Accommodation.Models
{
    public class Cleaners
    {
        [Key]
        public int CleanersID { get; set; }
        [Required]
        [RegularExpression(pattern: @"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Numbers and special characters are not allowed.")]
        [StringLength(maximumLength: 228, ErrorMessage = "First Name must be atleast 3 characters long", MinimumLength = 3)]
        [DisplayName("Full Name")]
        public string FullName { get; set; }
        [Required]
        [RegularExpression(pattern: @"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Numbers and special characters are not allowed.")]
        [StringLength(maximumLength: 228, ErrorMessage = "Last Name must be atleast 3 characters long", MinimumLength = 3)]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Email")]
        [DataType(dataType: DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        public string Gender { get; set; }
        [Display(Name = "Manager Email")]
        public string OwnerEmail { get; set; }
        [Display(Name = "Property Address")]
        public string buildingName { get; set; }

    }
}