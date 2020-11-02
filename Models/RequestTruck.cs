using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Accommodation.Models
{
    public class RequestTruck
    {
        [Key]
        public int RequestTruckId { get; set; }
        public string TruckId { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Pick Up Date")]
        public DateTime pickUpDate { get; set; }
        [DataType(DataType.Time)]
        [Display(Name = "Pick Up Time")]
        public DateTime pickUpTime { get; set; }
        public string TenantEmail { get; set; }
        [Display(Name = "Property Address")]
        public string propertyAddress { get; set; }
        [Display(Name = "My Location")]
        public string myLocation { get; set; }
        public decimal Distance { get; set; }
        public string Duration { get; set; }
        [Required]
        public string Latitude { get; set; }
        [Required]
        public string Longitude { get; set; }
        public string TruckMake { get; set; }
        public string TruckModel { get; set; }

        [DataType(DataType.Currency)]
        public decimal TotalPrice { get; set; }
        ApplicationDbContext db = new ApplicationDbContext();
        public decimal calcTotal()
        {
            decimal price = db.TruckPrices.ToList().Select(x=>x.Price).FirstOrDefault();
            return 30 * Distance;
        }
    }
}