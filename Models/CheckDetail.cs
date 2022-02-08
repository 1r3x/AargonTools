using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace AargonTools.Models
{
    [Keyless]
    [Table("check_detail")]
    [Index(nameof(CheckCode), Name = "x_check_code", IsUnique = true)]
    public partial class CheckDetail
    {
        [Required]
        [Column("debtor_acct")]
        [StringLength(15)]
        public string DebtorAcct { get; set; }
        [Column("bank_address_code")]
        public int BankAddressCode { get; set; }
        [Column("check_code")]
        public int CheckCode { get; set; }
        [Column("day_of_month1")]
        public byte? DayOfMonth1 { get; set; }
        [Column("day_of_month2")]
        public byte? DayOfMonth2 { get; set; }
        [Column("num_checks")]
        public int NumChecks { get; set; }
        [Column("check_num")]
        public int CheckNum { get; set; }
        [Column("check_amt", TypeName = "money")]
        public decimal CheckAmt { get; set; }
        [Column("check_date", TypeName = "datetime")]
        public DateTime CheckDate { get; set; }
        [Required]
        [Column("payee")]
        [StringLength(40)]
        public string Payee { get; set; }
        [Required]
        [Column("payor_first_name")]
        [StringLength(20)]
        public string PayorFirstName { get; set; }
        [Required]
        [Column("payor_last_name")]
        [StringLength(30)]
        public string PayorLastName { get; set; }
        [Column("payor_phone")]
        [StringLength(10)]
        public string PayorPhone { get; set; }
        [Column("payor_address")]
        [StringLength(50)]
        public string PayorAddress { get; set; }
        [Column("payor_city")]
        [StringLength(30)]
        public string PayorCity { get; set; }
        [Column("payor_state")]
        [StringLength(2)]
        public string PayorState { get; set; }
        [Column("payor_zip")]
        [StringLength(10)]
        public string PayorZip { get; set; }
        [Column("micr")]
        [StringLength(34)]
        public string Micr { get; set; }
        [Required]
        [Column("add_admin_fee")]
        [StringLength(1)]
        public string AddAdminFee { get; set; }
        [Column("admin_fee", TypeName = "money")]
        public decimal? AdminFee { get; set; }
        [Required]
        [Column("sif")]
        [StringLength(1)]
        public string Sif { get; set; }
        [Column("employee")]
        public int Employee { get; set; }
        [Column("comment")]
        [StringLength(255)]
        public string Comment { get; set; }
        [Column("create_date", TypeName = "datetime")]
        public DateTime? CreateDate { get; set; }
        [Column("business_check")]
        [StringLength(1)]
        public string BusinessCheck { get; set; }
    }
}
