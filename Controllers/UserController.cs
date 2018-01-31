using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PassCrypter.Utils;
using PassCrypter.Models;
using PassCrypter.DB;
using Microsoft.Data.Sqlite;
using Microsoft.AspNetCore.Cors;
using PassCrypter.Filters;

namespace PassCrypter.Controllers
{
    [EnableCors("AllowAllPolicy")]
    public class UserController : Controller
    {
        [HttpPost]
        public JsonResult SignUp(string name, string email, string password) 
        {   
            try 
            {
                // check if email already exists
                if(CheckEmailExists(email)) 
                    return Json(new { succeeded = false, reason = "Email Already Exists" });

                string hash = Hash.HashHexPBKDF2(password); // hash pass
            
                using (var db = new PassManagerContext())
                {
                    db.Users.Add(new User(name, email.ToLower(), hash));
                    db.SaveChanges();

                    return Json(new { succeeded = true, token = TokenController.IssueToken(email) });
                }
            } 
            catch(Exception e) {
                return Json(new { succeeded = false, reason = "An error occured." });
            }
        }

        [HttpPost]
        public JsonResult EmailExists(string email) {
            try 
            {
                return Json(new {userExists = CheckEmailExists(email)});
            }
            catch(Exception e)
            {
                return Json(new { reason = "An error occured." });
            }
        }

        [NonAction]
        private bool CheckEmailExists(string email) {
            using (var db = new PassManagerContext())
            {
                var users = db.Users
                    .Where(u => u.email.ToLower() == email.ToLower())
                    .ToList();

                return users.Any();
            }
        }

        [HttpDelete][JwtVerify]
        public JsonResult DeleteUser(string email) {   
            try 
            {
                using (var db = new PassManagerContext())
                {
                    User user = getUser(email);
                        
                    // remove user accounts
                    var accounts = db.Accounts.Where(a => a.userID == user.ID);
                    foreach(Account acc in accounts)
                    {
                        db.Accounts.Remove(acc);
                    }

                    // remove user
                    db.Users.Remove(user);
                    db.SaveChanges();
                    
                    return Json(new { succeeded = true });
                }
            } 
            catch(Exception e) 
            {
                return Json(new { succeeded = false, reason = "An error occured" });
            }
        }

        [HttpPost][JwtVerify]
        public JsonResult updateName(string email, string name) {
            try
            {
                using(var db = new PassManagerContext())
                {
                    db.Users
                      .Single(u => u.email.ToLower() == email.ToLower())
                      .name = name;

                    db.SaveChanges();

                    return Json(new { succeeded = true });
                }
            }
            catch(Exception e)
            {
                 return Json(new { succeeded = false, reason = "An error occured" });
            }
        }

        [HttpPost][JwtVerify]
        public JsonResult updateEmail(string email, string newEmail) {
            if(CheckEmailExists(newEmail))
                return Json(new { succeeded = false, reason = "Email already exists." });

            try
            {
                using(var db = new PassManagerContext())
                {
                    db.Users
                      .Single(u => u.email.ToLower() == email.ToLower())
                      .email = newEmail;

                    db.SaveChanges();

                    return Json(new { succeeded = true, token = TokenController.IssueToken(newEmail) });
                }
            }
            catch(Exception e)
            {
                 return Json(new { succeeded = false, reason = "An error occured" });
            }
        }

        [NonAction]
        private bool isEmailValid(string email) {
            return (new EmailVerifier()).IsValidEmail(email);
        }

        [NonAction]
        private User getUser(string email) {
            try
            {
                using(var db = new PassManagerContext())
                {
                    return db.Users
                            .Single(u => u.email.ToLower() == email.ToLower());
                }
            }
            catch(System.InvalidOperationException e)
            {
                return null;
            }
        }
    }
}
