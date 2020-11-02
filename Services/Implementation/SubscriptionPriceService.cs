using Accommodation.DAL.Interface;
using Accommodation.Models;
using Accommodation.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accommodation.Services.Implementation
{
    public class SubscriptionPriceService : ISubscriptionPriceService
    {
        private ISubscriptionPriceRepository _subscriptionPriceRepository;


        public SubscriptionPriceService(ISubscriptionPriceRepository subscriptionPriceRepository)
        {
            _subscriptionPriceRepository = subscriptionPriceRepository;
        }

        public void ChangeStatus(string ownerEmail)
        {
            throw new NotImplementedException();
        }

        public bool Delete(SubscriptionPrice subscriptionPrice)
        {
            return _subscriptionPriceRepository.Delete(subscriptionPrice);
        }

        public IEnumerable<SubscriptionPrice> Find(Func<SubscriptionPrice, bool> prdicate)
        {
            return _subscriptionPriceRepository.Find(prdicate);
        }

        public List<SubscriptionPrice> GetSubscriptionPrices()
        {
            return _subscriptionPriceRepository.GetSubscriptionPrices().ToList();
        }

        public SubscriptionPrice GetSubscriptionPrices(int id)
        {
            return _subscriptionPriceRepository.GetSubscriptionPrices(id);
        }

        public bool Insert(SubscriptionPrice subscriptionPrice)
        {
            return _subscriptionPriceRepository.Insert(subscriptionPrice);
        }

        public bool Update(SubscriptionPrice subscriptionPrice)
        {
            return _subscriptionPriceRepository.Update(subscriptionPrice);
        }
    }
}