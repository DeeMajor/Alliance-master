using Accommodation.Models;
using Accommodation.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Accommodation.Controllers
{
    public class SubscriptionPricesController : Controller
    {
        private ISubscriptionPriceService _subscriptionPriceService;
        public SubscriptionPricesController(ISubscriptionPriceService subscriptionPriceService)
        {
            _subscriptionPriceService = subscriptionPriceService;
        }
        // GET: SubscriptionPrices
        public ActionResult Index()
        {
            return View(_subscriptionPriceService.GetSubscriptionPrices());
        }

        // GET: SubscriptionPrices/Details/5
        public ActionResult Details(int id)
        {
            return View(_subscriptionPriceService.GetSubscriptionPrices(id));
        }

        // GET: SubscriptionPrices/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SubscriptionPrices/Create
        [HttpPost]
        public ActionResult Create(SubscriptionPrice subscriptionPrice )
        {
            try
            {
                if (_subscriptionPriceService.Insert(subscriptionPrice))
                {
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return View();
            }
            return View();
        }

        // GET: SubscriptionPrices/Edit/5
        public ActionResult Edit(int id)
        {
            if (!String.IsNullOrEmpty(id.ToString()))
            {
                try
                {
                    return View(_subscriptionPriceService.GetSubscriptionPrices(id));
                }
                catch { }
            }
            return View();
        }

        // POST: SubscriptionPrices/Edit/5
        [HttpPost]
        public ActionResult Edit(SubscriptionPrice subscriptionPrice)
        {
            try
            {
                if (_subscriptionPriceService.Update(subscriptionPrice))
                {
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return View();
            }
            return View();

        }

        // GET: SubscriptionPrices/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubscriptionPrice rooms = _subscriptionPriceService.GetSubscriptionPrices().Find(p => p.SubscriptionPriceId == id);
            if (rooms == null)
            {
                return HttpNotFound();
            }
            return View(rooms);
        }

        // POST: SubscriptionPrices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                SubscriptionPrice subscriptionPrice = _subscriptionPriceService.GetSubscriptionPrices().Find(p => p.SubscriptionPriceId == id);
                _subscriptionPriceService.Delete(subscriptionPrice);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }

        }
    }
}
