using System;
using MailKit.Net.Smtp;
using MimeKit;

namespace PassCrypter.Utils
{
      class Messager
      {
            private static readonly string SMTP_SERVER = "smtp.gmail.com";
            private static readonly int SMTP_NUMBER_NUMBER = 587;

            public static readonly string EMAIL = "passcrypter@gmail.com";
            private static readonly string EMAIL_PASSWORD = "821e964f9ed7514f0eda95a44b013213";

            private static readonly string FROM_ADDRESS_TITLE = "PassCrypter Support";
            private static readonly string TO_ADDRESS_TITLE = "Responder Email";

            public static void SendEmail(string emailTo, string subject, string body)
            {
                  SendEmail(emailTo, subject, body, TO_ADDRESS_TITLE);
            }

            public static void SendEmail(string emailTo, string subject, string body, string toAddressTitle) 
            {
                  var mimeMessage = new MimeMessage();
                  mimeMessage.From.Add(new MailboxAddress(FROM_ADDRESS_TITLE, EMAIL));
                  mimeMessage.To.Add(new MailboxAddress(toAddressTitle, emailTo));
                  mimeMessage.Subject = subject;
                  mimeMessage.Body = new TextPart("plain")
                  {
                        Text = body
                  };

                  using (var client = new SmtpClient())
                  {
                        client.Connect(SMTP_SERVER, SMTP_NUMBER_NUMBER, false);
                        // Note: only needed if the SMTP server requires authentication 
                        // Error 5.5.1 Authentication  
                        client.Authenticate(EMAIL, EMAIL_PASSWORD);
                        client.Send(mimeMessage);
                        Console.WriteLine("The mail has been sent successfully !!");
                        client.Disconnect(true);
                  }
            }
      }     
}