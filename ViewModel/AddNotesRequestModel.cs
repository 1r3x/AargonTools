using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AargonTools.ViewModel
{
    public class AddNotesRequestModel
    {
        [Required]
        [Column("debtor_acct")]
        [StringLength(15)]
        public string DebtorAcct { get; set; }
        [Column("employee")]
        public int Employee { get; set; }
        [Required]
        [Column("activity_code")]
        [StringLength(3)]
        public string ActivityCode { get; set; }
        [Required]
        [Column("note_text")]
        [StringLength(255)]
        public string NoteText { get; set; }
    }
}
