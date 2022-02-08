using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace AargonTools.Models
{
    [Keyless]
    [Table("debtor_pp_info")]
    [Index(nameof(BpDate), Name = "x_bp_date")]
    [Index(nameof(PpDate1), Name = "x_pp_date")]
    public partial class DebtorPpInfo
    {
        [Required]
        [Column("debtor_acct")]
        [StringLength(15)]
        public string DebtorAcct { get; set; }
        [Column("promise_type")]
        [StringLength(30)]
        public string PromiseType { get; set; }
        [Column("what")]
        [StringLength(30)]
        public string What { get; set; }
        [Column("pp_amount1", TypeName = "money")]
        public decimal? PpAmount1 { get; set; }
        [Column("pp_date1", TypeName = "datetime")]
        public DateTime? PpDate1 { get; set; }
        [Required]
        [Column("monthly1")]
        [StringLength(1)]
        public string Monthly1 { get; set; }
        [Column("num_payments1")]
        public int? NumPayments1 { get; set; }
        [Required]
        [Column("pending_dp")]
        [StringLength(1)]
        public string PendingDp { get; set; }
        [Column("pp_amount2", TypeName = "money")]
        public decimal? PpAmount2 { get; set; }
        [Column("pp_date2", TypeName = "datetime")]
        public DateTime? PpDate2 { get; set; }
        [Required]
        [Column("monthly2")]
        [StringLength(1)]
        public string Monthly2 { get; set; }
        [Column("num_payments2")]
        public int? NumPayments2 { get; set; }
        [Column("pp_amount3", TypeName = "money")]
        public decimal? PpAmount3 { get; set; }
        [Column("pp_date3", TypeName = "datetime")]
        public DateTime? PpDate3 { get; set; }
        [Column("bp_date", TypeName = "datetime")]
        public DateTime? BpDate { get; set; }
        [Column("_where")]
        [StringLength(30)]
        public string Where { get; set; }
        [Column("how")]
        [StringLength(30)]
        public string How { get; set; }
    }
}
