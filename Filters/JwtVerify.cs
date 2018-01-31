using System;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using PassCrypter.Jwt;

namespace PassCrypter.Filters
{
      public class JwtVerify : ActionFilterAttribute
      {
            public override void OnActionExecuting(ActionExecutingContext context)
            {
                  try
                  {
                        string token = context.HttpContext.Request.Headers["Authorization"];
                        if(JwtManager.Verify(token))
                        {
                              // get email
                              var body = JsonConvert.DeserializeAnonymousType
                              (
                                    Base64UrlEncoder.Decode(token.Split('.')[1]),
                                    new { email = String.Empty }
                              );

                              context.ActionArguments.Add("email", body.email);
                        }
                        else
                        {
                              Reject(context);
                        }
                  }
                  catch(Exception e)
                  {
                        Reject(context);
                  }
            }

            private void Reject(ActionExecutingContext context)
            {
                  context.Result = new JsonResult(new { reason = "Invalid token." });
            }
      }
}