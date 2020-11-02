using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Accommodation.Models
{
    public class SubscriptionPrice
    {
        [Key]
        public int SubscriptionPriceId { get; set; }
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
    }
}