using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Cors;
using PassCrypter.Utils;
using PassCrypter.Models;
using PassCrypter.DB;
using PassCrypter.Filters;
using PassCrypter.Jwt;

namespace PassCrypter.Controllers
{
      [EnableCors("AllowAllPolicy")]
      [Validate]
      public class TokenController : Controller
      {
            [HttpPost]
            public JsonResult Issue(string email, string password, string jsonActivity)
            {
                  try
                  {
                        using (var db = new PassManagerContext())
                        {
                              User user = db.Users
                                      .Single(u => u.email.ToLower() == email.ToLower() && Hash.VerifyPBKDF2(password, u.passHash));

                              SettingsController.AddActivity(db, user, jsonActivity);
                              return Json(new { loggedIn = true, token = IssueToken(email) });
                        }
                  }
                  catch (System.InvalidOperationException e)
                  {
                        return Json(new { loggedIn = false, reason = "Invalid email or password" });
                  }
                  catch (Exception e)
                  {
                        return Json(new { loggedIn = false, reason = "An error occured" });
                  }
            }
      
            [NonAction]
            public static string IssueToken(string email) 
            {
                  return JwtManager.Issue
                  (
                        new 
                        {
                              typ = "JWT",
                              alg = "HS256"
                        },

                        new 
                        {
                              iat = DateTimeOffset.Now.ToUnixTimeSeconds(),
                              exp = DateTimeOffset.Now.ToUnixTimeSeconds()+(10*60),
                              email = email
                        }
                  );
            }
      }
}
