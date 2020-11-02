using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Accommodation.Models
{
    public class RequestService
    {
        [Key]
        public int RequestServiceId { get; set; }
        [Display(Name = "Service Type")]
        public string serviceType { get; set; }
        [Display(Name = "Cleaner Name")]
        public string CleanerName { get; set; }
        [Display(Name = "Date Requesting For")]
        [DataType(DataType.Date)]
        public string DateRequestingFor { get; set; }
        [DataType(DataType.Time)]
        public string Time { get; set; }
        [DataType(DataType.Currency)]
        [Display(Name = "Service Price")]
        public decimal servicePrice { get; set; }
        public string Description { get; set; }
        public string CleanerEmail { get; set; }
        public string TenantEmail { get; set; }
        [Display(Name = "Date Requested")]
        public string DateRequested { get; set; }
        [Display(Name = "Service Name")]
        public string serviceName { get; set; }
        [Display(Name = "Time Slot")]
        [DataType(DataType.Time)]
        public string TimeSlot { get; set; }
    }
}