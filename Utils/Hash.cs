using System;
using System.Security.Cryptography;

namespace PassCrypter.Utils {

    public class Hash {
        public static readonly int PBKDF2_ITERATION_COUNT = 131072;
        public static readonly int PBKDF2_SALT_BYTE_LENGTH = 32;
        public static readonly int PBKDF2_DERIVED_BYTE_LENGTH = 32;


        public static string HMACSHA256Hex(string key, string data)
        {
            return ByteToHex(HMACSHA256(HexToByte(key), HexToByte(data)));
        }

        public static byte[] HMACSHA256(byte[] key, byte[] data) {
            using (HMACSHA256 hmac = new HMACSHA256(key))
            {
                return hmac.ComputeHash(data);
            }
        }

        /* verifies a hex string hashed by HashHexPBKDF2(string passHex) */
        public static bool VerifyPBKDF2(string passHex, string original)
        {
            string salt = original.Substring(0, PBKDF2_SALT_BYTE_LENGTH*2);

            string newHash = HashHexPBKDF2(passHex, HexToByte(salt), 
                PBKDF2_DERIVED_BYTE_LENGTH, PBKDF2_ITERATION_COUNT);

            return original.ToLower() == newHash.ToLower();
        }

        public static string HashHex256(string hex)
        {
            return ByteToHex(Sha256(HexToByte(hex)));
        }

        /* returns PBKDF2 hash with salt prepended to it */
        public static string HashHexPBKDF2(string passHex) {
            return HashHexPBKDF2(passHex, GenerateRandomSalt(PBKDF2_SALT_BYTE_LENGTH), 
                PBKDF2_DERIVED_BYTE_LENGTH, PBKDF2_ITERATION_COUNT);
        }

        public static string HashHexPBKDF2(string password, byte[] salt,
            int derivedByteLength, int iteraionCount) {
            
            string saltHex = ByteToHex(salt);

            byte[] hash = PBKDF2(HexToByte(password), salt, derivedByteLength, iteraionCount);
            string hashHex = ByteToHex(hash);

            return saltHex + hashHex;
        }

        public static byte[] PBKDF2(byte[] bytes, byte[] salt,
            int derivedByteLength, int iteraionCount) {
            byte[] hashValue;
            using (var pbkdf2 = new Rfc2898DeriveBytes(bytes, salt, iteraionCount))
            {
                hashValue = pbkdf2.GetBytes(derivedByteLength);
            }
            return hashValue;
        }

        public static byte[] Sha256(byte[] plain) {
           using(var sha256 = SHA256.Create())
           {
               return sha256.ComputeHash(plain);
           }
        }

        public static byte[] HexToByte(string hex)
        {
            if(hex.Length % 2 != 0) {
                return null;
            }

            byte[] bytes = new byte[hex.Length/2];


            for(int i = 1; i <= hex.Length-1; i += 2)
            {
                bytes[i/2] = Convert.ToByte(hex[i-1] + "" + hex[i], 16);
            }

            return bytes;
        }

        public static string ByteToHex(byte[] bytes)
        {
            string hex = BitConverter.ToString(bytes);
            return hex.Replace("-","");
        }

        public static byte[] GenerateRandomSalt()
        {
            return GenerateRandomSalt(PBKDF2_SALT_BYTE_LENGTH);
        }

        public static byte[] GenerateRandomSalt(int saltByteLength)
        {
            var csprng = RandomNumberGenerator.Create();
            var salt = new byte[saltByteLength];
            csprng.GetBytes(salt);
            return salt;
        }
    }
}