using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Accommodation.Models
{
    public class ManagerBuilding
    {
        [Key]
        public int ManagerBuildingId { get; set; }
        public int ManagerId { get; set; }
        public int BuildingId { get; set; }
        public virtual Building Building { get; set; }
        public virtual Manager Manager { get; set; }
    }
}