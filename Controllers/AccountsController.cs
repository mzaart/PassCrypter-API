using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PassCrypter.Utils;
using PassCrypter.Models;
using PassCrypter.DB;
using Microsoft.AspNetCore.Cors;
using PassCrypter.Filters;

namespace PassCrypter.Controllers
{
    [EnableCors("AllowAllPolicy")] 
    [JwtVerify]
    [Validate]
    public class AccountsController : Controller
    {

        [HttpPost]
        public JsonResult AddAccount(string accountJson, string email) {
            try 
            {
                int userId = getUserId(email);

                Account account = JsonConvert.DeserializeObject<Account>(accountJson);
            
                using (var db = new PassManagerContext())
                {
                    // check if account is already present
                    if(db.Accounts
                        .Where(a => a.websiteUrl == account.websiteUrl && a.userID == userId)
                        .ToList()
                        .Any()) return Json(new { succeeded = false, reason = "Account already exists."});
                    
                    
                    account.userID = userId;
                    
                    db.Accounts.Add(account);
                    db.SaveChanges();

                    return Json(new { succeeded = true});
                }
            } 
            catch(Exception e) 
            {
                return Json(new { succeeded = false, reason = "An error occurred."});
            }
        }

        [HttpPost]
        public JsonResult LoadAccounts(string email) {
            try
            {
                int userId = getUserId(email);

                var response = new Dictionary<string, List<Object>>();       

                using (var db = new PassManagerContext())
                {       
                    // group by tag
                    foreach(var acc in db.Accounts)
                    {
                        if(acc.userID == userId)
                        {
                            if(response.ContainsKey(acc.tag))
                                response[acc.tag].Add(new { websiteUrl = acc.websiteUrl});
                            else
                            {
                                response[acc.tag] = new List<Object>();
                                response[acc.tag].Add(new { websiteUrl = acc.websiteUrl});
                            }
                        }
                    }
                }

                return Json(response);
            } 
            catch(Exception e) 
            {
                return Json(new { reason = "An error has ocuured" });
            }
        }

        [HttpPost]
        public JsonResult LoadCreds(string websiteUrl, string email) {
            try 
            {
                int userId = getUserId(email);

                using (var db = new PassManagerContext()) {
                    Account account = db.Accounts
                                        .Single(a => a.userID == userId && a.websiteUrl == websiteUrl);
                    
                    if(account != null)
                        return Json(new { creds = account });
                    else
                        return Json(new { reason = "Not Found"});
                }
            }
            catch(Exception e) 
            {
                return Json(new { reason = "An error has ocuured" });
            }
        }

        [HttpDelete]
        public JsonResult Delete(string websiteUrl, string email) {
            try 
            {
                int userId = getUserId(email);

                using(var db = new PassManagerContext()) 
                {
                    Account acc = db.Accounts
                                    .First(a => a.userID == userId && websiteUrl == a.websiteUrl);
                
                    if(acc != null)
                    {
                        db.Accounts.Remove(acc);
                        db.SaveChanges();

                        return Json(new { succeded = true });
                    }
                    else
                        return Json(new { succeded = false, reason = "Failed to find item." });
                }
            } 
            catch(Exception e)
            {
                return Json(new { succeded = false, reason = "An error has occured" });
            }
        }

        [NonAction]
        private int getUserId(string email) {
            using(var db = new PassManagerContext())
            {
                return db.Users
                        .Single(u => u.email.ToLower() == email.ToLower())
                        .ID;
            }
        }
    }
}
