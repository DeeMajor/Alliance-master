using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Accommodation.Models
{
    public class CleanerRoaster
    {
        [Key]
        public int CleanerRoasterId { get; set; }
        public int ServiceId { get; set; }
        public virtual Service GetService { get; set; }
        public int CleanersID { get; set; }
        public virtual Cleaners GetCleaners { get; set; }
        public int ServiceTypeId { get; set; }
        
        ApplicationDbContext dbContext = new ApplicationDbContext();
        public int serviceTypeId()
        {
            var price = (from s in dbContext.Services
                         where ServiceId == s.ServiceId
                         select s.ServiceTypeId).FirstOrDefault();
            return price;
        }
        public decimal servicePrice()
        {
            var price = (from s in dbContext.Services
                         where ServiceId == s.ServiceId
                         select s.servicePrice).FirstOrDefault();
            return price;
        }
        public string Description()
        {
            var description = (from s in dbContext.Services
                         where ServiceId == s.ServiceId
                         select s.Description).FirstOrDefault();
            return description;
        }
        public string serviceName()
        {
            var description = (from s in dbContext.Services
                               where ServiceId == s.ServiceId
                               select s.serviceName).FirstOrDefault();
            return description;
        }
    }
}