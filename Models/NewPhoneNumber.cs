using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace AargonTools.Models
{
    //[Keyless]
    [Table("new_phone_numbers")]
    [Index(nameof(DebtorAcct), Name = "x_debtor_acct")]
    public partial class NewPhoneNumber
    {
        [Key]
        [Required]
        [Column("debtor_acct")]
        [StringLength(15)]
        public string DebtorAcct { get; set; }
        [Required]
        [Column("number_type")]
        [StringLength(10)]
        public string NumberType { get; set; }
        [Required]
        [Column("area_code")]
        [StringLength(3)]
        public string AreaCode { get; set; }
        [Required]
        [Column("phone_num")]
        [StringLength(7)]
        public string PhoneNum { get; set; }
        [Required]
        [Column("source")]
        [StringLength(20)]
        public string Source { get; set; }
        [Column("entered_by")]
        public int EnteredBy { get; set; }
        [Column("date_acquired", TypeName = "datetime")]
        public DateTime DateAcquired { get; set; }
        [Column("date_used", TypeName = "datetime")]
        public DateTime? DateUsed { get; set; }
    }
}
