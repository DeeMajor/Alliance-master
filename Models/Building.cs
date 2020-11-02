using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accommodation.Models
{
    public class Building
    {
        [Key]
        public int BuildingId { get; set; }
        [DisplayName("Owner Email")]
        public string OwnerEmail { get; set; }
        [DisplayName("Building Name")]
        public string BuildingName { get; set; }
        [Required]
        [Display(Name = "Street Number")]
        public string street_number { get; set; }
        [Required]
        [Display(Name = "Street Name")]
        public string route { get; set; }

        [Display(Name = "City")]
        public string locality { get; set; }
        [Display(Name = "Province")]
        public string administrative_area_level_1 { get; set; }
        [Display(Name = "Country")]
        public string country { get; set; }
        [Display(Name = "Postal Code")]
        public string postal_code { get; set; }
        [Required]
        public string Latitude { get; set; }
        [Required]
        public string Longitude { get; set; }
        [Display(Name = "Number Of Floors")]

        public int NoOfFloors { get; set; }
        [Display(Name = "Accommodation Type")]
        public string TypeOfAccommodation { get; set; }
        [Display(Name = "Number of rooms")]
        public int TotalNumberOfRooms { get; set; }
        public string Status { get; set; } //Active or Inactive
        [AllowHtml]
        [Display(Name = "Building Description")]
        public string BuildingDescription { get; set; }
        [Display(Name = "Picture")]

        public byte[] BuildingPic { get; set; }
        public string Address { get; set; }
        public ICollection <Room> Rooms { get; set; }
        public ICollection<RoomType> RoomTypes { get; set; }
      
    }
}