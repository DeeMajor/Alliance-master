using Accommodation.DAL.Interface;
using Accommodation.Models;
using Accommodation.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accommodation.DAL.Implementation
{
    public class SubscriptionPriceRepository : ISubscriptionPriceRepository
    {
        private DatabaseService<SubscriptionPrice> _databaseService;
        public SubscriptionPriceRepository(DatabaseService<SubscriptionPrice> databaseService)
        {
            _databaseService = databaseService;
        }
        public bool Delete(SubscriptionPrice subscriptionPrice)
        {
            return _databaseService.Delete(subscriptionPrice);
        }

        public IEnumerable<SubscriptionPrice> Find(Func<SubscriptionPrice, bool> prdicate)
        {
            return _databaseService.Find(prdicate);
        }

        public List<SubscriptionPrice> GetSubscriptionPrices()
        {
            return _databaseService.Get().ToList();
        }

        public SubscriptionPrice GetSubscriptionPrices(int id)
        {
            return _databaseService.Get(id);
        }

        public bool Insert(SubscriptionPrice subscriptionPrice)
        {
            return _databaseService.Insert(subscriptionPrice);
        }

        public bool Update(SubscriptionPrice subscriptionPrice)
        {
            return _databaseService.Update(subscriptionPrice);
        }
    }
}