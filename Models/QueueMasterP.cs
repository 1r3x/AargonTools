using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AargonTools.Interfaces;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace AargonTools.Models
{
    //[Keyless]
    [Table("queue_master_p")]
    [Index(nameof(DebtorAcct), Name = "x_debtor_acct", IsUnique = true)]
    public partial class QueueMasterP : IQueueMaster
    {
        [Column("employee")]
        public int? Employee { get; set; }
        [Column("priority")]
        public int? Priority { get; set; }
        [Key]//experimrnt
        [Required]
        [Column("debtor_acct")]
        [StringLength(15)]
        public string DebtorAcct { get; set; }
    }
}
