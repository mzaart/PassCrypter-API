using System;
using System.Text.RegularExpressions;

namespace PassCrypter.Utils 
{
      class Validator
      {
            public static bool NullOrEmpty(string str)
            {
                  return !string.IsNullOrEmpty(str);
            }

            public static bool WhiteSpace(string str) 
            {
                  return !string.IsNullOrWhiteSpace(str);
            }

            public static bool Present(string str) 
            {
                  return NullOrEmpty(str) && WhiteSpace(str);
            }

            public static bool Alpha(string str) 
            {
                  return new Regex("^[a-zA-Z()]+$").IsMatch(str);
            }

            public static bool AlphaSpace(string str)
            {
                  return new Regex("^[a-zA-Z() ]+").IsMatch(str) && WhiteSpace(str);
            }

            public static bool Numeric(string str)
            {
                  return new Regex("^[0-9]+$").IsMatch(str);
            }

            public static bool Hex(string str)
            {
                  return new Regex("^[a-f0-9]+$").IsMatch(str);
            }

            public static bool Binary(string str)
            {
                  return new Regex("^[01]+$").IsMatch(str);
            }

            public static bool AlphaNum(string str)
            {
                  return new Regex("^([0-9]|[a-z])+([0-9a-z]+)$").IsMatch(str);
            }

            public static bool AlphaNumStrict(string str)
            {
                  return new Regex("(?=.*[0-9])(?=.*([A-Z]|[a-z]))").IsMatch(str);
            }

            public static bool Email(string str)
            {
                  return (new EmailVerifier()).IsValidEmail(str);
            }

            /*
            Criteria:
            - Atleast 8 characters long
            - Contains atleast one uppercase character
            - Contains atleast one lower case character
            - Contains atleast one number
            - Contains atleast one special character
             */
            public static bool Password(string str)
            {
                  return str.Length >= 8
                         && new Regex("(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[/*-+_@&$#%])").IsMatch(str);
            }
      }
}