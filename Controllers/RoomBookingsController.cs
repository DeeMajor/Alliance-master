using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Drawing;
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
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using PayFast;
using PayFast.AspNet;
using QRCoder;

namespace Accommodation.Controllers
{
    public class RoomBookingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private IRoomService _roomService;

        private readonly IAppointmentService _appointmentService;
        private int _buildingId;

        public RoomBookingsController(IRoomService roomService,IAppointmentService appointmentService)
        {
            _roomService = roomService;
            _appointmentService = appointmentService;
            _buildingId = 0;
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

        // GET: RoomBookings
        public ActionResult Index()
        {
            var userName = User.Identity.GetUserName();
            var roomBookings = db.RoomBookings.Include(r => r.Room);
            if (User.IsInRole("Tenant"))
            {
                return View(roomBookings.ToList().Where(x=>x.TenantEmail==userName));

            }
            else
            {
                return View(roomBookings.ToList());

            }
        }
        public ActionResult SendEmail(int? id)
        {
            var userName = User.Identity.GetUserName();

            RoomBooking roomBooking = db.RoomBookings.Find(id);
            var tenant = db.Tenants.Where(x => x.Email == userName).FirstOrDefault();
            var mailTo = new List<MailAddress>();
            mailTo.Add(new MailAddress(userName, tenant.FullName));
            var body = $"Hello {tenant.FullName}, <br/><br/> " +
                $"Please be adviced, we will under go an inspection on {roomBooking.CheckOutDate} {roomBooking.CheckOutTime}. <br/><br/>";
            Accommodation.Services.Implementation.EmailService emailService = new Accommodation.Services.Implementation.EmailService();
            emailService.SendEmail(new EmailContent()
            {
                mailTo = mailTo,
                mailCc = new List<MailAddress>(),
                mailSubject = "Vacate Notice",
                mailBody = body,
                mailFooter = "<br/> Many Thanks, <br/> <b>Alliance Properties SA</b>",
                mailPriority = MailPriority.High,
                mailAttachments = new List<Attachment>()
            });
            TempData["AlertMessage"] = "Please check your emails.";
            return RedirectToAction("tenant","Home");
        }
        // GET: RoomBookings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomBooking roomBooking = db.RoomBookings.Find(id);
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(roomBooking.BookingId.ToString(), QRCodeGenerator.ECCLevel.H);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(5);
            roomBooking.QrCodeImage = ImageToByte(qrCodeImage);
            db.Entry(roomBooking).State = EntityState.Modified;
            db.SaveChanges();
            if (roomBooking == null)
            {
                return HttpNotFound();
            }
            return View(roomBooking);
        }

        public ActionResult Confirm(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomBooking roomBooking = db.RoomBookings.Find(id);
            if (roomBooking == null)
            {
                return HttpNotFound();
            }
            return View(roomBooking);
        }

        [Authorize]
        // GET: RoomBookings/Create
        public ActionResult Create(int id)
        {
            ViewBag.Id = id;
            _buildingId = id;
            ViewBag.BuildingId = new SelectList(db.buildings, "BuildingId", "BuildingName");
            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "RoomNumber");
            return View();
        }

        // POST: RoomBookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookingId,BuildingId,RoomId,TenantEmail,RoomPrice,Status,TenantId,DateOFBooking")] RoomBooking roomBooking)
        {
            var userName = User.Identity.GetUserName();
            var tenId = db.Tenants.Where(p => p.Email == userName).Select(p=>p.TenantId).FirstOrDefault();
            var roomPrice = _roomService.GetRooms().ToList().Where(p => p.RoomId == roomBooking.RoomId).Select(p => p.RoomPrice).FirstOrDefault();
            if (ModelState.IsValid)
            {
                //if ((Convert.ToDateTime(roomBooking.DateOFBooking))>DateTime.Now)
                //{
                    roomBooking.TenantId = tenId;
                    roomBooking.TenantEmail = userName;
                    roomBooking.BuildingId = roomBooking.GetBuildingName();
                    //roomBooking.DateOFBooking = DateTime.Now.Date;
                    roomBooking.RoomId = roomBooking.RoomId;
                    roomBooking.RoomPrice = roomPrice;
                    roomBooking.Status = "Not yet Checked In!";
                    roomBooking.BuildingAddress = roomBooking.GetBuildingAddress();
                
                db.RoomBookings.Add(roomBooking);
                    db.SaveChanges();
                
                Session["bookID"] = roomBooking.BookingId;

                return RedirectToAction("Confirm", new { id = roomBooking.BookingId });

            //}
            //else
            //{
            //    ModelState.AddModelError("", "You can not satrt on a date that has already passed");
            //    ViewBag.BuildingId = new SelectList(db.buildings, "BuildingId", "BuildingName", roomBooking.BuildingId);
            //    ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "RoomNumber", roomBooking.RoomId);
            //    return View(roomBooking);
            //}

        }

            ViewBag.BuildingId = new SelectList(db.buildings, "BuildingId", "BuildingName", roomBooking.BuildingId);
            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "RoomNumber", roomBooking.RoomId);
            return View(roomBooking);
        }

        // GET: RoomBookings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomBooking roomBooking = db.RoomBookings.Find(id);
            if (roomBooking == null)
            {
                return HttpNotFound();
            }
            ViewBag.BuildingId = new SelectList(db.buildings, "BuildingId", "BuildingName", roomBooking.BuildingId);
            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "RoomNumber", roomBooking.RoomId);
            return View(roomBooking);
        }

        // POST: RoomBookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookingId,BuildingId,RoomId,TenantEmail,RoomPrice,Status,BuildingAddress")] RoomBooking roomBooking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(roomBooking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BuildingId = new SelectList(db.buildings, "BuildingId", "BuildingName", roomBooking.BuildingId);
            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "RoomNumber", roomBooking.RoomId);
            return View(roomBooking);
        }

        // GET: RoomBookings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomBooking roomBooking = db.RoomBookings.Find(id);
            if (roomBooking == null)
            {
                return HttpNotFound();
            }
            return View(roomBooking);
        }

        // POST: RoomBookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RoomBooking roomBooking = db.RoomBookings.Find(id);
            db.RoomBookings.Remove(roomBooking);
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
        public ActionResult PaymentSuccessful(string ReservationID)
        {
            if (!String.IsNullOrEmpty(ReservationID))
            {
                var reservation = db.RoomBookings.Find(Convert.ToInt32(ReservationID));
                //Payment
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(reservation.BookingId.ToString(), QRCodeGenerator.ECCLevel.H);
                QRCode qrCode = new QRCode(qrCodeData);
                Bitmap qrCodeImage = qrCode.GetGraphic(20);

                reservation.QrCodeImage = ImageToByte(qrCodeImage);
                db.Entry(reservation).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("ReservationDetails", "RoomBookings", new { ReservationID = ReservationID });
        }
        public static byte[] ImageToByte(System.Drawing.Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }
        public ActionResult ReservationDetails(string ReservationID)
        {
            if (!String.IsNullOrEmpty(ReservationID))
            {
                var reservation = db.RoomBookings.Find(Convert.ToInt32(ReservationID));

                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(reservation.BookingId.ToString(), QRCodeGenerator.ECCLevel.H);
                QRCode qrCode = new QRCode(qrCodeData);
                Bitmap qrCodeImage = qrCode.GetGraphic(20);
                reservation.QrCodeImage = ImageToByte(qrCodeImage);
                db.Entry(reservation).State = EntityState.Modified;
                db.SaveChanges();

                var attachments = new List<Attachment>();
                attachments.Add(new Attachment(new MemoryStream(GeneratePDF(Convert.ToInt32(ReservationID))), "Laundry" +
                    "Receipt", "application/pdf"));

                var tenant1 = db.Tenants.Find(reservation.TenantId);

                var mailTo = new List<MailAddress>();
                mailTo.Add(new MailAddress(User.Identity.GetUserName(), tenant1.FullName));
                var body = $"Hello {tenant1.FullName}, please see attached receipt for the recent reservation you made. <br/>Make sure you bring along your receipt when you check in for your ride.<br/>";
                Accommodation.Services.Implementation.EmailService emailService = new Accommodation.Services.Implementation.EmailService();
                emailService.SendEmail(new EmailContent()
                {
                    mailTo = mailTo,
                    mailCc = new List<MailAddress>(),
                    mailSubject = "Application Statement | Ref No.:" + reservation.BookingId,
                    mailBody = body,
                    mailFooter = "<br/> Many Thanks, <br/> <b>Alliance</b>",
                    mailPriority = MailPriority.High,
                    mailAttachments = attachments

                });

                return View(reservation);
            }
            return View();
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
            int ReservationID = int.Parse(Session["bookID"].ToString());
            RoomBooking roomBooking = new RoomBooking();
            roomBooking = db.RoomBookings.Find(ReservationID);
            var tenant1 = db.Tenants.Find(roomBooking.TenantId);
            //Tenant tenant = new Tenant();
            //var tenant = db.Tenants.Where(p=>p.Email == User.Identity.GetUserName()).FirstOrDefault();

            var attachments = new List<Attachment>();
            attachments.Add(new Attachment(new MemoryStream(GeneratePDF(ReservationID)), "Laundry Receipt", "application/pdf"));


            var mailTo = new List<MailAddress>();
            mailTo.Add(new MailAddress(User.Identity.GetUserName(), tenant1.FullName));
            var body = $"Hello {tenant1.FullName}, please see attached receipt for the recent reservation you made. <br/>Make sure you bring along your receipt when you check in for your ride.<br/>";
            Accommodation.Services.Implementation.EmailService emailService = new Accommodation.Services.Implementation.EmailService();
            emailService.SendEmail(new EmailContent()
            {
                mailTo = mailTo,
                mailCc = new List<MailAddress>(),
                mailSubject = "Application Statement | Ref No.:" + roomBooking.BookingId,
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
            onceOffRequest.amount = Convert.ToDouble(roomBooking.RoomPrice);
            onceOffRequest.item_name = "Relocating Furniture payment";
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

            int ID = int.Parse(Session["bookID"].ToString());
            RoomBooking roomBooking = new RoomBooking();
            roomBooking = db.RoomBookings.Find(ID);
            var tenant1 = db.Tenants.Find(roomBooking.TenantId);


            //var reservation = _iReservationService.Get(Convert.ToInt64(ReservationID));
            //var user = _iUserService.Get(reservation.UserID);

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
                "Alliance Properties SA \n" +
                "Email :Alliance.grp18@gmail.com" + "\n" +
                "\n" + "\n");
            table1.AddCell("------------Tenant Details--------------!");

            table1.AddCell("Full Name : \t" + tenant1.FullName);
            table1.AddCell("Last Name : \t" + tenant1.LastName);
            table1.AddCell("Identity Number : \t" + tenant1.IDNumber);
            table1.AddCell("Phone Number : \t" + tenant1.Phone);

            table1.AddCell("\n------------Booking details--------------!\n");

            table1.AddCell("Booking # : \t" + ReservationID);
            table1.AddCell("Room Number : \t" + roomBooking.Room.RoomNumber);
            table1.AddCell("Room Price : \t" + roomBooking.RoomPrice.ToString("C"));
            table1.AddCell("Start date : \t" + roomBooking.DateOFBooking);
            table1.AddCell("Building name : \t" + roomBooking.BuildingId);
            table1.AddCell("Building Address : \t" + roomBooking.BuildingAddress);
          
            table1.AddCell("\n");

            table3.AddCell("------------Looking forward to hear from you soon--------------!");

            //////Intergrate information into 1 document
            var qrCode = iTextSharp.text.Image.GetInstance(roomBooking.QrCodeImage);
            qrCode.ScaleToFit(200, 200);
            table1.AddCell(cell);
            document.Add(table1);
            document.Add(qrCode);
            document.Add(table3);
            document.Close();

            byte[] bytes = memoryStream.ToArray();
            memoryStream.Close();
            return bytes;
        }

        [AllowAnonymous]
        public string CheckIn(string tenentEmail)
        {
            var cchktnt = db.RoomBookings.Where(x => x.TenantEmail == tenentEmail).FirstOrDefault();
            cchktnt.Status = "Checked In!!";
            db.Entry(cchktnt).State = EntityState.Modified;
            db.SaveChanges();
            return $"Checkin successful for Ref #: {cchktnt.BookingId}. Tenant can get in the Property.";
        }
    }
}
