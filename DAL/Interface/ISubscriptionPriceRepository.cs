using Accommodation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accommodation.DAL.Interface
{
   public interface ISubscriptionPriceRepository
    {
        List<SubscriptionPrice> GetSubscriptionPrices();
        SubscriptionPrice GetSubscriptionPrices(int id);
        bool Insert(SubscriptionPrice subscriptionPrice);
        bool Update(SubscriptionPrice subscriptionPrice);
        bool Delete(SubscriptionPrice subscriptionPrice);
        IEnumerable<SubscriptionPrice> Find(Func<SubscriptionPrice, bool> prdicate);
    }
}
