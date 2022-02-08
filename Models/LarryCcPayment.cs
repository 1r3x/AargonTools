using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace AargonTools.Models
{
    [Keyless]
    [Table("larry_cc_payments")]
    [Index(nameof(DebtorAcct), Name = "x_debtor_acct")]
    [Index(nameof(Id), Name = "x_id", IsUnique = true)]
    public partial class LarryCcPayment
    {
        [Column("debtor_acct")]
        [StringLength(11)]
        public string DebtorAcct { get; set; }
        [Column("date_process", TypeName = "datetime")]
        public DateTime? DateProcess { get; set; }
        [Column("batch")]
        [StringLength(1)]
        public string Batch { get; set; }
        [Column("company")]
        [StringLength(1)]
        public string Company { get; set; }
        [Column("employee")]
        public int? Employee { get; set; }
        [Column("card_holder")]
        [StringLength(80)]
        public string CardHolder { get; set; }
        [Column("address1")]
        [StringLength(40)]
        public string Address1 { get; set; }
        [Column("address2")]
        [StringLength(30)]
        public string Address2 { get; set; }
        [Column("city")]
        [StringLength(35)]
        public string City { get; set; }
        [Column("state_code")]
        [StringLength(2)]
        public string StateCode { get; set; }
        [Column("zip")]
        [StringLength(10)]
        public string Zip { get; set; }
        [Column("card_num")]
        [StringLength(16)]
        public string CardNum { get; set; }
        [Column("exp_month")]
        [StringLength(2)]
        public string ExpMonth { get; set; }
        [Column("exp_year")]
        [StringLength(4)]
        public string ExpYear { get; set; }
        [Column("cvv")]
        [StringLength(4)]
        public string Cvv { get; set; }
        [Column("sub_total", TypeName = "money")]
        public decimal? SubTotal { get; set; }
        [Column("total", TypeName = "money")]
        public decimal? Total { get; set; }
        [Column("status")]
        [StringLength(20)]
        public string Status { get; set; }
        [Column("approval_code")]
        [StringLength(30)]
        public string ApprovalCode { get; set; }
        [Column("results")]
        [StringLength(255)]
        public string Results { get; set; }
        [Column("verified")]
        [StringLength(1)]
        public string Verified { get; set; }
        [Column("processed")]
        [StringLength(1)]
        public string Processed { get; set; }
        [Column("id")]
        public int Id { get; set; }
        [Column("confirmed")]
        [StringLength(1)]
        public string Confirmed { get; set; }
        [Column("next_acct")]
        [StringLength(11)]
        public string NextAcct { get; set; }
        [Column("fee", TypeName = "money")]
        public decimal? Fee { get; set; }
        [Column("talk_off_for_collector")]
        public int? TalkOffForCollector { get; set; }
        [Column("create_date", TypeName = "datetime")]
        public DateTime? CreateDate { get; set; }
        [Column("ecard_num")]
        [MaxLength(100)]
        public byte[] EcardNum { get; set; }
        [Column("sms_notified")]
        [StringLength(1)]
        public string SmsNotified { get; set; }
        [Column("ecfee")]
        [MaxLength(100)]
        public byte[] Ecfee { get; set; }
        [Column("hsa")]
        [StringLength(1)]
        public string Hsa { get; set; }
    }
}
