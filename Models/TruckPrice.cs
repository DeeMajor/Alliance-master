using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Accommodation.Models
{
    public class TruckPrice
    {
        [Key]
        public int TruckPriceId { get; set; }
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
    }
}