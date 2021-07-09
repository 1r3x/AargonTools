using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace AargonTools.Models
{
    [Table("api_move_logs")]
    public partial class ApiMoveLog
    {
        [Key]
        [Column("debtor_acc")]
        [StringLength(11)]
        public string DebtorAcc { get; set; }
        [Column("move_setup_id")]
        public int? MoveSetupId { get; set; }
        [Column("previous_employee")]
        public int PreviousEmployee { get; set; }
        [Column("new_employee")]
        public int NewEmployee { get; set; }
        [Column("move_date", TypeName = "datetime")]
        public DateTime MoveDate { get; set; }
    }
}
