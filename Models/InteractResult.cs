using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace AargonTools.Models
{
    //[Keyless]
    [Table("interact_results")]
    public partial class InteractResult
    {
        [Key]
        [Column("debtor_acct")]
        [StringLength(15)]
        public string DebtorAcct { get; set; }
        [Column("ANI")]
        [StringLength(20)]
        public string Ani { get; set; }
        [Column("start_time", TypeName = "datetime")]
        public DateTime? StartTime { get; set; }
        [Column("end_time", TypeName = "datetime")]
        public DateTime? EndTime { get; set; }
        [Column("opening_intent")]
        [StringLength(50)]
        public string OpeningIntent { get; set; }
        [Column("last_dialogue")]
        [StringLength(50)]
        public string LastDialogue { get; set; }
        [Column("transfer_reason")]
        [StringLength(50)]
        public string TransferReason { get; set; }
        [Column("payment_amt", TypeName = "money")]
        public decimal? PaymentAmt { get; set; }
        [Column("call_result")]
        [StringLength(50)]
        public string CallResult { get; set; }
        [Column("term_reason")]
        [StringLength(50)]
        public string TermReason { get; set; }
    }
}
