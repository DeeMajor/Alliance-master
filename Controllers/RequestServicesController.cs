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
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using PayFast;
using PayFast.AspNet;

namespace Accommodation.Controllers
{
    public class RequestServicesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public RequestServicesController()
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
        // GET: RequestServices
        public ActionResult Index()
        {
            var userName = User.Identity.GetUserName();
            var requestServices = db.RequestServices.Where(x => x.CleanerEmail == userName);
            return View(requestServices.ToList());
        }
        public ActionResult Index1()
        {
            var userName = User.Identity.GetUserName();
            var requestServices = db.RequestServices.Where(x => x.TenantEmail == userName);
            return View(requestServices.ToList());
        }
        public ActionResult TenantIndex()
        {
            var userName = User.Identity.GetUserName();
            var requestServices = db.RequestServices.Where(x => x.CleanerEmail == userName);
            return View(requestServices.ToList());
        }
        //public ActionResult requestService(int? id)
        //{
        //    CleanerRoaster request = db.CleanerRoasters.Find(id);

        //    RequestService requestService = new RequestService();
        //    requestService.serviceType = request.GetService.Type;
        //    requestService.CleanerName = $"{request.GetCleaners.FullName} {request.GetCleaners.LastName}";
        //    requestService.DateRequestingFor = request.Date;
        //    requestService.Time = request.Time;
        //    requestService.servicePrice = request.servicePrice();
        //    requestService.Description = request.Description();
        //    requestService.CleanerEmail = request.GetCleaners.Email;
        //    requestService.serviceName = request.serviceName();
        //    requestService.DateRequested = DateTime.Now.Date;
        //    db.RequestServices.Add(requestService);
        //    db.SaveChanges();

        //    Session["id"] = requestService.RequestServiceId;

        //    return View(db.RequestServices.ToList().Where(x => x.RequestServiceId == requestService.RequestServiceId));
        //}
        public ActionResult requestService(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestService requestService = db.RequestServices.Find(id);
            if (requestService == null)
            {
                return HttpNotFound();
            }
            return View(requestService);
        }
        // GET: RequestServices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestService requestService = db.RequestServices.Find(id);
            if (requestService == null)
            {
                return HttpNotFound();
            }
            return View(requestService);
        }

        // GET: RequestServices/Create
        public ActionResult Create(int? id)
        {
            Session["id"] = id;
            return View();
        }

        // POST: RequestServices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RequestServiceId,serviceType,CleanerName,DateRequestingFor,Time,servicePrice,Description,CleanerEmail,TenantEmail,DateRequested,serviceName,TimeSlot")] RequestService requestService)
        {
            var id = Convert.ToInt32(Session["id"]);
            var request = db.CleanerRoasters.Where(x => x.CleanerRoasterId == id).FirstOrDefault();
            if (ModelState.IsValid)
            {
                requestService.serviceType = request.GetService.ServiceTypes.Type;
                requestService.CleanerName = request.GetCleaners.FullName + " " + request.GetCleaners.LastName;
                requestService.servicePrice = request.servicePrice();
                requestService.Description = request.Description();
                requestService.CleanerEmail = request.GetCleaners.Email;
                requestService.serviceName = request.serviceName();
                requestService.DateRequested = DateTime.Now.Date.ToString();
                db.RequestServices.Add(requestService);
                db.SaveChanges();
                return RedirectToAction("requestService", new { id = requestService.RequestServiceId });
            }

            return View(requestService);
        }

        // GET: RequestServices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestService requestService = db.RequestServices.Find(id);
            if (requestService == null)
            {
                return HttpNotFound();
            }
            return View(requestService);
        }

        // POST: RequestServices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RequestServiceId,serviceType,CleanerName,DateRequestingFor,Time,servicePrice,Description,CleanerEmail,TenantEmail,DateRequested,serviceName,TimeSlotID")] RequestService requestService)
        {
            if (ModelState.IsValid)
            {
                db.Entry(requestService).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(requestService);
        }

        // GET: RequestServices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestService requestService = db.RequestServices.Find(id);
            if (requestService == null)
            {
                return HttpNotFound();
            }
            return View(requestService);
        }

        // POST: RequestServices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RequestService requestService = db.RequestServices.Find(id);
            db.RequestServices.Remove(requestService);
            db.SaveChanges();
            return RedirectToAction("Index");
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
            int requestService = int.Parse(Session["id"].ToString());
            RequestService roomBooking = new RequestService();
            roomBooking = db.RequestServices.Find(requestService);
            //Tenant tenant = new Tenant();
            var userName = User.Identity.GetUserName();

            var tenant = db.Tenants.Where(p => p.Email == userName).FirstOrDefault();

            var attachments = new List<System.Net.Mail.Attachment>();
            attachments.Add(new System.Net.Mail.Attachment(new MemoryStream(GeneratePDF(requestService)), "Reservation Receipt", "application/pdf"));

            var mailTo = new List<MailAddress>();
            mailTo.Add(new MailAddress(userName, tenant.FullName));
            var body = $"Hello {tenant.FullName}, please see attached receipt for the recent reservation you made. <br/>Make sure you bring along your receipt when you check in for your ride.<br/>";
            Accommodation.Services.Implementation.EmailService emailService = new Accommodation.Services.Implementation.EmailService();
            emailService.SendEmail(new EmailContent()
            {
                mailTo = mailTo,
                mailCc = new List<MailAddress>(),
                mailSubject = "Application Statement | Ref No.:" + roomBooking.RequestServiceId,
                mailBody = body,
                mailFooter = "<br/> Many Thanks, <br/> <b>Alliance</b>",
                mailPriority = MailPriority.High,
                mailAttachments = attachments

            });
            // Merchant Details
            onceOffRequest.merchant_id = this.payFastSettings.MerchantId;
            onceOffRequest.merchant_key = this.payFastSettings.MerchantKey;
            onceOffRequest.return_url = this.payFastSettings.ReturnUrl;
            onceOffRequest.cancel_url = this.payFastSettings.CancelUrl;
            onceOffRequest.notify_url = this.payFastSettings.NotifyUrl;

            // Buyer Details

            onceOffRequest.email_address = "sbtu01@payfast.co.za";
            //onceOffRequest.email_address = "sbtu01@payfast.co.za";

            // Transaction Details
            onceOffRequest.m_payment_id = "";
            onceOffRequest.amount = Convert.ToDouble(roomBooking.servicePrice);
            onceOffRequest.item_name = "Room Booking payment";
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

        public byte[] GeneratePDF(int ReservationID)
        {
            MemoryStream memoryStream = new MemoryStream();
            Document document = new Document(PageSize.A5, 0, 0, 0, 0);
            PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
            document.Open();

            int requestService = int.Parse(Session["id"].ToString());
            RequestService roomBooking = new RequestService();
            roomBooking = db.RequestServices.Find(requestService);

            iTextSharp.text.Font font_heading_3 = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.RED);
            iTextSharp.text.Font font_body = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.BaseColor.BLUE);

            // Create the heading paragraph with the headig font
            PdfPTable table1 = new PdfPTable(1);
            PdfPTable table2 = new PdfPTable(5);
            PdfPTable table3 = new PdfPTable(1);

            iTextSharp.text.pdf.draw.VerticalPositionMark seperator = new iTextSharp.text.pdf.draw.LineSeparator();
            seperator.Offset = -6f;
            // Remove table cell
            table1.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            table3.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

            table1.WidthPercentage = 80;
            table1.SetWidths(new float[] { 100 });
            table2.WidthPercentage = 80;
            table3.SetWidths(new float[] { 100 });
            table3.WidthPercentage = 80;

            PdfPCell cell = new PdfPCell(new Phrase(""));
            cell.Colspan = 3;
            table1.AddCell("\n");
            table1.AddCell(cell);
            table1.AddCell("\n\n");
            table1.AddCell(
                "\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t" +
                "Homelink \n" +
                "Email :homelink.grp18@gmail.com" + "\n" +
                "\n" + "\n");
            table1.AddCell("------------Tenant Details--------------!");

            table1.AddCell("Employee Name : \t" + roomBooking.CleanerName);
            table1.AddCell("Last Name : \t" + roomBooking.CleanerEmail);
            table1.AddCell("Identity Number : \t" + roomBooking.DateRequested);
            table1.AddCell("Phone Number : \t" + roomBooking.serviceName);

            table1.AddCell("\n------------Service Booking details--------------!\n");

            table1.AddCell("Booking # : \t" + ReservationID);
            table1.AddCell("Service Type : \t" + roomBooking.serviceType);
            table1.AddCell("Service Price : \t" + roomBooking.servicePrice.ToString("C"));
            table1.AddCell("Description: \t" + roomBooking.Description);
            table1.AddCell("Date Requested For: \t" + roomBooking.DateRequestingFor);
            table1.AddCell("Time Requested For : \t" + roomBooking.Time);

            table1.AddCell("\n");

            table3.AddCell("------------Looking forward to hear from you soon--------------!");

            //////Intergrate information into 1 document
            //var qrCode = iTextSharp.text.Image.GetInstance(reservation.QrCodeImage);
            //qrCode.ScaleToFit(200, 200);
            table1.AddCell(cell);
            document.Add(table1);
            //document.Add(qrCode);
            document.Add(table3);
            document.Close();

            byte[] bytes = memoryStream.ToArray();
            memoryStream.Close();
            return bytes;
        }
    }
}
