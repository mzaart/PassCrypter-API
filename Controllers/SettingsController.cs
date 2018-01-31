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
    public class SettingsController : Controller
    {

      [NonAction]
      public static void AddActivity(PassManagerContext db, User user, string jsonActivity)
      {
            Activity recent = JsonConvert.DeserializeObject<Activity>(jsonActivity);

            // add activity
            Settings settings = JsonConvert.DeserializeObject<Settings>(user.settings);
            
            List<Activity> activities = settings.recentActivity;
            if(activities.Count >= 10)
                  activities.RemoveAt(activities.Count-1);
            activities.Insert(0, recent);

            user.settings = JsonConvert.SerializeObject(settings);

            db.SaveChanges();

            // alert user
            if(settings.alert)
                  MessageController.Alert(user, recent);
      }

      [HttpPost]
      public JsonResult View(string email)
      {
            try
            {
                  User user = getUser(email);
                  
                  return Json(new { 
                        name = user.name,
                        email = email,
                        settings = user.settings 
                  });
            }
            catch(Exception e)
            {
                  return Json(new { reason = "An error occurred." });
            }
      }

      [HttpPost]
      public JsonResult Update(string settingsJson, string email)
      {
            try
            {
                  using(var db = new PassManagerContext())
                  {
                        User user =  db.Users
                              .Single(u => u.email.ToLower() == email.ToLower());
                  
                        user.settings = settingsJson;

                        db.SaveChanges();

                        return Json(new { succeeded = true });
                  }
            }
            catch(Exception e)
            {
                  return Json(new { succeeded = false, reason = "An error occurred." });
            }
      }

      [NonAction]
      public void AddActivity(User user, Activity activity)
      {
            Settings settings = JsonConvert.DeserializeObject<Settings>(user.settings);

            settings.recentActivity.Insert(0, activity);
            if(settings.recentActivity.Count == 10)
            {
                  settings.recentActivity.RemoveAt(settings.recentActivity.Count-1);
            }

            user.settings = JsonConvert.SerializeObject(settings);
      }
     

      [NonAction]
      private User getUser(string email) {
            using(var db = new PassManagerContext())
            {
                  return db.Users
                        .Single(u => u.email.ToLower() == email.ToLower());
            }
      }
    }
}
