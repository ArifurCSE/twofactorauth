using TwoFAAPI.Data;

namespace TwoFAAPI.Interface
{
    public interface IAuthRepository
    {
        Task<UserModel> GetUser2faAuthTotpSecretKey(int userId);
        Task<bool> User2faVerifyingTOTPBySecretkey(TwoFaVerifyDto twoFaVerifyDto);
        Task<bool> User2faAuth(UserModel user);
        Task<bool> GetUser2faAuthVerifyingTOTP(UserModel user);
        Task<bool> IsUser2faAuthEnable(UserModel user);
        Task<List<TwoFABackupCode>> Get2faAuthBackupCodes(UserModel user);
        Task<bool> User2faAuthBackupCodesVerify(UserModel user);
        Task<bool> User2faAuthDisable(int empId);
        Task<HrEmpprofile> GetUserProfile(int empId);
    }
}
