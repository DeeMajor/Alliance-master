using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;
//using SendGrid.Helpers.Mail;


namespace Accommodation.Models
{
    public class Email
    {
        //overload with what you want to show in the email.
        public void SendConfirmation(string email, string Name)
        {
            try
            {
                var myMessage = new SendGridMessage
                {
                    From = new EmailAddress("no-reply@homify.co.za", "Alliance")
                };


                myMessage.AddTo(email);
                string subject = "Application Status: ";
                string body = (
                    "Dear " + Name + "<br/>" + "<br/>" +               
                    "Many Thanks, " +
                    "<br/>" +
                    "Alliance Team ");

                myMessage.Subject = subject;
                myMessage.HtmlContent = body;

                var transportWeb = new SendGrid.SendGridClient("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");

                transportWeb.SendEmailAsync(myMessage);
            }
            catch (Exception x)
            {
                Console.WriteLine(x);
            }

        }

    }


}
