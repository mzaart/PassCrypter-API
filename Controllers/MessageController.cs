using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Cors;
using PassCrypter.Utils;
using PassCrypter.Models;
using PassCrypter.DB;
using PassCrypter.Filters;

namespace PassCrypter.Controllers
{
    [EnableCors("AllowAllPolicy")]
    [JwtVerify]
    [Validate]
    public class MessageController : Controller
    {
      [NonAction]
      public static void Alert(User user, Activity recent)
      {
            string subject = $"New Sign-in from {recent.browser} on {recent.os}";

            string body = $@"Hi {user.name},

Your PassCrypter account was just accesssed from {recent.browser} on {recent.os}.
If you don't recognize this activity, it is very likely that some one has gained unauthorized access to your account.

You are receiving this email because you've enbled Alert and Notifications from your account settings.";

            Messager.SendEmail(user.email, subject, body, ".");
      }
  
      [HttpPost]
      public JsonResult support(string name, string emailTo, string subject, string body, string email)
      {
            try
            {
                  if(!UserExists(email))
                        return Json(new { sent = false, reason = "Wrong email or password." });

                  body = "Respond To: " + emailTo + "\n" + body;
                  Messager.SendEmail(Messager.EMAIL, "PassCrypter Support: "+subject, body);

                  return Json(new { sent = true });
            }
            catch(Exception e)
            {
                  return Json(new { sent = false, reason = "An error has occurred." });
            }
      }

      [NonAction]
      private bool UserExists(string email) {
            try
            {
                  using(var db = new PassManagerContext())
                  {
                        db.Users
                        .Single(u => u.email.ToLower() == email.ToLower());
                        
                        return true;
                  }
            } catch(InvalidOperationException e)
            {
                  return false;
            }
      }
    }
}
