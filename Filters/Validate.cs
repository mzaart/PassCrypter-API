using System;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using PassCrypter.Utils;

namespace PassCrypter.Filters
{
      public class Validate : ActionFilterAttribute
      {
            public override void OnActionExecuting(ActionExecutingContext context)
            {
                  try
                  {
                        foreach(string key in context.ActionArguments.Keys)
                        {
                              string arg = (string) context.ActionArguments[key];

                              switch(key)
                              {
                                    case "email":
                                    case "newEmail":
                                    case "emailTo":
                                          if(!Validator.Email(arg)) Reject(context);
                                          break;
                                    case "name":
                                          if(!Validator.AlphaSpace(arg)) Reject(context);
                                          break;
                                    case "password":
                                          if(!Validator.Hex(arg)) Reject(context);
                                          break;
                                    default:
                                          if(!Validator.Present(arg)) Reject(context);
                                          break;
                              }
                        }
                  }
                  catch(Exception e)
                  {
                        Reject(context);
                  }
                  
            }

            private static void Reject(ActionExecutingContext context)
            {
                  context.Result = new JsonResult(new { reason = "Invalid Request Parameters." });
            }
      }
}