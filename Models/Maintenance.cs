using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accommodation.Models
{
    public class Maintenance
    {
        [Key]
        public int MaintainanceId { get; set; }
        [DisplayName("Tenant Name")]
        public string TenantName { get; set; }
        public string TenantEmail { get; set; }
        [DisplayName("Date Reported")]
        public DateTime? ReportDate { get; set; }
        [DisplayName("Date Fixed")]
        public DateTime? FixedDate { get; set; }
        [DisplayName("Tenant's Comment")]
        public string Comments { get; set; }
        [DisplayName("Property")]
        public string propertyName { get; set; }
        public string Status { get; set; }
        public byte[] Image { get; set; }
    }
}