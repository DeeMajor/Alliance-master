using Accommodation.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accommodation.Controllers
{
    public class HomeController : Controller
    {
        private IBuildingService _buildingService;
        public HomeController(IBuildingService buildingService)
        {
            _buildingService = buildingService;
        }
        //public ActionResult Index(string search)
        //{
        //    return View(_buildingService.GetBuildings().Where(x => x.locality.Contains(search)).ToList());
        //}
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ClientHome()
        {
            return View();
        }
        public ActionResult tenant()
        {
            return View();
        }
        public ActionResult Thankyou()
        {
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Galary()
        {
            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        
        public ActionResult Success()
        {
            return View();
        }
    }
}