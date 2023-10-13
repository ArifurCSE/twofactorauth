using System.Data.Entity;
using TwoFAAPI.Data;
using TwoFAAPI.Interface;
using TwoFAAPI.Utility;

namespace TwoFAAPI.Implementations
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IConfiguration _configuration;
        private readonly UtilityManager utilityManager;
        private readonly FADBContext _dbContext;
        public AuthRepository(IConfiguration config, FADBContext dbContext)
        {

            _configuration = config;
            utilityManager = new UtilityManager();
            _dbContext = dbContext;
        }


        //Implementation of GetUser2faAuthTotpSecretKey:

        public async Task<UserModel> GetUser2faAuthTotpSecretKey(int userId)
        {
            try
            {
                var user = (from p in _dbContext.HrEmp.Where(p => p.EmpId == userId)
                            select new UserModel
                            {
                                EmpId = p.EmpId,
                                //Fname = p.Fname,
                                //Lname = p.Lname,
                                //EmailOfficial = p.EmailOfficial,
                                Is2faenable = p.Is2faenable,
                            }).FirstOrDefault();
                if (user != null)
                {
                    user.TotpSecretkey = utilityManager.Get2faAuthTotpSecretKey();
                }
                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //Implementation of User 2fa Verifying OTP By Secret key:

        public async Task<bool> User2faVerifyingTOTPBySecretkey(TwoFaVerifyDto twoFaVerifyDto)
        {
            try
            {
                bool isValid = utilityManager.GetUser2faAuthVerifyingTOTP(twoFaVerifyDto.TotpSecretkey, twoFaVerifyDto.TotpVerifyCode);
                return isValid;
            }
            catch (Exception)
            {

                throw;
            }
        }

        //Implementation of User2faAuth:

        public async Task<bool> User2faAuth(UserModel user)
        {
            try
            {
                var hrEmp = await _dbContext.HrEmp.FindAsync(user.EmpId);
                if (hrEmp != null)
                {
                    hrEmp.Is2faenable = user.Is2faenable;
                    string topskey = hrEmp.TotpSecretkey;
                    if (!string.IsNullOrEmpty(user.TotpSecretkey))
                        hrEmp.TotpSecretkey = user.TotpSecretkey;
                    if (!string.IsNullOrEmpty(user.BackupCode))
                    {
                        string enypcode = utilityManager.EncryptString(topskey, user.BackupCode);
                        HrEmpTwoFABackupcode hrEmpTwoFABackupcode = await _dbContext.HrEmpTwoFABackupcode.Where(e => e.EmpId == hrEmp.EmpId && e.BackupCode == enypcode).FirstOrDefaultAsync();
                        if (hrEmpTwoFABackupcode is not null)
                        {
                            if (Convert.ToBoolean(hrEmpTwoFABackupcode.IsUsed))
                            {
                                throw new Exception("Your Verify Code is already used!");
                            }
                            else
                            {
                                hrEmpTwoFABackupcode.IsUsed = true; hrEmpTwoFABackupcode.LastModifiedBy = hrEmp.EmpId; hrEmpTwoFABackupcode.LastModifiedOn = DateTime.Now;
                                await _dbContext.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            throw new Exception("Your Verify Code is invalid!");
                        }
                    }
                    if ((hrEmp.Is2faenable ?? false) == true)
                    {
                        hrEmp.TotpEnableon = user.TotpEnableon;
                    }
                    else
                    {
                        hrEmp.TotpDisableon = user.TotpDisableon; hrEmp.TotpDisablereason = user.TotpDisablereason;
                    }
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        //Implementation of GetUser2faAuthVerifyingTOTP:

        public async Task<bool> GetUser2faAuthVerifyingTOTP(UserModel user)
        {
            try
            {
                var userData = _dbContext.HrEmp.Where(p => p.EmpId == user.EmpId).FirstOrDefault();
                bool isValid = utilityManager.GetUser2faAuthVerifyingTOTP(userData.TotpSecretkey, user.TotpVerifyCode);
                return isValid;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //Implementation of Get2faAuthBackupCodes:

        public async Task<List<TwoFABackupCode>> Get2faAuthBackupCodes(UserModel user)
        {
            try
            {
                List<HrEmpTwoFABackupcode> hrEmpTwos = new List<HrEmpTwoFABackupcode>();
                var lstBC = new List<TwoFABackupCode>();
                var bcLst = utilityManager.Get2faAuthTotpBackupCodes(5);
                HrEmp hrEmp = await _dbContext.HrEmp.FindAsync(user.EmpId);
                if (string.IsNullOrEmpty(hrEmp.TotpSecretkey))
                {
                    throw new Exception("Your Secretkey not found");
                }
                hrEmpTwos = await _dbContext.HrEmpTwoFABackupcode.Where(e => e.EmpId == user.EmpId).ToListAsync();
                if (hrEmpTwos.Any())
                {
                    _dbContext.HrEmpTwoFABackupcode.RemoveRange(hrEmpTwos); await _dbContext.SaveChangesAsync();
                }
                #region Save to HR_EMPTWOFABACKUPCODE 
                hrEmpTwos = new List<HrEmpTwoFABackupcode>();
                foreach (string c in bcLst)
                {
                    var objBC = new TwoFABackupCode();
                    objBC.EmpId = user.EmpId;
                    objBC.BackupCode = c;
                    lstBC.Add(objBC);
                    hrEmpTwos.Add(new HrEmpTwoFABackupcode()
                    {
                        EmpId = user.EmpId,
                        BackupCode = utilityManager.EncryptString(hrEmp.TotpSecretkey, c), // TODO Encrypted c
                        IsUsed = false,
                        //OrgId = user.OrgId,
                        CreatedBy = user.EmpId,
                        CreatedOn = DateTime.Now
                    });
                }
                await _dbContext.HrEmpTwoFABackupcode.AddRangeAsync(hrEmpTwos);
                await _dbContext.SaveChangesAsync();
                #endregion
                return lstBC;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //Implementation of User2faAuthBackupCodesVerify:

        public async Task<bool> User2faAuthBackupCodesVerify(UserModel user)
        {
            try
            {

                ////Verify with dycript secret key 
                bool res = true;
                var userData = _dbContext.HrEmp.Where(p => p.EmpId == user.EmpId).FirstOrDefault();
                if (string.IsNullOrEmpty(userData.TotpSecretkey))
                {
                    throw new Exception("Your Secretkey not found");
                }

                if (!string.IsNullOrEmpty(user.TotpVerifyCode))
                {
                    string enypcode = utilityManager.EncryptString(userData.TotpSecretkey, user.TotpVerifyCode);
                    HrEmpTwoFABackupcode hrEmpTwoFABackupcode = await _dbContext.HrEmpTwoFABackupcode.Where(e => e.EmpId == userData.EmpId && e.BackupCode == enypcode).FirstOrDefaultAsync();
                    if (hrEmpTwoFABackupcode is not null)
                    {

                        if (Convert.ToBoolean(hrEmpTwoFABackupcode.IsUsed))
                        {
                            res = false;
                            throw new Exception("Your Verify Code is already used!");
                        }
                        else
                        {
                            hrEmpTwoFABackupcode.IsUsed = true;
                            hrEmpTwoFABackupcode.LastModifiedBy = userData.EmpId;
                            hrEmpTwoFABackupcode.LastModifiedOn = DateTime.Now;
                            await _dbContext.SaveChangesAsync();
                        }

                    }
                    else
                    {
                        throw new Exception("Your Verify Code is invalid!");
                    }
                }
                else
                {
                    throw new Exception("Verify Code not found");
                }
                //userData.TotpSecretkey
                //user.BackupCode
                //Table:HR_EMPTWOFABACKUPCODE,check with (IS_USED=false)
                //set IS_USED=true

                return res;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //Implementation of User2faAuthDisable:

        public async Task<bool> User2faAuthDisable(int empId)
        {
            try
            {
                var userData = _dbContext.HrEmp.Where(p => p.EmpId == empId).FirstOrDefault();
                if (userData is not null)
                {
                    userData.Is2faenable = false;
                    //userData.LastModifiedBy = empId;
                    //userData.LastModifiedOn = DateTime.Now;
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("User Not Found");
                }
                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<bool> IsUser2faAuthEnable(UserModel user)
        {
            //try
            //{
            //    var userData = await _dbContext.HrEmp.Where(p => p.EmpId == user.EmpId).FirstOrDefaultAsync();

            //    if (userData != null) { 
            //        return userData.Is2faenable;
            //    }
            //}
            //catch (Exception)
            //{

            //    throw;
            //}

            return false;
        }

        public async Task<HrEmpprofile> GetUserProfile(int empId)
        {
            try
            {
                HrEmpprofile hrEmpprofile = new HrEmpprofile();
                HrEmp emp = new HrEmp();
                hrEmpprofile.EmpId = empId;
                emp = await _dbContext.HrEmp.Where(e => e.EmpId == empId).FirstOrDefaultAsync();
                hrEmpprofile.Emp = new UserModel()
                {
                    EmpId =  empId,
                    FullName = emp.FullName,
                    Email = emp.Email

                };
                return hrEmpprofile;    
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
