using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using PassCrypter.Utils;

namespace PassCrypter.Jwt
{
      public class JwtManager
      {
            // key used to sign token
            private static readonly string Key = "cfd61b8a7397fa7c10b2ae548f5bfaef";

            //  issues a token as a base64 encoded string
            public static string Issue(Object head, Object body)
            {
                  string headEncoded = Base64UrlEncoder.Encode(JsonConvert.SerializeObject(head));
                  string bodyEncoded = Base64UrlEncoder.Encode(JsonConvert.SerializeObject(body));

                  byte[] signature = Hash.HMACSHA256
                  (
                        Hash.HexToByte(Key), 
                        Encoding.UTF8.GetBytes(headEncoded + "." + bodyEncoded)
                  );
                  string signatureEncoded = Base64UrlEncoder.Encode(signature);

                  return headEncoded + "." + bodyEncoded + "." + signatureEncoded;
            }

            public static bool Verify(string jwtTokenBase64)
            {
                  try
                  {
                        // check if format is valid
                        string regex = @"^[^\.]+\.[^\.]+\.[^\.]+$";
                        if(!(new Regex(regex).IsMatch(jwtTokenBase64)))
                              return false;

                        // check signature
                        string[] jwt = jwtTokenBase64.Split('.');

                        byte[] signature = Hash.HMACSHA256
                        (
                              Hash.HexToByte(Key), 
                              Encoding.UTF8.GetBytes(jwt[0] + "." + jwt[1])
                        );
                        if(Base64UrlEncoder.Encode(signature) != jwt[2])
                              return false;

                        // check if token is expired
                        var body = JsonConvert.DeserializeAnonymousType(Base64UrlEncoder.Decode(jwt[1]), new { exp = 0 });
                        if(DateTimeOffset.Now.ToUnixTimeSeconds() > body.exp)
                              return false;

                        return true;
                  }
                  catch(Exception e)
                  {
                        return false;
                  }
            }
      }
}