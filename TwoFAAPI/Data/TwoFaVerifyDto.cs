namespace TwoFAAPI.Data
{
    public class TwoFaVerifyDto
    {
        public string TotpSecretkey { get; set; }
        public string TotpVerifyCode { get; set; }
    }
}
