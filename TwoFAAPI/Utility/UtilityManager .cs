using OtpNet;
using System.Security.Cryptography;
using System.Text;

namespace TwoFAAPI.Utility
{
    public class UtilityManager
    {
        #region 2FA Auth
        public string Get2faAuthTotpSecretKey()
        {
            try
            {
                string totpSecretKey = "";

                var secret = KeyGeneration.GenerateRandomKey(20);
                totpSecretKey = Base32Encoding.ToString(secret);

                return totpSecretKey;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool GetUser2faAuthVerifyingTOTP()
        {
            try
            {
                string totpSecretKey = "";

                var secret = KeyGeneration.GenerateRandomKey(20);
                totpSecretKey = Base32Encoding.ToString(secret);

                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public bool GetUser2faAuthVerifyingTOTP(string totpSecretKey, string VerifyCode)
        {
            try
            {
                var secret = Base32Encoding.ToBytes(totpSecretKey);

                var totp = new Totp(secret);

                bool valid = totp.VerifyTotp(VerifyCode, out long timeStepMatched,
                    VerificationWindow.RfcSpecifiedNetworkDelay);



                return valid;
            }
            catch (Exception ex)
            {

                throw ex;
            }



        }

        public List<string> Get2faAuthTotpBackupCodes(int NoofCode)
        {
            try
            {
                var codes = new List<string>();
                var rng = new Random();

                for (var i = 0; i < NoofCode; i++)
                {
                    var code = new string(Enumerable.Repeat("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ", 6)
                        .Select(s => s[rng.Next(s.Length)]).ToArray());
                    codes.Add(code);
                }

                return codes;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }



        public string EncryptString(string key, string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }



        #endregion
    }
}
