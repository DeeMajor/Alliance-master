using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Accommodation.Models;
using Accommodation.Services.Implementation;
using Accommodation.Services.Interface;
using Microsoft.AspNet.Identity;
using PayFast;
using PayFast.AspNet;

namespace Accommodation.Controllers
{
    public class OwnersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;
        private ISubscriptionPriceService _subscriptionPriceService;


        public OwnersController(ApplicationUserManager userManager, ISubscriptionPriceService subscriptionPriceService)
        {
            _userManager = userManager;
            _subscriptionPriceService = subscriptionPriceService;

        }
        // GET: Owners
        public ActionResult Index()
        {
            var userName = User.Identity.GetUserName();
            var owners = db.owners.Where(x => x.Email == userName);
            return View(owners.ToList());
        }
        public ActionResult AdminIndex()
        {
            return View(db.owners.ToList());
        }
        public ActionResult Download(int? id)
        {
            MemoryStream ms = null;

            var item = db.owners.FirstOrDefault(x => x.ownerID == id);
            if (item != null)
            {
                ms = new MemoryStream(item.FileContent);
            }
            return new FileStreamResult(ms, item.FileName);


            //return RedirectToAction("Download");
        }



        [HttpPost]
        public ActionResult ViewPDF(int? id)
        {
            var r = db.owners.Find(id);
            //PDF pDF = db.PDFs.Find(id);
            string embed = "<object data=\"{0}\" type=\"application/pdf\" width=\"500px\" height=\"300px\">";
            embed += "If you are unable to view file, you can download from <a href = \"{0}\">here</a>";
            embed += " or download <a target = \"_blank\" href = \"http://get.adobe.com/reader/\">Adobe PDF Reader</a> to view the file.";
            embed += "</object>";
            TempData["Embed"] = string.Format(embed, VirtualPathUtility.ToAbsolute(r.FileName));

            return RedirectToAction("Index");
        }

        public ActionResult Approve(int? id)
        {
            Owner owner = db.owners.Where(p => p.ownerID == id).FirstOrDefault();
            if (owner.Status == "Approved" || owner.Status == "Rejected")
            {
                TempData["AlertMessage"] = "Th application has already been Rejected/Approved";
                return RedirectToAction("AdminIndex");
            }
            else
            {
                var amount = _subscriptionPriceService.GetSubscriptionPrices().Select(p => p.Price).Max();
                ApprovedOwnerss approvedOwnerss = new ApprovedOwnerss();
                approvedOwnerss.ownerID = owner.ownerID;
                approvedOwnerss.FullName = owner.FullName;
                approvedOwnerss.LastName = owner.LastName;
                approvedOwnerss.Email = owner.Email;
                approvedOwnerss.IDNumber = owner.IDNumber;
                approvedOwnerss.Phone = owner.Phone;
                approvedOwnerss.Type = owner.Type;
                approvedOwnerss.FileName = owner.FileName;
                approvedOwnerss.AltContactNumber = owner.AltContactNumber;
                approvedOwnerss.Status = "Approved";
                approvedOwnerss.Price = amount;
                db.ApprovedOwners.Add(approvedOwnerss);
                db.SaveChanges();
                //var userId = User.;
                //var nno = db.owners.ToList().Where(p => p.Email == approvedOwnerss.Email).Select(p => p.FullName).FirstOrDefault();
                //var user = db.owners.Find(owner.UserId);

                var mailTo = new List<MailAddress>();
                mailTo.Add(new MailAddress(owner.Email, owner.FullName));
                var body = $"Hello {owner.FullName}, Congratulations. We are glad to inform you that your application has been approved. You can now procced to adding your building details. You are required to pay the Subscription Fee in order for your building to be active to the Tenants <br/> Regards,<br/><br/> HomeLink <br/> .";

                Accommodation.Services.Implementation.EmailService emailService = new Accommodation.Services.Implementation.EmailService();
                emailService.SendEmail(new EmailContent()
                {
                    mailTo = mailTo,
                    mailCc = new List<MailAddress>(),
                    mailSubject = "Application Statement | Ref No.:" + owner.ownerID,
                    mailBody = body,
                    mailFooter = "<br/> Many Thanks, <br/> <b>Alliance</b>",
                    mailPriority = MailPriority.High,
                    mailAttachments = new List<Attachment>()

                });
                db.owners.Remove(owner);
                db.SaveChanges();
                //UserManager.AddToRole(user.Id, "Landlord");
                _userManager.AddToRole(owner.UserId, "Landlord");
                TempData["AlertMessage"] = $"{owner.FullName} has been successfully approved";

                return RedirectToAction("AdminIndex");
            }

        }
        public ActionResult Reject(int? id)
        {
            Owner owner = db.owners.Where(p => p.ownerID == id).FirstOrDefault();
            if (owner.Status == "Rejected" || owner.Status == "Approved")
            {
                TempData["AlertMessage"] = "Th application has already been Rejected";
                return RedirectToAction("AdminIndex");
            }
            else
            {
                owner.Status = "Rejected";
                db.Entry(owner).State = EntityState.Modified;
                db.SaveChanges();
                var mailTo = new List<MailAddress>();
                mailTo.Add(new MailAddress(owner.Email, owner.FullName));
                var body = $"Good Day {owner.FullName}, we apologize to inform you that your application has been rejected because your documentation(Title Deed) is incorrect.";

                Accommodation.Services.Implementation.EmailService emailService = new Accommodation.Services.Implementation.EmailService();
                emailService.SendEmail(new EmailContent()
                {
                    mailTo = mailTo,
                    mailCc = new List<MailAddress>(),
                    mailSubject = "Application Statement  | Ref No.:" + owner.ownerID,
                    mailBody = body,
                    mailFooter = "<br/> Many Thanks, <br/> <b>Alliance</b>",
                    mailPriority = MailPriority.High,
                    mailAttachments = new List<Attachment>()

                });
                TempData["AlertMessage"] = $"{owner.FullName} has been rejected";

                return RedirectToAction("AdminIndex");
            }
        }

        // GET: Owners/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Owner owner = db.owners.Find(id);
            if (owner == null)
            {
                return HttpNotFound();
            }
            return View(owner);
        }

        // GET: Owners/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Owners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ownerID,FullName,LastName,IDNumber,Type,Email,Phone,Nationality,Gender,AltContactNumber")] Owner owner)
        {
            if (ModelState.IsValid)
            {
                db.owners.Add(owner);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(owner);
        }

        // GET: Owners/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Owner owner = db.owners.Find(id);
            if (owner == null)
            {
                return HttpNotFound();
            }
            return View(owner);
        }

        // POST: Owners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ownerID,FullName,LastName,IDNumber,Type,Email,Phone,Nationality,Gender,AltContactNumber")] Owner owner)
        {
            if (ModelState.IsValid)
            {
                db.Entry(owner).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(owner);
        }

        // GET: Owners/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Owner owner = db.owners.Find(id);
            if (owner == null)
            {
                return HttpNotFound();
            }
            return View(owner);
        }

        // POST: Owners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Owner owner = db.owners.Find(id);
            db.owners.Remove(owner);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        private readonly PayFastSettings payFastSettings;


        #region Constructor

        public OwnersController()
        {
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

            // Merchant Details
            onceOffRequest.merchant_id = this.payFastSettings.MerchantId;
            onceOffRequest.merchant_key = this.payFastSettings.MerchantKey;
            onceOffRequest.return_url = this.payFastSettings.ReturnUrl;
            onceOffRequest.cancel_url = this.payFastSettings.CancelUrl;
            onceOffRequest.notify_url = this.payFastSettings.NotifyUrl;

            // Buyer Details
            onceOffRequest.email_address = "sbtu01@payfast.co.za";
            //double amount = Convert.ToDouble(db.Items.Select(x => x.CostPrice).FirstOrDefault());
            //var products = db.Items.Select(x => x.Name).ToList();
            // Transaction Details
            onceOffRequest.m_payment_id = "";
            onceOffRequest.amount = 1500;
            onceOffRequest.item_name = "Once off option";
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
            #endregion Methods
            // Buyer Details
            adHocRequest.email_address = "sbtu01@payfast.co.za";
            //double amount = Convert.ToDouble(db.FoodOrders.Select(x => x.Total).FirstOrDefault());
            //var products = db.FoodOrders.Select(x => x.UserEmail).ToList();
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


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
