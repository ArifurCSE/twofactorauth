using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TwoFAAPI.Data;
using TwoFAAPI.Interface;
using TwoFAAPI.Utility;

namespace TwoFAAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
       public class AuthController : ControllerBase
    {
        private readonly FADBContext _dbContext;
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration configuration;
        private readonly UtilityManager _utilityManager;
        public AuthController(FADBContext dbContex, IAuthRepository authRepository)
        {
            _dbContext = dbContex;
            _authRepository = authRepository;          
            _utilityManager = new UtilityManager();
        }
    
   

        #region 2fa Auth        
        [HttpPost]
        [Route("User2faAuth")]
        public async Task<IActionResult> User2faAuth(UserModel user)
        {
            try
            {
                bool isSuccess = await _authRepository.User2faAuth(user);
                return Ok(isSuccess);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpPost]
        [Route("User2faVerifyingTOTPBySecretkey")]
        public async Task<IActionResult> User2faVerifyingTOTPBySecretkey(TwoFaVerifyDto twoFaVerifyDto)
        {
            try
            {
                bool isSuccess = await _authRepository.User2faVerifyingTOTPBySecretkey(twoFaVerifyDto);
                return Ok(isSuccess);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpGet]
        [Route("GetUser2faAuthTotpSecretKey/{userId}")]

        public async Task<IActionResult> GetUser2faAuthTotpSecretKey(int userId)
        {

            try
            {
                UserModel user = await _authRepository.GetUser2faAuthTotpSecretKey(userId);
                return Ok(user);

            }
            catch (Exception)
            {
                throw;
            }

        }
        [HttpPost]
        [Route("GetUser2faAuthVerifyingTOTP")]

        public async Task<IActionResult> GetUser2faAuthVerifyingTOTP(UserModel user)
        {

            try
            {

                bool isVerified = await _authRepository.GetUser2faAuthVerifyingTOTP(user);


                return Ok(isVerified);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]
        [Route("isUser2faAuthEnable")]
        public async Task<IActionResult> IsUser2faAuthEnable(UserModel user)
        {
            try
            {

                bool isSuccess = await _authRepository.IsUser2faAuthEnable(user);
                return Ok(isSuccess);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]
        [Route("Get2faAuthBackupCodes")]
        public async Task<IActionResult> Get2faAuthBackupCodes(UserModel user)
        {
            try
            {
                List<TwoFABackupCode> backupCodes = await _authRepository.Get2faAuthBackupCodes(user);
                return Ok(backupCodes);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        [HttpPost]
        [Route("User2faAuthBackupCodesVerify")]
        public async Task<IActionResult> User2faAuthBackupCodesVerify(UserModel user)
        {
            try
            {
                bool isSuccess = await _authRepository.User2faAuthBackupCodesVerify(user);
                return Ok(new CommonResponse(isSuccess, "Success"));
            }
            catch (Exception ex)
            {
                return Ok(new CommonResponse(ex.Message.ToString()));
            }

        }
        [HttpPost]
        [Route("User2faAuthDisable")]
        public async Task<IActionResult> User2faAuthDisable([FromBody] int EmpId)
        {
            try
            {
                return Ok(new CommonResponse(await _authRepository.User2faAuthDisable(EmpId), "Success"));
            }
            catch (Exception ex)
            {
                return Ok(new CommonResponse(ex.Message.ToString()));
            }
        }

        #endregion
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel userLogin)
        {
            try
            {
                if (userLogin is not null)
                {
                    HrEmp hrEmp = new HrEmp();
                    hrEmp = await _dbContext.HrEmp.Where(e => e.Email == userLogin.Email).FirstOrDefaultAsync();
                    if (hrEmp is null)
                    {
                        return NotFound();
                    }
                    return Ok(new CommonResponse(hrEmp, "Success"));

                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message.ToString());
            }

        }


        [HttpGet]
        [Route("getUserProfile/{empId}")]
        public async Task<IActionResult> GetUserProfile(int empId)
        {
            try
            {
                var userProfile = await _authRepository.GetUserProfile(empId);



                return Ok(userProfile);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserModel userLogin)
        {
            HrEmp hrEmp = new HrEmp()
            {
                Email = userLogin.Email,
                Password = userLogin.Password,
                FullName = userLogin.FullName,
                TotpSecretkey = "",
                TotpDisablereason = "",
                TotpVerifyCode = "",
                BackupCode = "",
            };
            await _dbContext.AddAsync(hrEmp);
            await _dbContext.SaveChangesAsync();
            return NotFound();
        }
    }
    }
