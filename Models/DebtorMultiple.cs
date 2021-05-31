using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace AargonTools.Models
{
    [Keyless]
    [Table("debtor_multiples")]
    public partial class DebtorMultiple
    {
        [Required]
        [Column("debtor_acct")]
        [StringLength(15)]
        public string DebtorAcct { get; set; }
        [Required]
        [Column("debtor_acct2")]
        [StringLength(15)]
        public string DebtorAcct2 { get; set; }
    }
}
