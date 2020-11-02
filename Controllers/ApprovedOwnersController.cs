using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Accommodation.Models;
using Accommodation.Services.Interface;
using Microsoft.AspNet.Identity;
using PayFast;
using PayFast.AspNet;

namespace Accommodation.Controllers
{
    public class ApprovedOwnersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ISubscriptionPriceService _subscriptionPriceService;
        public ApprovedOwnersController(ISubscriptionPriceService subscriptionPriceService)
        {
            _subscriptionPriceService = subscriptionPriceService;
            this.payFastSettings = new PayFastSettings();
            this.payFastSettings.MerchantId = ConfigurationManager.AppSettings["MerchantId"];
            this.payFastSettings.MerchantKey = ConfigurationManager.AppSettings["MerchantKey"];
            this.payFastSettings.PassPhrase = ConfigurationManager.AppSettings["PassPhrase"];
            this.payFastSettings.ProcessUrl = ConfigurationManager.AppSettings["ProcessUrl"];
            this.payFastSettings.ValidateUrl = ConfigurationManager.AppSettings["ValidateUrl"];
            this.payFastSettings.ReturnUrl = ConfigurationManager.AppSettings["ReturnUrl"];
            this.payFastSettings.CancelUrl = ConfigurationManager.AppSettings["CancelUrl"];
            this.payFastSettings.NotifyUrl = ConfigurationManager.AppSettings["NotifyUrl"];
        }
        // GET: ApprovedOwners
        public ActionResult Index()
        {
            return View(db.ApprovedOwners.ToList());
        }
        public ActionResult Index1()
        {
            var email = User.Identity.GetUserName();
            return View(db.ApprovedOwners.ToList().Where(p=>p.Email ==email));
        }

        // GET: ApprovedOwners/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApprovedOwnerss approvedOwners = db.ApprovedOwners.Find(id);
            if (approvedOwners == null)
            {
                return HttpNotFound();
            }
            return View(approvedOwners);
        }
        public ActionResult Approve(int? id)
        {
            ApprovedOwnerss approvedOwners = db.ApprovedOwners.Where(p => p.ownerID == id).FirstOrDefault();
            approvedOwners.Status = "Paid";
            db.Entry(approvedOwners).State = EntityState.Modified;
            db.SaveChanges();
            TempData["AlertMessage"] = $"Status successfully updated";

            return RedirectToAction("Index1");
        }
        // GET: ApprovedOwners/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ApprovedOwners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ownerID,FullName,LastName,IDNumber,Type,Status,Email,Phone,Nationality,Gender,AltContactNumber")] ApprovedOwnerss approvedOwners)
        {
            if (ModelState.IsValid)
            {
             
                db.ApprovedOwners.Add(approvedOwners);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(approvedOwners);
        }

        // GET: ApprovedOwners/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApprovedOwnerss approvedOwners = db.ApprovedOwners.Find(id);
            if (approvedOwners == null)
            {
                return HttpNotFound();
            }
            return View(approvedOwners);
        }

        // POST: ApprovedOwners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ownerID,FullName,LastName,IDNumber,Type,Status,Email,Phone,Nationality,Gender,AltContactNumber")] ApprovedOwnerss approvedOwners)
        {
            if (ModelState.IsValid)
            {
                db.Entry(approvedOwners).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(approvedOwners);
        }

        // GET: ApprovedOwners/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApprovedOwnerss approvedOwners = db.ApprovedOwners.Find(id);
            if (approvedOwners == null)
            {
                return HttpNotFound();
            }
            return View(approvedOwners);
        }

        // POST: ApprovedOwners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ApprovedOwnerss approvedOwners = db.ApprovedOwners.Find(id);
            db.ApprovedOwners.Remove(approvedOwners);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public void ChangeStatu()
        {
            var userame = User.Identity.GetUserName();
            var dbRecord = db.ApprovedOwners.Where(x => x.Email == userame).FirstOrDefault();
            dbRecord.Status = "Paid";
            db.Entry(dbRecord).State = EntityState.Modified;
            db.SaveChanges();

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        //Payment
        #region Fields

        private readonly PayFastSettings payFastSettings;

        #endregion Fields

        #region Constructor

        //public ApprovedOwnersController()
        //{
            
        //}

        #endregion Constructor

        #region Methods



        public ActionResult Recurring()
        {
            var recurringRequest = new PayFastRequest(this.payFastSettings.PassPhrase);

            // Merchant Details
            recurringRequest.merchant_id = this.payFastSettings.MerchantId;
            recurringRequest.merchant_key = this.payFastSettings.MerchantKey;
            recurringRequest.return_url = this.payFastSettings.ReturnUrl;
            recurringRequest.cancel_url = this.payFastSettings.CancelUrl;
            recurringRequest.notify_url = this.payFastSettings.NotifyUrl;

            // Buyer Details
            recurringRequest.email_address = "sbtu01@payfast.co.za";

            // Transaction Details
            recurringRequest.m_payment_id = "8d00bf49-e979-4004-228c-08d452b86380";
            recurringRequest.amount = 20;
            recurringRequest.item_name = "Recurring Option";
            recurringRequest.item_description = "Some details about the recurring option";

            // Transaction Options
            recurringRequest.email_confirmation = true;
            recurringRequest.confirmation_address = "drnendwandwe@gmail.com";

            // Recurring Billing Details
            recurringRequest.subscription_type = SubscriptionType.Subscription;
            recurringRequest.billing_date = DateTime.Now;
            recurringRequest.recurring_amount = 20;
            recurringRequest.frequency = BillingFrequency.Monthly;
            recurringRequest.cycles = 0;

            var redirectUrl = $"{this.payFastSettings.ProcessUrl}{recurringRequest.ToString()}";

            return Redirect(redirectUrl);
        }

        public ActionResult OnceOff()
        {
            var onceOffRequest = new PayFastRequest(this.payFastSettings.PassPhrase);
            ChangeStatu();
            // Merchant Details
            onceOffRequest.merchant_id = this.payFastSettings.MerchantId;
            onceOffRequest.merchant_key = this.payFastSettings.MerchantKey;
            onceOffRequest.return_url = this.payFastSettings.ReturnUrl;
            onceOffRequest.cancel_url = this.payFastSettings.CancelUrl;
            onceOffRequest.notify_url = this.payFastSettings.NotifyUrl;

            // Buyer Details

            onceOffRequest.email_address = "sbtu01@payfast.co.za";
            
            decimal amount = _subscriptionPriceService.GetSubscriptionPrices().Select(p => p.Price).FirstOrDefault();
            // Transaction Details
            onceOffRequest.m_payment_id = "";
            onceOffRequest.amount = Convert.ToDouble(amount);
            onceOffRequest.item_name = "Subscription payment";
            onceOffRequest.item_description = "Some details about the once off payment";


            // Transaction Options
            onceOffRequest.email_confirmation = true;
            onceOffRequest.confirmation_address = "sbtu01@payfast.co.za";

            var redirectUrl = $"{this.payFastSettings.ProcessUrl}{onceOffRequest.ToString()}";

            return Redirect(redirectUrl);
        }

        public ActionResult AdHoc()
        {
            var adHocRequest = new PayFastRequest(this.payFastSettings.PassPhrase);

            // Merchant Details
            adHocRequest.merchant_id = this.payFastSettings.MerchantId;
            adHocRequest.merchant_key = this.payFastSettings.MerchantKey;
            adHocRequest.return_url = this.payFastSettings.ReturnUrl;
            adHocRequest.cancel_url = this.payFastSettings.CancelUrl;
            adHocRequest.notify_url = this.payFastSettings.NotifyUrl;

            // Buyer Details
            adHocRequest.email_address = "sbtu01@payfast.co.za";

            // Transaction Details
            adHocRequest.m_payment_id = "";
            adHocRequest.amount = 70;
            adHocRequest.item_name = "Adhoc Agreement";
            adHocRequest.item_description = "Some details about the adhoc agreement";

            // Transaction Options
            adHocRequest.email_confirmation = true;
            adHocRequest.confirmation_address = "sbtu01@payfast.co.za";

            // Recurring Billing Details
            adHocRequest.subscription_type = SubscriptionType.AdHoc;

            var redirectUrl = $"{this.payFastSettings.ProcessUrl}{adHocRequest.ToString()}";

            return Redirect(redirectUrl);
        }

        public ActionResult Return()
        {
            return View();
        }

        public ActionResult Cancel()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Notify([ModelBinder(typeof(PayFastNotifyModelBinder))] PayFastNotify payFastNotifyViewModel)
        {
            payFastNotifyViewModel.SetPassPhrase(this.payFastSettings.PassPhrase);

            var calculatedSignature = payFastNotifyViewModel.GetCalculatedSignature();

            var isValid = payFastNotifyViewModel.signature == calculatedSignature;

            System.Diagnostics.Debug.WriteLine($"Signature Validation Result: {isValid}");

            // The PayFast Validator is still under developement
            // Its not recommended to rely on this for production use cases
            var payfastValidator = new PayFastValidator(this.payFastSettings, payFastNotifyViewModel, IPAddress.Parse(this.HttpContext.Request.UserHostAddress));

            var merchantIdValidationResult = payfastValidator.ValidateMerchantId();

            System.Diagnostics.Debug.WriteLine($"Merchant Id Validation Result: {merchantIdValidationResult}");

            var ipAddressValidationResult = payfastValidator.ValidateSourceIp();

            System.Diagnostics.Debug.WriteLine($"Ip Address Validation Result: {merchantIdValidationResult}");

            // Currently seems that the data validation only works for successful payments
            if (payFastNotifyViewModel.payment_status == PayFastStatics.CompletePaymentConfirmation)
            {
                var dataValidationResult = await payfastValidator.ValidateData();

                System.Diagnostics.Debug.WriteLine($"Data Validation Result: {dataValidationResult}");
            }

            if (payFastNotifyViewModel.payment_status == PayFastStatics.CancelledPaymentConfirmation)
            {
                System.Diagnostics.Debug.WriteLine($"Subscription was cancelled");
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public ActionResult Error()
        {
            return View();
        }

        #endregion Methods
    }
}
