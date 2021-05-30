using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace AargonTools.Models
{
    //[Keyless]
    [Table("debtor_bad_numbers")]
    public partial class DebtorBadNumber
    {
        [Key]
        [Required]
        [Column("debtor_acct")]
        [StringLength(15)]
        public string DebtorAcct { get; set; }
        [Column("home_area_code")]
        [StringLength(3)]
        public string HomeAreaCode { get; set; }
        [Column("home_phone")]
        [StringLength(7)]
        public string HomePhone { get; set; }
        [Column("time_attempted", TypeName = "datetime")]
        public DateTime TimeAttempted { get; set; }
        [Column("reason")]
        [StringLength(20)]
        public string Reason { get; set; }
    }
}
