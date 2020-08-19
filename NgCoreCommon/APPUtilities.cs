using System;

namespace NgCore.Common
{
    public class APPUtilities
    {
        public static string ConnectionString { get; set; }
        public static bool IsMySql { get; set; }

        public static string GenerateRandomString(int length)
        {
            var pw = string.Empty;

            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                var randonNo = random.Next(0, 3);

                if (randonNo == 0)
                {
                    pw += ((char)random.Next(65, 91)).ToString();
                }
                else if (randonNo == 2)
                {
                    pw += ((char)random.Next(97, 102)).ToString();
                }
                else
                {
                    pw += random.Next(0, 9);
                }
            }

            return pw;
        }
        public static string Encrypt(string key)
        {
            string res = string.Empty;
            res = SymmetricEncryption.EncryptString(key);
            return res;
        }

        public static  string Decrypt(string key)
        {
            string res = string.Empty;
            res = SymmetricEncryption.DecryptString(key);
            return res;
        }


        public static string DecryptString(string encrString)  
            {  
                byte[] b;  
                string decrypted;  
                try  
                {  
                    b = Convert.FromBase64String(encrString);  
                    decrypted = System.Text.ASCIIEncoding.ASCII.GetString(b);  
                }  
                catch (FormatException fe)   
                {  
                    decrypted = "";  
                }  
                return decrypted;  
            }  
            
            public static string EnryptString(string strEncrypted)   
            {  
                byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(strEncrypted);  
                string encrypted = Convert.ToBase64String(b);  
                return encrypted;  
            } 

    }
}