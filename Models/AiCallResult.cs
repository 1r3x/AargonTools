using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace AargonTools.Models
{
    //[Keyless]
    [Table("AI_call_results")]
    public partial class AiCallResult
    {
        [Key]//experimrnt
        [Column("id")]
        public int Id { get; set; }
        [Column("debtor_acct")]
        [StringLength(15)]
        public string DebtorAcct { get; set; }
        [Column("call_type")]
        [StringLength(1)]
        public string CallType { get; set; }
        [Column("call_phoneNumber")]
        [StringLength(10)]
        public string CallPhoneNumber { get; set; }
        [Column("call_time", TypeName = "datetime")]
        public DateTime? CallTime { get; set; }
        [Column("call_length")]
        public int? CallLength { get; set; }
        [Column("call_paymentAmt", TypeName = "money")]
        public decimal? CallPaymentAmt { get; set; }
        [Column("call_status")]
        [StringLength(50)]
        public string CallStatus { get; set; }
        [Column("call_disposition")]
        [StringLength(100)]
        public string CallDisposition { get; set; }
        [Column("call_recording_url")]
        [StringLength(255)]
        public string CallrecordingUrl { get; set; }
        [Column("call_recording_file")]
        [StringLength(255)]
        public string CallrecordingFile { get; set; }
    }
}
