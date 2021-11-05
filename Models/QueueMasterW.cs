using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AargonTools.Interfaces;

#nullable disable

namespace AargonTools.Models
{
    //[Keyless]
    [Table("queue_master_w")]
    public partial class QueueMasterW : IQueueMaster
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
