using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Accommodation.Models
{
    public class Service
    {
        [Key]
        public int ServiceId { get; set; }
        [Display(Name = "Service Type")]
        public int ServiceTypeId { get; set; }
        public virtual ServiceType ServiceTypes { get; set; }
        [Display(Name = "Service Price"), DataType(DataType.Currency)]
        public decimal servicePrice { get; set; }
        public string Description { get; set; }
        [Display(Name = "Service Name")]
        public string serviceName { get; set; }
        public string CreatedBy { get; set; }
        ApplicationDbContext dbContext = new ApplicationDbContext();
        public string getType()
        {
            var type = (from st in dbContext.ServiceTypes
                        where ServiceTypeId == st.ServiceTypeId
                        select st.Type).FirstOrDefault();
            return type;
        }
    }
}