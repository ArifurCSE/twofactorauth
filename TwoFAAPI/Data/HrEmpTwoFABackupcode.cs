using System.ComponentModel.DataAnnotations;

namespace TwoFAAPI.Data
{
    public class HrEmpTwoFABackupcode
    {
        [Key]
        public int BackupCodeId { get; set; }
        public int EmpId { get; set; }
        public string BackupCode { get; set; }
        public bool? IsUsed { get; set; }
        public int? OrgId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }

    }
}
