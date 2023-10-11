namespace TwoFAAPI.Data
{
    public class UserModel
    {
        public int EmpId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public bool? Is2faenable { get; set; }
        public string TotpSecretkey { get; set; }
        public DateTime? TotpEnableon { get; set; }
        public DateTime? TotpDisableon { get; set; }
        public string TotpDisablereason { get; set; }
        public string TotpVerifyCode { get; set; }
        public string BackupCode { get; set; }

    }
}
